using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Voice.Api;
using Disqord.Voice.Api.Models;
using Microsoft.Extensions.Logging;
using Qommon.Pooling;

namespace Disqord.Voice.Default;

/// <summary>
///     Handles the DAVE (Discord Audio/Video E2EE) protocol state machine.
///     Based on Discord's official DaveSessionManager reference implementation.
/// </summary>
internal sealed class DaveProtocolHandler : IDisposable
{
    private const int InitTransitionId = 0;

    public DaveEncryptor Encryptor => _encryptor;

    public DaveDecryptor Decryptor => _decryptor;

    private readonly ILogger _logger;
    private readonly IVoiceGatewayClient _gateway;
    private readonly Snowflake _guildId;
    private readonly Snowflake _selfUserId;

    private DaveSession? _session;
    private readonly Dave.MlsFailureCallback _mlsFailureCallback;
    private readonly DaveEncryptor _encryptor;
    private readonly DaveDecryptor _decryptor;
    private ushort _protocolVersion;

    /// <summary>
    ///     Whether the bot has joined the MLS group (via Welcome or Commit).
    ///     Proposals cannot be processed before joining.
    /// </summary>
    private bool _hasJoinedGroup;

    /// <summary>
    ///     Maps transition ID → protocol version for pending transitions.
    ///     Multiple transitions can be in flight simultaneously.
    /// </summary>
    private readonly Dictionary<int, ushort> _pendingTransitions = new();

    /// <summary>
    ///     Tracks the latest protocol version that was prepared, used to set up
    ///     key ratchets for newly connecting users mid-transition.
    /// </summary>
    private ushort _latestPreparedTransitionVersion;

    public unsafe DaveProtocolHandler(
        IVoiceGatewayClient gateway,
        ushort protocolVersion,
        Snowflake guildId,
        Snowflake selfUserId,
        ILoggerFactory loggerFactory)
    {
        _gateway = gateway;
        _protocolVersion = protocolVersion;
        _guildId = guildId;
        _selfUserId = selfUserId;
        _logger = loggerFactory.CreateLogger($"Voice {guildId} DAVE");

        Dave.SetLoggerFactory(loggerFactory);

        _mlsFailureCallback = (source, reason, _) =>
        {
            var sourceStr = Marshal.PtrToStringUTF8((nint) source);
            var reasonStr = Marshal.PtrToStringUTF8((nint) reason);
            _logger.LogError("MLS failure: {Source} {Reason}", sourceStr, reasonStr);
        };

        _encryptor = new DaveEncryptor();
        _encryptor.SetPassthroughMode(true);

        _decryptor = new DaveDecryptor();
        _decryptor.TransitionToPassthroughMode(true);
    }

    public Task InitializeAsync(CancellationToken cancellationToken)
    {
        _logger.LogDebug("DAVE protocol handler initializing (version {0}).", _protocolVersion);

        if (_protocolVersion > 0)
        {
            HandlePrepareEpochCore(_protocolVersion, epoch: 1);
            return SendKeyPackageAsync(cancellationToken);
        }

        // Protocol version 0 means DAVE is disabled.
        PrepareDaveProtocolRatchets(InitTransitionId, 0);
        ExecuteTransitionCore(InitTransitionId);
        return Task.CompletedTask;
    }

    /// <summary>
    ///     Handles a DAVE protocol message inline within the gateway dispatch loop.
    /// </summary>
    public async Task HandleMessageAsync(VoiceGatewayMessage message, CancellationToken cancellationToken)
    {
        switch (message.Op)
        {
            case VoiceGatewayPayloadOperation.ClientConnect:
            {
                HandleClientConnect(message);
                break;
            }

            case VoiceGatewayPayloadOperation.ClientDisconnect:
            {
                HandleClientDisconnect(message);
                break;
            }

            case VoiceGatewayPayloadOperation.DaveMlsExternalSenderPackage:
            {
                HandleExternalSenderPackage(message.BinaryPayload.Span);
                break;
            }

            case VoiceGatewayPayloadOperation.DaveProtocolPrepareEpoch:
            {
                await HandlePrepareEpochAsync(message, cancellationToken).ConfigureAwait(false);
                break;
            }

            case VoiceGatewayPayloadOperation.DaveMlsProposals:
            {
                await HandleProposalsAsync(message.BinaryPayload, cancellationToken).ConfigureAwait(false);
                break;
            }

            case VoiceGatewayPayloadOperation.DaveMlsAnnounceCommitTransition:
            {
                await HandleAnnounceCommitTransitionAsync(message.BinaryPayload, cancellationToken).ConfigureAwait(false);
                break;
            }

            case VoiceGatewayPayloadOperation.DaveMlsWelcome:
            {
                await HandleWelcomeAsync(message.BinaryPayload, cancellationToken).ConfigureAwait(false);
                break;
            }

            case VoiceGatewayPayloadOperation.DaveProtocolPrepareTransition:
            {
                await HandlePrepareTransitionAsync(message, cancellationToken).ConfigureAwait(false);
                break;
            }

            case VoiceGatewayPayloadOperation.DaveProtocolExecuteTransition:
            {
                HandleExecuteTransition(message);
                break;
            }

            default:
            {
                _logger.LogWarning("Unexpected DAVE opcode {0}.", message.Op);
                break;
            }
        }
    }

