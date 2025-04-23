using System;

namespace Disqord.Voice;

public class VoiceEncryptionException : Exception
{
    public VoiceEncryptionException()
    { }

    public VoiceEncryptionException(string? message)
        : base(message)
    { }

    public VoiceEncryptionException(string? message, Exception? innerException)
        : base(message, innerException)
    { }
}
