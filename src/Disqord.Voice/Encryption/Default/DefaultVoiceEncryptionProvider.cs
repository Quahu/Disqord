using System;
using System.Collections.Generic;
using System.Linq;
using static Disqord.Voice.KnownEncryptionModes;

namespace Disqord.Voice.Default;

/// <inheritdoc/>
public class DefaultVoiceEncryptionProvider : IVoiceEncryptionProvider
{
    private readonly (string ModeName, Func<IVoiceEncryption> Factory)[] _encryptions;

    public DefaultVoiceEncryptionProvider()
    {
        _encryptions = GetSupportedEncryptions().ToArray();
    }

    private static IEnumerable<(string ModeName, Func<IVoiceEncryption> Get)> GetSupportedEncryptions()
    {
        if (AEADAes256GcmRtpSizeEncryption.IsSupported)
        {
            yield return (AEADAes256GcmRtpSize, static () => new AEADAes256GcmRtpSizeEncryption());
        }

        yield return (AEADXChaCha20Poly1305RtpSize, static () => new AEADXChaCha20Poly1305RtpSizeEncryption());

#pragma warning disable CS0618 // Type or member is obsolete
        yield return (XSalsa20Poly1305, static () => new XSalsa20Poly1305Encryption());
        yield return (XSalsa20Poly1305Lite, static () => new XSalsa20Poly1305LiteEncryption());
        yield return (XSalsa20Poly1305Suffix, static () => new XSalsa20Poly1305SuffixEncryption());
#pragma warning restore CS0618 // Type or member is obsolete
    }

    /// <inheritdoc/>
    public IVoiceEncryption? GetEncryption(Span<string> availableEncryptionModes)
    {
        foreach (var encryption in _encryptions)
        {
            if (availableEncryptionModes.Contains(encryption.ModeName))
            {
                return encryption.Factory();
            }
        }

        return null;
    }
}
