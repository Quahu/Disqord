using System.Runtime.InteropServices;

namespace Disqord.Voice;

public static unsafe partial class Sodium
{
    private const string LibraryName = "libsodium";

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int crypto_aead_aes256gcm_keybytes();

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int crypto_aead_aes256gcm_nsecbytes();

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int crypto_aead_aes256gcm_npubbytes();

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int crypto_aead_aes256gcm_abytes();

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool crypto_aead_aes256gcm_is_available();

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int crypto_aead_aes256gcm_encrypt(byte* c, ulong* clen_p, byte* m, ulong mlen, byte* ad, ulong adlen, byte* nsec, byte* npub, byte* k);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int crypto_aead_aes256gcm_decrypt(byte* m, ulong* mlen_p, byte* nsec, byte* c, ulong clen, byte* ad, ulong adlen, byte* npub, byte* k);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int crypto_aead_aes256gcm_encrypt_detached(byte* c, byte* mac, ulong* maclen_p, byte* m, ulong mlen, byte* ad, ulong adlen, byte* nsec, byte* npub, byte* k);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int crypto_aead_aes256gcm_decrypt_detached(byte* m, byte* nsec, byte* c, ulong clen, byte* mac, byte* ad, ulong adlen, byte* npub, byte* k);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int crypto_aead_xchacha20poly1305_ietf_keybytes();

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int crypto_aead_xchacha20poly1305_ietf_nsecbytes();

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int crypto_aead_xchacha20poly1305_ietf_npubbytes();

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int crypto_aead_xchacha20poly1305_ietf_abytes();

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int crypto_aead_xchacha20poly1305_ietf_encrypt(byte* c, ulong* clen_p, byte* m, ulong mlen, byte* ad, ulong adlen, byte* nsec, byte* npub, byte* k);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int crypto_aead_xchacha20poly1305_ietf_decrypt(byte* m, ulong* mlen_p, byte* nsec, byte* c, ulong clen, byte* ad, ulong adlen, byte* npub, byte* k);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int crypto_aead_xchacha20poly1305_ietf_encrypt_detached(byte* c, byte* mac, ulong* maclen_p, byte* m, ulong mlen, byte* ad, ulong adlen, byte* nsec, byte* npub, byte* k);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int crypto_aead_xchacha20poly1305_ietf_decrypt_detached(byte* m, byte* nsec, byte* c, ulong clen, byte* mac, byte* ad, ulong adlen, byte* npub, byte* k);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int crypto_secretbox_xsalsa20poly1305_keybytes();

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int crypto_secretbox_xsalsa20poly1305_noncebytes();

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int crypto_secretbox_xsalsa20poly1305_macbytes();

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int crypto_secretbox_easy(byte* c, byte* m, ulong mlen, byte* n, byte* k);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int crypto_secretbox_open_easy(byte* m, byte* c, ulong clen, byte* n, byte* k);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
    private static extern void randombytes_buf(byte* buf, nuint size);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int sodium_init();
}
