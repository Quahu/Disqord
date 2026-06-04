using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Voice.Api;
using Disqord.Voice.Api.Models;
using Microsoft.Extensions.Logging;
using Qommon;
using Qommon.Pooling;

namespace Disqord.Voice.Default;

/// <summary>
///     Handles the DAVE (Discord Audio/Video E2EE) protocol state machine.
///     Based on Discord's official DaveSessionManager reference implementation.
/// </summary>
public sealed class DaveProtocolHandler : IDisposable
{
    private const int InitTransitionId = 0;

    /// <summary>
    ///     The number of consecutive DAVE decrypt failures to tolerate before triggering recovery.
    ///     Matches discord.js's DEFAULT_DECRYPTION_FAILURE_TOLERANCE.
    ///     At 50 packets/sec, 36 failures ≈ 0.72 seconds of tolerance.
    /// </summary>
    private const int DecryptionFailureTolerance = 36;

    public DaveEncryptor Encryptor => _encryptor;

    /// <summary>
    ///     Gets whether a DAVE transition is currently pending or reinitializing.
    ///     When <see langword="true"/>, decrypt failures are expected and should not be logged.
    /// </summary>
    public bool IsTransitioning => _pendingTransitionCount > 0 || _reinitializing;

    private readonly ILogger _logger;
    private readonly IVoiceGatewayClient _gateway;
    private readonly Snowflake _guildId;
    private readonly Snowflake _selfUserId;

    private DaveSession? _session;
    private readonly Dave.MlsFailureCallback _mlsFailureCallback;
    private readonly DaveEncryptor _encryptor;
    private readonly Dictionary<Snowflake, DaveDecryptor> _decryptors = new();
    private readonly ReaderWriterLockSlim _decryptorsLock = new();
    private int _disposed;
    private volatile bool _mlsFailureOccurred;
    private volatile bool _reinitializing;
    private ushort _protocolVersion;
    private readonly ushort _targetProtocolVersion;

    // Consecutive DAVE decrypt failure tracking (read from the UDP thread, written from both threads).
    private int _consecutiveDecryptFailures;
    private volatile int _lastExecutedTransitionId;
    private volatile int _pendingTransitionCount;
    private int _decryptRecoveryRequested;

    /// <summary>
    ///     The server's actual MLS group ID, extracted from incoming commit messages.
    ///     Libdave's <c>CanProcessCommit</c> checks <c>commit.group_id() != groupId_</c>,
    ///     but the server may assign a different group ID than <c>BigEndianBytesFrom(guildId)</c>.
    ///     Caching the server's group ID and using it for subsequent <c>Init</c> calls
    ///     avoids repeated <c>CanProcessCommit</c> failures and unnecessary recovery cycles.
    /// </summary>
    private Snowflake? _serverGroupId;

    /// <summary>
    ///     Whether the bot has joined the MLS group (via Welcome or Commit).
    ///     Proposals cannot be processed before joining.
    /// </summary>
    private bool _hasJoinedGroup;

    /// <summary>
    ///     Maps transition ID -> protocol version for pending transitions.
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

        // Start at version 0 (unencrypted) - the handler is created at session description time,
        // before we've joined the MLS group. The first transition (Welcome) will move to the target version.
        // This ensures the v0->v1 check in ExecuteTransitionCore triggers correctly,
        // enabling the passthrough grace window for frames that arrive before DAVE is fully established.
        _protocolVersion = 0;
        _targetProtocolVersion = protocolVersion;
        _guildId = guildId;
        _selfUserId = selfUserId;
        _logger = loggerFactory.CreateLogger($"Voice {guildId} DAVE");

        Dave.SetLoggerFactory(loggerFactory);

        _mlsFailureCallback = (source, reason, _) =>
        {
            _mlsFailureOccurred = true;
            var sourceStr = Marshal.PtrToStringUTF8((nint) source);
            var reasonStr = Marshal.PtrToStringUTF8((nint) reason);
            _logger.LogDebug("MLS failure: {Source} {Reason}", sourceStr, reasonStr);
        };

