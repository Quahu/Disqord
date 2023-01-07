using Microsoft.Extensions.Logging;

namespace Disqord.Voice.Api;

public interface IVoiceGatewayClientFactory
{
    IVoiceGatewayClient Create(Snowflake guildId, Snowflake currentMemberId, string sessionId, string token, string endpoint, ILogger logger);
}