    private void HandleClientConnect(VoiceGatewayMessage message)
    {
        var model = message.JsonPayload!.D!.ToType<ClientConnectJsonModel>()!;
        foreach (var userId in model.UserIds)
        {
            SetupKeyRatchetForUser(userId, _latestPreparedTransitionVersion);
        }
    }

    private void HandleClientDisconnect(VoiceGatewayMessage message)
    { }

    private void HandleExternalSenderPackage(ReadOnlySpan<byte> payload)
    {
        _logger.LogDebug("Received DAVE external sender package ({0} bytes).", payload.Length);
        _session!.SetExternalSender(payload);
    }

    private async Task HandlePrepareEpochAsync(VoiceGatewayMessage message, CancellationToken cancellationToken)
    {
        var model = message.JsonPayload!.D!.ToType<DavePrepareEpochJsonModel>()!;
        _logger.LogDebug("Preparing DAVE epoch {0} with protocol version {1}.", model.Epoch, model.ProtocolVersion);

        if (model.Epoch == 1)
        {
            _protocolVersion = (ushort) model.ProtocolVersion;
            HandlePrepareEpochCore(_protocolVersion, model.Epoch);
            await SendKeyPackageAsync(cancellationToken).ConfigureAwait(false);
        }
    }

    private void HandlePrepareEpochCore(ushort protocolVersion, int epoch)
    {
        if (epoch == 1)
        {
            _hasJoinedGroup = false;

            if (_session != null)
            {
                _session.Reset();
                _session.SetProtocolVersion(protocolVersion);
            }
            else
            {
                _session = DaveSession.Create(_mlsFailureCallback);
            }

            _session.Init(protocolVersion, _guildId, _selfUserId);
        }
    }

    private async Task HandleProposalsAsync(ReadOnlyMemory<byte> payload, CancellationToken cancellationToken)
    {
        if (!_hasJoinedGroup)
        {
            _logger.LogDebug("Skipping DAVE MLS proposals ({0} bytes) - not yet joined the MLS group.", payload.Length);
            return;
        }

        _logger.LogDebug("Processing DAVE MLS proposals ({0} bytes).", payload.Length);

        using var recognizedUserIds = GetRecognizedUserIds();

        var sendBuffer = default(RentedArray<byte>);
        using (var commitWelcome = _session!.ProcessProposals(payload.Span, recognizedUserIds.AsSpan()))
        {
            if (commitWelcome.Length > 0)
            {
                sendBuffer = RentedArray<byte>.Rent(1 + commitWelcome.Length);
                sendBuffer[0] = (byte) VoiceGatewayPayloadOperation.DaveMlsCommitWelcome;
                commitWelcome.Span.CopyTo(sendBuffer.AsSpan(1));
            }
        }

        if (sendBuffer.Length > 0)
        {
            try
            {
                _logger.LogDebug("Sending DAVE commit/welcome ({0} bytes).", sendBuffer.Length - 1);
                await _gateway.SendBinaryAsync(sendBuffer.AsMemory(), cancellationToken).ConfigureAwait(false);
            }
            finally
            {
                sendBuffer.Dispose();
            }
        }
    }

