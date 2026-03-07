namespace Disqord.Voice;

public interface IVoiceConnectionFactory
{
    IVoiceConnectionHost Create(Snowflake guildId, Snowflake channelId, Snowflake currentMemberId, SetVoiceStateDelegate setVoiceStateDelegate);
}