        _encryptor = new DaveEncryptor();
        _encryptor.SetPassthroughMode(true);
    }

    public Task InitializeAsync(CancellationToken cancellationToken)
    {
        _logger.LogDebug("DAVE protocol handler initializing (target version {0}).", _targetProtocolVersion);

        if (_targetProtocolVersion > 0)
        {
            HandlePrepareEpochCore(_targetProtocolVersion, epoch: 1);
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
        // Check for pending recovery triggered by excessive consecutive decrypt failures.
        if (Interlocked.Exchange(ref _decryptRecoveryRequested, 0) == 1)
        {
            var failures = Volatile.Read(ref _consecutiveDecryptFailures);
            var transitionId = _lastExecutedTransitionId;
            _logger.LogDebug("Triggering DAVE recovery due to {0} consecutive decrypt failures (transition {1}).", failures, transitionId);
            await RecoverFromInvalidTransitionAsync(transitionId, cancellationToken).ConfigureAwait(false);
        }

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

    /// <summary>
    ///     Acquires a read-locked lease on the decryptor for the specified user.
    ///     The caller <b>must</b> dispose the returned lease to release the read lock.
    /// </summary>
    public DecryptorLease GetDecryptor(Snowflake userId)
    {
        _decryptorsLock.EnterReadLock();
        _decryptors.TryGetValue(userId, out var decryptor);
        return new DecryptorLease(decryptor, _decryptorsLock);
    }

    /// <summary>
    ///     A read-lock lease over a <see cref="DaveDecryptor"/>.
    ///     Disposing this lease releases the read lock on the decryptors collection.
    /// </summary>
    public readonly struct DecryptorLease : IDisposable
    {
        public readonly DaveDecryptor? Decryptor;
        private readonly ReaderWriterLockSlim _lock;

        internal DecryptorLease(DaveDecryptor? decryptor, ReaderWriterLockSlim rwLock)
        {
            Decryptor = decryptor;
            _lock = rwLock;
        }

        public void Dispose() => _lock?.ExitReadLock();
    }

    /// <summary>
    ///     Reports a DAVE decrypt result from the UDP receive path.
    ///     Tracks consecutive failures and requests recovery when the threshold is exceeded.
    /// </summary>
    /// <remarks>
    ///     This is called from the UDP receive thread. Recovery is deferred to the gateway thread.
    /// </remarks>
    public void OnDecryptResult(bool success)
    {
        if (success)
        {
            Interlocked.Exchange(ref _consecutiveDecryptFailures, 0);
            return;
        }

        // Don't count failures when reinitializing or when transitions are pending.
        // Matches discord.js behavior.
        if (_reinitializing || _pendingTransitionCount > 0)
        {
            return;
        }

        var failures = Interlocked.Increment(ref _consecutiveDecryptFailures);
        if (failures > DecryptionFailureTolerance && _lastExecutedTransitionId > 0)
        {
            // Request recovery - will be processed on the gateway thread in HandleMessageAsync
            Volatile.Write(ref _decryptRecoveryRequested, 1);
        }
    }

    private void HandleClientConnect(VoiceGatewayMessage message)
    {
        var model = message.JsonPayload!.D!.ToType<ClientConnectJsonModel>()!;
        if (model.UserIds != null)
        {
            foreach (var userId in model.UserIds)
            {
                SetupKeyRatchetForUser(userId, _latestPreparedTransitionVersion);
            }
        }

        if (model.UserId.TryGetValue(out var singleUserId))
        {
            SetupKeyRatchetForUser(singleUserId, _latestPreparedTransitionVersion);
        }
    }

    private void HandleClientDisconnect(VoiceGatewayMessage message)
    {
        var model = message.JsonPayload!.D!.ToType<ClientDisconnectJsonModel>()!;
        if (model.UserId == _selfUserId)
        {
            return;
        }

        _decryptorsLock.EnterWriteLock();
        try
        {
            if (_decryptors.Remove(model.UserId, out var decryptor))
            {
                decryptor.Dispose();
            }
        }
        finally
        {
            _decryptorsLock.ExitWriteLock();
        }
    }

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
            // Reset to version 0 - the MLS session is being reset, so DAVE is not yet
            // established for this epoch. The next transition will be v0->v1, which
            // correctly triggers the passthrough grace window for receiving.
            var targetVersion = (ushort) model.ProtocolVersion;
            _protocolVersion = 0;
            ClearDecryptors();
            HandlePrepareEpochCore(targetVersion, model.Epoch);
            await SendKeyPackageAsync(cancellationToken).ConfigureAwait(false);
        }
    }

    private void HandlePrepareEpochCore(ushort protocolVersion, int epoch)
    {
        if (epoch == 1)
        {
            _hasJoinedGroup = false;
            _pendingTransitions.Clear();
            _pendingTransitionCount = 0;

            if (_session != null)
            {
                _session.Reset();
                _session.SetProtocolVersion(protocolVersion);
            }
            else
            {
                _session = DaveSession.Create(_mlsFailureCallback);
            }

            _session.Init(protocolVersion, _serverGroupId ?? _guildId, _selfUserId);
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

        _mlsFailureOccurred = false;

        using var recognizedUserIds = GetRecognizedUserIds();

        var sendBuffer = default(RentedArray<byte>);
        using (var commitWelcome = _session!.ProcessProposals(payload.Span, recognizedUserIds.AsSpan()))
        {
            if (!_mlsFailureOccurred && commitWelcome.Length > 0)
            {
                sendBuffer = RentedArray<byte>.Rent(1 + commitWelcome.Length);
                sendBuffer[0] = (byte) VoiceGatewayPayloadOperation.DaveMlsCommitWelcome;
                commitWelcome.Span.CopyTo(sendBuffer.AsSpan(1));
            }
        }

        if (_mlsFailureOccurred)
        {
            sendBuffer.Dispose();
            _logger.LogWarning("MLS failure detected during ProcessProposals, recovering session.");
            await RecoverSessionAsync(cancellationToken).ConfigureAwait(false);
            return;
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

        // Extract the server's MLS group ID from the commit for use in future Init calls.
        // The server may assign a group ID that differs from BigEndianBytesFrom(guildId),
        // causing CanProcessCommit to always fail. Learning the real ID lets subsequent
        // Init calls set groupId_ correctly, eliminating recovery cascades.
        if (TryExtractMlsGroupId(commitData, out var extractedGroupId) && _serverGroupId != extractedGroupId)
        {
            _logger.LogDebug("Learned server MLS group ID: 0x{0:X16}.", (ulong) extractedGroupId);
            _serverGroupId = extractedGroupId;
        }

        var result = default(DaveCommitResult);
        try
        {
            result = _session!.ProcessCommit(commitData);

            if (result.IsFailed || result.IsIgnored)
            {
                // IsFailed: the commit data is corrupt or invalid.
                // IsIgnored: the commit is for an epoch the session has already moved past,
                // typically our own echoed commit after ProcessProposals advanced the MLS state.
                // In both cases, recovery is needed because GetKeyRatchet uses currentState_
                // which only ProcessCommit or ProcessWelcome can advance via ReplaceState.
                if (result.IsFailed)
                {
                    _logger.LogWarning("DAVE commit processing failed for transition {0}.", transitionId);
                }
                else
                {
                    _logger.LogDebug("DAVE commit ignored for transition {0} (own echoed commit); recovering.", transitionId);
                }

                await RecoverFromInvalidTransitionAsync(transitionId, cancellationToken).ConfigureAwait(false);
                return;
            }

            _reinitializing = false;
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

            _reinitializing = false;
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
                {
                    continue;
                }

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
            _pendingTransitionCount = _pendingTransitions.Count;
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
                _logger.LogDebug("No pending transition found for transition {0}.", transitionId);
                return;
            }

            protocolVersion = _latestPreparedTransitionVersion;
        }

        _pendingTransitionCount = _pendingTransitions.Count;

        var oldVersion = _protocolVersion;
        _protocolVersion = protocolVersion;

        if (protocolVersion == 0)
        {
            // Downgrade to unencrypted.
            _session?.Reset();
            _encryptor.SetPassthroughMode(true);
            SetDecryptorsPassthroughMode(true);
        }
        else if (oldVersion == 0)
        {
            // Upgrading from downgraded state - enable passthrough grace window then disable.
            _encryptor.SetPassthroughMode(false);
            SetDecryptorsPassthroughMode(true);
            SetDecryptorsPassthroughMode(false);
        }
        else
        {
            // v1->v1 transition: don't touch passthrough mode.
            // Key ratchets already handle the transition. Matches discord.js behavior.
            _encryptor.SetPassthroughMode(false);
        }

        SetupKeyRatchetForUser(_selfUserId, protocolVersion);

        Interlocked.Exchange(ref _consecutiveDecryptFailures, 0);
        _reinitializing = false;
        _lastExecutedTransitionId = transitionId;
        _logger.LogInformation("DAVE transition {0} executed, version {1} -> {2}.", transitionId, oldVersion, _protocolVersion);
    }

    private void SetupKeyRatchetForUser(Snowflake userId, ushort protocolVersion)
    {
        if (protocolVersion == 0 || _session == null)
        {
            return;
        }

        using var ratchet = _session.GetKeyRatchet(userId);
        if (ratchet.Handle == 0)
        {
            return;
        }

        if (userId == _selfUserId)
        {
            _encryptor.SetKeyRatchet(ratchet);
        }
        else
        {
            _decryptorsLock.EnterWriteLock();
            try
            {
                if (!_decryptors.TryGetValue(userId, out var decryptor))
                {
                    // Do NOT enable passthrough on per-user decryptors.
                    // The native library's passthrough detection can misclassify encrypted
                    // frames as "not encrypted", leaking encrypted garbage as Opus data.
                    // Unencrypted frames during DAVE transitions will be dropped and the
                    // resulting gap is filled with silence by AudioReceiver.
                    decryptor = new DaveDecryptor();
                    _decryptors[userId] = decryptor;
                }

                decryptor.TransitionToKeyRatchet(ratchet);
            }
            finally
            {
                _decryptorsLock.ExitWriteLock();
            }

            // Reset the consecutive failure counter so expected failures during
            // key transitions don't trigger unnecessary MLS session recovery.
            Volatile.Write(ref _consecutiveDecryptFailures, 0);
        }
    }

    private void SetDecryptorsPassthroughMode(bool passthroughMode)
    {
        _decryptorsLock.EnterWriteLock();
        try
        {
            foreach (var decryptor in _decryptors.Values)
            {
                decryptor.TransitionToPassthroughMode(passthroughMode);
            }
        }
        finally
        {
            _decryptorsLock.ExitWriteLock();
        }
    }

    private void ClearDecryptors()
    {
        _decryptorsLock.EnterWriteLock();
        try
        {
            foreach (var decryptor in _decryptors.Values)
            {
                decryptor.Dispose();
            }

            _decryptors.Clear();
        }
        finally
        {
            _decryptorsLock.ExitWriteLock();
        }
    }

    private async Task RecoverSessionAsync(CancellationToken cancellationToken)
    {
        if (_reinitializing)
        {
            _logger.LogDebug("Skipping session recovery - already reinitializing.");
            return;
        }

        _logger.LogDebug("Recovering DAVE session - resetting MLS state and re-sending key package.");

        _reinitializing = true;
        _hasJoinedGroup = false;
        _pendingTransitions.Clear();
        _pendingTransitionCount = 0;
        Interlocked.Exchange(ref _consecutiveDecryptFailures, 0);
        ClearDecryptors();
        _session?.Reset();
        _session?.Init(_targetProtocolVersion, _serverGroupId ?? _guildId, _selfUserId);

        await SendKeyPackageAsync(cancellationToken).ConfigureAwait(false);
    }

    private async Task RecoverFromInvalidTransitionAsync(int transitionId, CancellationToken cancellationToken)
    {
        if (_reinitializing)
        {
            _logger.LogDebug("Skipping recovery for transition {0} - already reinitializing.", transitionId);
            return;
        }

        _logger.LogDebug("Recovering from invalid transition {0}.", transitionId);

        _reinitializing = true;
        _hasJoinedGroup = false;
        _pendingTransitions.Clear();
        _pendingTransitionCount = 0;
        Interlocked.Exchange(ref _consecutiveDecryptFailures, 0);

        await SendInvalidCommitWelcomeAsync(transitionId, cancellationToken).ConfigureAwait(false);

        ClearDecryptors();
        _session?.Reset();
        _session?.Init(_targetProtocolVersion, _serverGroupId ?? _guildId, _selfUserId);

        await SendKeyPackageAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    ///     Extracts the MLS group ID from a serialized MLS commit message.
    ///     Format: version(2) + wire_format(2) + group_id_length(1) + group_id(8).
    /// </summary>
    private static bool TryExtractMlsGroupId(ReadOnlySpan<byte> commitData, out Snowflake groupId)
    {
        groupId = default;

        // Need: version(2) + wire_format(2) + length(1) + group_id(8) = 13 bytes minimum.
        if (commitData.Length < 13)
        {
            return false;
        }

        var groupIdLength = commitData[4];
        if (groupIdLength != sizeof(ulong))
        {
            return false;
        }

        groupId = BinaryPrimitives.ReadUInt64BigEndian(commitData.Slice(5, sizeof(ulong)));
        return true;
    }

    private RentedArray<Snowflake> GetRecognizedUserIds()
    {
        var connectedUsers = _gateway.ConnectedUserIds;
        var includeSelf = !connectedUsers.Contains(_selfUserId);
        var count = connectedUsers.Count + (includeSelf ? 1 : 0);
        var result = RentedArray<Snowflake>.Rent(count);
        var i = 0;
        foreach (var userId in connectedUsers)
        {
            result[i++] = userId;
        }

        if (includeSelf)
        {
            result[i] = _selfUserId;
        }

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
        _logger.LogDebug("Sending DAVE invalid commit/welcome for transition {0}.", transitionId);

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
        if (Interlocked.Exchange(ref _disposed, 1) != 0)
            return;

        _session?.Dispose();
        _session = null;
        _encryptor.Dispose();
        ClearDecryptors();
        _decryptorsLock.Dispose();
    }
}
