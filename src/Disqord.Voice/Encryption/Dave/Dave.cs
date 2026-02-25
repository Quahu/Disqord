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

    private static readonly LogSinkCallback? _logSinkCallback;
    private static ILogger? _nativeLogger;

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

        _logSinkCallback = OnNativeLog;
        SetLogSinkCallback(_logSinkCallback);
    }

    internal static void SetLoggerFactory(ILoggerFactory loggerFactory)
    {
        if (Volatile.Read(ref _nativeLogger) != null)
            return;

        var logger = loggerFactory.CreateLogger("Voice DAVE");
        Interlocked.CompareExchange(ref _nativeLogger, logger, null);
    }

    private static void OnNativeLog(LoggingSeverity severity, byte* file, int line, byte* message)
    {
        var logger = _nativeLogger;
        if (logger == null)
            return;

        var logLevel = severity switch
        {
            LoggingSeverity.Verbose => LogLevel.Trace,
            LoggingSeverity.Info => LogLevel.Debug,
            LoggingSeverity.Warning => LogLevel.Warning,
            LoggingSeverity.Error => LogLevel.Error,
            _ => LogLevel.None
        };

        if (!logger.IsEnabled(logLevel))
            return;

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
