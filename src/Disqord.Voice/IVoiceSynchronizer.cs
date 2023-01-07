using Disqord.Logging;

namespace Disqord.Voice;

/// <summary>
///     Represents a type responsible for handling delaying of voice packets for subscribed clients.
/// </summary>
public interface IVoiceSynchronizer : ILogging
{
    void Subscribe(IVoiceUdpClient client);

    void Unsubscribe(IVoiceUdpClient client);
}
