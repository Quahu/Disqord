using System.Runtime.InteropServices;

namespace Disqord.Voice;

public static unsafe partial class Sodium
{
    private const string LibraryName = "libsodium";

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
}
