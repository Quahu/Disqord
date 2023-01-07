using System;

namespace Disqord.Voice;

public class VoiceConnectionException : Exception
{
    /// <inheritdoc/>
    public VoiceConnectionException()
    { }

    /// <inheritdoc/>
    public VoiceConnectionException(string? message)
        : base(message)
    { }

    /// <inheritdoc/>
    public VoiceConnectionException(string? message, Exception? innerException)
        : base(message, innerException)
    { }
}
