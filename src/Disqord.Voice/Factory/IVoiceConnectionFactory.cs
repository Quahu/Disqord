namespace Disqord.Voice;

public interface IVoiceConnectionFactory
{
    IVoiceConnection Create(Snowflake guildId, Snowflake channelId, Snowflake currentMemberId, SetVoiceStateDelegate setVoiceStateDelegate);
}
