using System;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace Disqord.Voice;

/// <summary>
///     Represents DAVE (Discord Audio/Video Encryption) interoperability.
/// </summary>
/// <seealso href="https://daveprotocol.com/"> DAVE Protocol documentation </seealso>
public static unsafe partial class Dave
{
    private const string LibraryName = "libdave";

    /// <summary>
    ///     Gets whether the native DAVE library is available.
    /// </summary>
    public static bool IsAvailable { get; }

    /// <summary>
    ///     Gets the maximum supported DAVE protocol version from the native library.
    /// </summary>
    public static ushort MaxSupportedVersion { get; }

    private static readonly LogSinkCallback? _noOpLogSinkCallback;
    private static LogSinkCallback? _forwardingLogSinkCallback;
    private static ILogger? _nativeLogger;
    private static volatile bool _nativeLoggingEnabled;

    static Dave()
    {
        try
        {
            MaxSupportedVersion = MaxSupportedProtocolVersion();
            IsAvailable = true;
        }
        catch (DllNotFoundException)
        {
            IsAvailable = false;
        }

        if (IsAvailable)
        {
            // Suppress the default stdout logging in libdave. By default, native logs are disabled
            // and only forwarded to the ILogger when EnableNativeLogging is called.
            _noOpLogSinkCallback = static (_, _, _, _) => { };
            SetLogSinkCallback(_noOpLogSinkCallback);
        }
    }

    /// <summary>
    ///     Enables or disables native DAVE library logging.
    ///     When enabled, native logs are forwarded to the <see cref="ILoggerFactory"/>
    ///     under the <c>"Voice DAVE"</c> category at <see cref="LogLevel.Debug"/> and <see cref="LogLevel.Trace"/> levels.
    /// </summary>
    public static bool IsNativeLoggingEnabled
    {
        get => _nativeLoggingEnabled;
        set
        {
            _nativeLoggingEnabled = value;

            if (!IsAvailable)
            {
                return;
            }

            if (value && _forwardingLogSinkCallback != null)
            {
                SetLogSinkCallback(_forwardingLogSinkCallback);
            }
            else
            {
                SetLogSinkCallback(_noOpLogSinkCallback!);
            }
        }
    }

    internal static void SetLoggerFactory(ILoggerFactory loggerFactory)
    {
        if (Volatile.Read(ref _nativeLogger) != null)
        {
            return;
        }

        var logger = loggerFactory.CreateLogger("Voice DAVE");
        if (Interlocked.CompareExchange(ref _nativeLogger, logger, null) != null)
        {
            return;
        }

        _forwardingLogSinkCallback = OnNativeLog;

        if (_nativeLoggingEnabled)
        {
            SetLogSinkCallback(_forwardingLogSinkCallback);
        }
    }

    private static void OnNativeLog(LoggingSeverity severity, byte* file, int line, byte* message)
    {
        var logger = _nativeLogger;
        if (logger == null)
        {
            return;
        }

        // Downgrade all native log levels. The native library logs protocol-level
        // issues (e.g., "unexpected group", "Decrypt failed") at Error/Warning, but these are
        // expected during DAVE transitions and handled by our C# protocol handler.
        var logLevel = severity switch
        {
            LoggingSeverity.Error => LogLevel.Debug,
            LoggingSeverity.Warning => LogLevel.Debug,
            LoggingSeverity.Info => LogLevel.Debug,
            LoggingSeverity.Verbose => LogLevel.Trace,
            _ => LogLevel.None
        };

        if (!logger.IsEnabled(logLevel))
        {
            return;
        }

        var messageStr = Marshal.PtrToStringUTF8((nint) message);
        logger.Log(logLevel, "{Message}", messageStr);
    }

    public enum Codec
    {
        Unknown = 0,
        Opus = 1,
        VP8 = 2,
        VP9 = 3,
        H264 = 4,
        H265 = 5,
        AV1 = 6
    }

    public enum MediaType
    {
        Audio = 0,
        Video = 1
    }

    public enum EncryptorResultCode
    {
        Success = 0,
        EncryptionFailure = 1,
        MissingKeyRatchet = 2,
        MissingCryptor = 3,
        TooManyAttempts = 4
    }

    public enum DecryptorResultCode
    {
        Success = 0,
        DecryptionFailure = 1,
        MissingKeyRatchet = 2,
        InvalidNonce = 3,
        MissingCryptor = 4
    }

    public enum LoggingSeverity
    {
        Verbose = 0,
        Info = 1,
        Warning = 2,
        Error = 3,
        None = 4
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct EncryptorStats
    {
        public ulong PassthroughCount;
        public ulong EncryptSuccessCount;
        public ulong EncryptFailureCount;
        public ulong EncryptDuration;
        public ulong EncryptAttempts;
        public ulong EncryptMaxAttempts;
        public ulong EncryptMissingKeyCount;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DecryptorStats
    {
        public ulong PassthroughCount;
        public ulong DecryptSuccessCount;
        public ulong DecryptFailureCount;
        public ulong DecryptDuration;
        public ulong DecryptAttempts;
        public ulong DecryptMissingKeyCount;
        public ulong DecryptInvalidNonceCount;
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void MlsFailureCallback(byte* source, byte* reason, nint userData);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void PairwiseFingerprintCallback(byte* fingerprint, nuint length, nint userData);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void EncryptorProtocolVersionChangedCallback(nint userData);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void LogSinkCallback(LoggingSeverity severity, byte* file, int line, byte* message);
}