    private async Task HandleAnnounceCommitTransitionAsync(ReadOnlyMemory<byte> payload, CancellationToken cancellationToken)
    {
        var span = payload.Span;
        if (span.Length < 2)
        {
            _logger.LogWarning("DAVE announce commit transition payload too short.");
            return;
        }

        var transitionId = (int) BinaryPrimitives.ReadUInt16BigEndian(span);
        var commitData = span[2..];

        _logger.LogDebug("Received DAVE commit for transition {0} ({1} bytes).", transitionId, commitData.Length);

        var result = default(DaveCommitResult);
        try
        {
            result = _session!.ProcessCommit(commitData);

            if (result.IsFailed)
            {
                _logger.LogWarning("DAVE commit processing failed for transition {0}.", transitionId);
                await RecoverFromInvalidTransitionAsync(transitionId, cancellationToken).ConfigureAwait(false);
                return;
            }

            if (result.IsIgnored)
            {
                _logger.LogDebug("DAVE commit ignored (we were the committer) for transition {0}.", transitionId);
                return;
            }

            _hasJoinedGroup = true;

            PrepareDaveProtocolRatchets(transitionId, _session!.GetProtocolVersion());

            if (transitionId == InitTransitionId)
            {
                ExecuteTransitionCore(InitTransitionId);
            }
            else
            {
                await SendTransitionReadyAsync(transitionId, cancellationToken).ConfigureAwait(false);
            }
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogWarning(ex, "DAVE commit processing failed for transition {0}.", transitionId);
            await RecoverFromInvalidTransitionAsync(transitionId, cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            result.Dispose();
        }
    }

    private async Task HandleWelcomeAsync(ReadOnlyMemory<byte> payload, CancellationToken cancellationToken)
    {
        var span = payload.Span;
        if (span.Length < 2)
        {
            _logger.LogWarning("DAVE welcome payload too short.");
            return;
        }

        var transitionId = (int) BinaryPrimitives.ReadUInt16BigEndian(span);
        var welcomeData = span[2..];

        _logger.LogDebug("Received DAVE welcome for transition {0} ({1} bytes).", transitionId, welcomeData.Length);

        using var recognizedUserIds = GetRecognizedUserIds();
        var result = default(DaveWelcomeResult);
        try
        {
            result = _session!.ProcessWelcome(welcomeData, recognizedUserIds.AsSpan());

            if (!result.IsValid)
            {
                _logger.LogWarning("DAVE welcome processing failed for transition {0} (did not join group).", transitionId);
                await SendInvalidCommitWelcomeAsync(transitionId, cancellationToken).ConfigureAwait(false);
                await SendKeyPackageAsync(cancellationToken).ConfigureAwait(false);
                return;
            }

            _hasJoinedGroup = true;

            PrepareDaveProtocolRatchets(transitionId, _session!.GetProtocolVersion());

            if (transitionId == InitTransitionId)
            {
                ExecuteTransitionCore(InitTransitionId);
            }
            else
            {
                await SendTransitionReadyAsync(transitionId, cancellationToken).ConfigureAwait(false);
            }
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogWarning(ex, "DAVE welcome processing failed for transition {0}.", transitionId);
            await RecoverFromInvalidTransitionAsync(transitionId, cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            result.Dispose();
        }
    }

    private async Task HandlePrepareTransitionAsync(VoiceGatewayMessage message, CancellationToken cancellationToken)
    {
        var model = message.JsonPayload!.D!.ToType<DavePrepareTransitionJsonModel>()!;
        _logger.LogDebug("Preparing DAVE transition {0} with protocol version {1}.", model.TransitionId, model.ProtocolVersion);

        PrepareDaveProtocolRatchets(model.TransitionId, (ushort) model.ProtocolVersion);

        if (model.TransitionId != InitTransitionId)
        {
            await SendTransitionReadyAsync(model.TransitionId, cancellationToken).ConfigureAwait(false);
        }
    }

    private void HandleExecuteTransition(VoiceGatewayMessage message)
    {
        var model = message.JsonPayload!.D!.ToType<DaveExecuteTransitionJsonModel>()!;
        _logger.LogDebug("Executing DAVE transition {0}.", model.TransitionId);

        ExecuteTransitionCore(model.TransitionId);
    }

    private void PrepareDaveProtocolRatchets(int transitionId, ushort protocolVersion)
    {
        if (protocolVersion > 0 && _session != null)
        {
            foreach (var userId in _gateway.ConnectedUserIds)
            {
                if (userId == _selfUserId)
                    continue;

                SetupKeyRatchetForUser(userId, protocolVersion);
            }
        }

        if (transitionId == InitTransitionId)
        {
            SetupKeyRatchetForUser(_selfUserId, protocolVersion);
        }
        else
        {
            _pendingTransitions[transitionId] = protocolVersion;
        }

        _latestPreparedTransitionVersion = protocolVersion;
        _logger.LogDebug("Prepared DAVE ratchets for transition {0}, version {1}.", transitionId, protocolVersion);
    }

    private void ExecuteTransitionCore(int transitionId)
    {
        if (!_pendingTransitions.Remove(transitionId, out var protocolVersion))
        {
            if (transitionId != InitTransitionId)
            {
                _logger.LogWarning("No pending transition found for transition {0}.", transitionId);
                return;
            }

            protocolVersion = _latestPreparedTransitionVersion;
        }

        var oldVersion = _protocolVersion;
        _protocolVersion = protocolVersion;

        if (protocolVersion == 0)
        {
            _session?.Reset();
            _encryptor?.SetPassthroughMode(true);
        }
        else
        {
            _encryptor?.SetPassthroughMode(false);
            _decryptor?.TransitionToPassthroughMode(false);
        }

        SetupKeyRatchetForUser(_selfUserId, protocolVersion);

        _logger.LogInformation("DAVE transition {0} executed, version {1} -> {2}.", transitionId, oldVersion, _protocolVersion);
    }

    private void SetupKeyRatchetForUser(Snowflake userId, ushort protocolVersion)
    {
        if (protocolVersion == 0 || _session == null)
            return;

        using var ratchet = _session.GetKeyRatchet(userId);
        if (userId == _selfUserId)
        {
            _encryptor?.SetKeyRatchet(ratchet);
        }
        else
        {
            _decryptor?.TransitionToKeyRatchet(ratchet);
        }
    }

    private async Task RecoverFromInvalidTransitionAsync(int transitionId, CancellationToken cancellationToken)
    {
        _logger.LogWarning("Recovering from invalid transition {0}.", transitionId);

        _hasJoinedGroup = false;

        await SendInvalidCommitWelcomeAsync(transitionId, cancellationToken).ConfigureAwait(false);

        _session?.Reset();
        _session?.Init(_protocolVersion, _guildId, _selfUserId);

        await SendKeyPackageAsync(cancellationToken).ConfigureAwait(false);
    }

    private RentedArray<Snowflake> GetRecognizedUserIds()
    {
        var connectedUsers = _gateway.ConnectedUserIds;
        var result = RentedArray<Snowflake>.Rent(connectedUsers.Count + 1);
        var i = 0;
        foreach (var userId in connectedUsers)
        {
            result[i++] = userId;
        }

        result[i] = _selfUserId;
        return result;
    }

    private async Task SendKeyPackageAsync(CancellationToken cancellationToken)
    {
        RentedArray<byte> buffer;
        using (var keyPackage = _session!.GetMarshalledKeyPackage())
        {
            _logger.LogDebug("Sending DAVE key package ({0} bytes).", keyPackage.Span.Length);

            buffer = RentedArray<byte>.Rent(1 + keyPackage.Span.Length);
            buffer[0] = (byte) VoiceGatewayPayloadOperation.DaveMlsKeyPackage;
            keyPackage.Span.CopyTo(buffer.AsSpan(1));
        }

        try
        {
            await _gateway.SendBinaryAsync(buffer.AsMemory(), cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            buffer.Dispose();
        }
    }

    private Task SendTransitionReadyAsync(int transitionId, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Sending DAVE transition ready for transition {0}.", transitionId);

        return _gateway.SendAsync(new VoiceGatewayPayloadJsonModel
        {
            Op = VoiceGatewayPayloadOperation.DaveProtocolTransitionReady,
            D = new DaveTransitionReadyJsonModel
            {
                TransitionId = transitionId
            }
        }, cancellationToken);
    }

    private Task SendInvalidCommitWelcomeAsync(int transitionId, CancellationToken cancellationToken)
    {
        _logger.LogWarning("Sending DAVE invalid commit/welcome for transition {0}.", transitionId);

        return _gateway.SendAsync(new VoiceGatewayPayloadJsonModel
        {
            Op = VoiceGatewayPayloadOperation.DaveMlsInvalidCommitWelcome,
            D = new DaveInvalidCommitWelcomeJsonModel
            {
                TransitionId = transitionId
            }
        }, cancellationToken);
    }

    public void Dispose()
    {
        _session?.Dispose();
        _session = null;
        _encryptor?.Dispose();
        _decryptor?.Dispose();
    }
}
