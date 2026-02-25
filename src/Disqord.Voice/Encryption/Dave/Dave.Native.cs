using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Disqord.Voice;

public static unsafe partial class Dave
{
    [LibraryImport(LibraryName, EntryPoint = "daveMaxSupportedProtocolVersion")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ushort MaxSupportedProtocolVersion();

    [LibraryImport(LibraryName, EntryPoint = "daveFree")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void Free(void* ptr);

    [LibraryImport(LibraryName, EntryPoint = "daveSessionCreate")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint SessionCreate(nint context, byte* authSessionId, MlsFailureCallback callback, nint userData);

    [LibraryImport(LibraryName, EntryPoint = "daveSessionDestroy")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SessionDestroy(nint session);

    [LibraryImport(LibraryName, EntryPoint = "daveSessionInit")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SessionInit(nint session, ushort version, ulong groupId, byte* selfUserId);

    [LibraryImport(LibraryName, EntryPoint = "daveSessionReset")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SessionReset(nint session);

    [LibraryImport(LibraryName, EntryPoint = "daveSessionSetProtocolVersion")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SessionSetProtocolVersion(nint session, ushort version);

    [LibraryImport(LibraryName, EntryPoint = "daveSessionGetProtocolVersion")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ushort SessionGetProtocolVersion(nint session);

    [LibraryImport(LibraryName, EntryPoint = "daveSessionGetLastEpochAuthenticator")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SessionGetLastEpochAuthenticator(nint session, out byte* authenticator, out nuint length);

    [LibraryImport(LibraryName, EntryPoint = "daveSessionSetExternalSender")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SessionSetExternalSender(nint session, byte* externalSender, nuint length);

    [LibraryImport(LibraryName, EntryPoint = "daveSessionProcessProposals")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SessionProcessProposals(nint session, byte* proposals, nuint length, byte** recognizedUserIds, nuint recognizedUserIdsLength, out byte* commitWelcomeBytes, out nuint commitWelcomeBytesLength);

    [LibraryImport(LibraryName, EntryPoint = "daveSessionProcessCommit")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint SessionProcessCommit(nint session, byte* commit, nuint length);

    [LibraryImport(LibraryName, EntryPoint = "daveSessionProcessWelcome")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint SessionProcessWelcome(nint session, byte* welcome, nuint length, byte** recognizedUserIds, nuint recognizedUserIdsLength);

    [LibraryImport(LibraryName, EntryPoint = "daveSessionGetMarshalledKeyPackage")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SessionGetMarshalledKeyPackage(nint session, out byte* keyPackage, out nuint length);

    [LibraryImport(LibraryName, EntryPoint = "daveSessionGetKeyRatchet")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint SessionGetKeyRatchet(nint session, byte* userId);

    [LibraryImport(LibraryName, EntryPoint = "daveSessionGetPairwiseFingerprint")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SessionGetPairwiseFingerprint(nint session, ushort version, byte* userId, PairwiseFingerprintCallback callback, nint userData);

    [LibraryImport(LibraryName, EntryPoint = "daveKeyRatchetDestroy")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void KeyRatchetDestroy(nint keyRatchet);

    [LibraryImport(LibraryName, EntryPoint = "daveCommitResultIsFailed")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static partial bool CommitResultIsFailed(nint commitResultHandle);

    [LibraryImport(LibraryName, EntryPoint = "daveCommitResultIsIgnored")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static partial bool CommitResultIsIgnored(nint commitResultHandle);

    [LibraryImport(LibraryName, EntryPoint = "daveCommitResultGetRosterMemberIds")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void CommitResultGetRosterMemberIds(nint commitResultHandle, out ulong* rosterIds, out nuint rosterIdsLength);

    [LibraryImport(LibraryName, EntryPoint = "daveCommitResultGetRosterMemberSignature")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void CommitResultGetRosterMemberSignature(nint commitResultHandle, ulong rosterId, out byte* signature, out nuint signatureLength);

    [LibraryImport(LibraryName, EntryPoint = "daveCommitResultDestroy")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void CommitResultDestroy(nint commitResultHandle);

    [LibraryImport(LibraryName, EntryPoint = "daveWelcomeResultGetRosterMemberIds")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void WelcomeResultGetRosterMemberIds(nint welcomeResultHandle, out ulong* rosterIds, out nuint rosterIdsLength);

    [LibraryImport(LibraryName, EntryPoint = "daveWelcomeResultGetRosterMemberSignature")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void WelcomeResultGetRosterMemberSignature(nint welcomeResultHandle, ulong rosterId, out byte* signature, out nuint signatureLength);

    [LibraryImport(LibraryName, EntryPoint = "daveWelcomeResultDestroy")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void WelcomeResultDestroy(nint welcomeResultHandle);

    [LibraryImport(LibraryName, EntryPoint = "daveEncryptorCreate")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint EncryptorCreate();

    [LibraryImport(LibraryName, EntryPoint = "daveEncryptorDestroy")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void EncryptorDestroy(nint encryptor);

    [LibraryImport(LibraryName, EntryPoint = "daveEncryptorSetKeyRatchet")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void EncryptorSetKeyRatchet(nint encryptor, nint keyRatchet);

    [LibraryImport(LibraryName, EntryPoint = "daveEncryptorSetPassthroughMode")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void EncryptorSetPassthroughMode(nint encryptor, [MarshalAs(UnmanagedType.U1)] bool passthroughMode);

    [LibraryImport(LibraryName, EntryPoint = "daveEncryptorAssignSsrcToCodec")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void EncryptorAssignSsrcToCodec(nint encryptor, uint ssrc, Codec codecType);

    [LibraryImport(LibraryName, EntryPoint = "daveEncryptorGetProtocolVersion")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ushort EncryptorGetProtocolVersion(nint encryptor);

    [LibraryImport(LibraryName, EntryPoint = "daveEncryptorGetMaxCiphertextByteSize")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nuint EncryptorGetMaxCiphertextByteSize(nint encryptor, MediaType mediaType, nuint frameSize);

    [LibraryImport(LibraryName, EntryPoint = "daveEncryptorHasKeyRatchet")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static partial bool EncryptorHasKeyRatchet(nint encryptor);

    [LibraryImport(LibraryName, EntryPoint = "daveEncryptorIsPassthroughMode")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static partial bool EncryptorIsPassthroughMode(nint encryptor);

    [LibraryImport(LibraryName, EntryPoint = "daveEncryptorEncrypt")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial EncryptorResultCode EncryptorEncrypt(nint encryptor, MediaType mediaType, uint ssrc, byte* frame, nuint frameLength, byte* encryptedFrame, nuint encryptedFrameCapacity, out nuint bytesWritten);

    [LibraryImport(LibraryName, EntryPoint = "daveEncryptorSetProtocolVersionChangedCallback")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void EncryptorSetProtocolVersionChangedCallback(nint encryptor, EncryptorProtocolVersionChangedCallback callback, nint userData);

    [LibraryImport(LibraryName, EntryPoint = "daveEncryptorGetStats")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void EncryptorGetStats(nint encryptor, MediaType mediaType, out EncryptorStats stats);

    [LibraryImport(LibraryName, EntryPoint = "daveDecryptorCreate")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint DecryptorCreate();

    [LibraryImport(LibraryName, EntryPoint = "daveDecryptorDestroy")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void DecryptorDestroy(nint decryptor);

    [LibraryImport(LibraryName, EntryPoint = "daveDecryptorTransitionToKeyRatchet")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void DecryptorTransitionToKeyRatchet(nint decryptor, nint keyRatchet);

    [LibraryImport(LibraryName, EntryPoint = "daveDecryptorTransitionToPassthroughMode")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void DecryptorTransitionToPassthroughMode(nint decryptor, [MarshalAs(UnmanagedType.U1)] bool passthroughMode);

    [LibraryImport(LibraryName, EntryPoint = "daveDecryptorDecrypt")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial DecryptorResultCode DecryptorDecrypt(nint decryptor, MediaType mediaType, byte* encryptedFrame, nuint encryptedFrameLength, byte* frame, nuint frameCapacity, out nuint bytesWritten);

    [LibraryImport(LibraryName, EntryPoint = "daveDecryptorGetMaxPlaintextByteSize")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nuint DecryptorGetMaxPlaintextByteSize(nint decryptor, MediaType mediaType, nuint encryptedFrameSize);

    [LibraryImport(LibraryName, EntryPoint = "daveDecryptorGetStats")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void DecryptorGetStats(nint decryptor, MediaType mediaType, out DecryptorStats stats);

    [LibraryImport(LibraryName, EntryPoint = "daveSetLogSinkCallback")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SetLogSinkCallback(LogSinkCallback callback);
}
