using System;
using System.Runtime.InteropServices;

namespace Disqord.Voice;

/// <summary>
///     Represents Sodium interoperability.
/// </summary>
/// <seealso href="https://doc.libsodium.org/"> Sodium documentation </seealso>
public static unsafe partial class Sodium
{
    /// <summary>
    ///     The amount of key bytes.
    /// </summary>
    public static readonly int XSalsa20Poly1305KeyLength;

    /// <summary>
    ///     The amount of nonce bytes.
    /// </summary>
    public static readonly int XSalsa20Poly1305NonceLength;

    /// <summary>
    ///     The amount of mac bytes.
    /// </summary>
    public static readonly int XSalsa20Poly1305MacLength;

    /// <summary>
    ///     Encrypts the specified span.
    /// </summary>
    /// <param name="target"> The span to encrypt the data into. </param>
    /// <param name="source"> The span to encrypt. </param>
    /// <param name="nonce"> The nonce. </param>
    /// <param name="key"> The encryption key. </param>
    /// <returns>
    ///     The result.
    /// </returns>
    public static void Encrypt(Span<byte> target, ReadOnlySpan<byte> source, ReadOnlySpan<byte> nonce, ReadOnlySpan<byte> key)
    {
        fixed (byte* sourcePtr = source)
        fixed (byte* targetPtr = target)
        fixed (byte* keyPtr = key)
        fixed (byte* noncePtr = nonce)
        {
            var result = crypto_secretbox_easy(targetPtr, sourcePtr, (ulong) source.Length, noncePtr, keyPtr);
            CheckEncryptionResult(result);
        }
    }

    public static void CheckEncryptionResult(int result)
    {
        if (result != 0)
        {
            throw new ExternalException($"Sodium failed to encrypt the data ({result}).", result);
        }
    }

    /// <summary>
    ///     Decrypts the specified span.
    /// </summary>
    /// <param name="target"> The span to decrypt the data into. </param>
    /// <param name="source"> The span to decrypt. </param>
    /// <param name="nonce"> The nonce. </param>
    /// <param name="key"> The encryption key. </param>
    /// <returns>
    ///     The result.
    /// </returns>
    public static void Decrypt(Span<byte> target, ReadOnlySpan<byte> source, ReadOnlySpan<byte> nonce, ReadOnlySpan<byte> key)
    {
        fixed (byte* sourcePtr = source)
        fixed (byte* targetPtr = target)
        fixed (byte* keyPtr = key)
        fixed (byte* noncePtr = nonce)
        {
            var result = crypto_secretbox_open_easy(targetPtr, sourcePtr, (ulong) source.Length, noncePtr, keyPtr);
            if (result != 0)
                throw new ExternalException($"Sodium failed to decrypt the data  ({result}).", result);
        }
    }

    /// <summary>
    ///     Fills the specified span with random bytes.
    /// </summary>
    /// <param name="bytes"> The span to fill with random bytes. </param>
    public static void GetRandomBytes(Span<byte> bytes)
    {
        fixed (byte* bytesPtr = bytes)
        {
            randombytes_buf(bytesPtr, (nuint) bytes.Length);
        }
    }

    static Sodium()
    {
        _ = sodium_init();

        XSalsa20Poly1305KeyLength = crypto_secretbox_xsalsa20poly1305_keybytes();
        XSalsa20Poly1305NonceLength = crypto_secretbox_xsalsa20poly1305_noncebytes();
        XSalsa20Poly1305MacLength = crypto_secretbox_xsalsa20poly1305_macbytes();
    }
}
