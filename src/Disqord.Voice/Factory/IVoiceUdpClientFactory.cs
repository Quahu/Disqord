namespace Disqord.Voice;

public interface IVoiceUdpClientFactory
{
    IVoiceUdpClient Create(uint ssrc, byte[] encryptionKey, string hostName, int port, IVoiceEncryption encryption);
}
