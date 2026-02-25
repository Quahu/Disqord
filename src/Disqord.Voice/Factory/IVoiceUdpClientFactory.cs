using Microsoft.Extensions.Logging;

namespace Disqord.Voice;

public interface IVoiceUdpClientFactory
{
    IVoiceUdpClient Create(uint ssrc, string hostName, int port, ILogger logger, IVoiceEncryption encryption);
}
