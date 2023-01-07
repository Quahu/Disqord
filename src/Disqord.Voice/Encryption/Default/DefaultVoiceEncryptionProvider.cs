using System;
using static Disqord.Voice.Api.KnownEncryptionModes;

namespace Disqord.Voice.Default;

/// <inheritdoc/>
public class DefaultVoiceEncryptionProvider : IVoiceEncryptionProvider
{
    private XSalsa20Poly1305Encryption? _xSalsa20Poly1305;
    private XSalsa20Poly1305SuffixEncryption? _xSalsa20Poly1305Suffix;

    /// <inheritdoc/>
    public IVoiceEncryption? GetEncryption(Span<string> availableEncryptionModes)
    {
        var hasXSalsa20Poly1305 = false;
        var hasXSalsa20Poly1305Lite = false;
        var hasXSalsa20Poly1305Suffix = false;
        foreach (var encryptionMode in availableEncryptionModes)
        {
            if (!hasXSalsa20Poly1305 && encryptionMode == XSalsa20Poly1305)
            {
                hasXSalsa20Poly1305 = true;
                break;
            }

            if (!hasXSalsa20Poly1305Lite && encryptionMode == XSalsa20Poly1305Lite)
            {
                hasXSalsa20Poly1305Lite = true;
                continue;
            }

            if (!hasXSalsa20Poly1305Suffix && encryptionMode == XSalsa20Poly1305Suffix)
            {
                hasXSalsa20Poly1305Suffix = true;
                continue;
            }
        }

        if (hasXSalsa20Poly1305)
        {
            // Cheapest - no suffix.
            return _xSalsa20Poly1305 ??= new();
        }

        if (hasXSalsa20Poly1305Lite)
        {
            // 2nd cheapest - 4 byte suffix.
            return new XSalsa20Poly1305LiteEncryption();
        }

        if (hasXSalsa20Poly1305Suffix)
        {
            return _xSalsa20Poly1305Suffix ??= new();
        }

        return null;
    }
}
