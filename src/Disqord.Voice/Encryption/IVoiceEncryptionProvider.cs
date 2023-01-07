using System;

namespace Disqord.Voice;

/// <summary>
///     Represents a type responsible for providing <see cref="IVoiceEncryption"/> instances.
/// </summary>
public interface IVoiceEncryptionProvider
{
    /// <summary>
    ///     Gets an encryption for the specified available encryption modes.
    /// </summary>
    /// <param name="availableEncryptionModes"> The available encryption modes. </param>
    /// <returns>
    ///     The encryption or <see langword="null"/> if no supported encryption is available.
    /// </returns>
    IVoiceEncryption? GetEncryption(Span<string> availableEncryptionModes);
}
