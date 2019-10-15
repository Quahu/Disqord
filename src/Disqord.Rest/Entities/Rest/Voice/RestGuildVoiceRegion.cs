using Disqord.Models;

namespace Disqord.Rest
{
    public class RestGuildVoiceRegion : RestVoiceRegion
    {
        public Snowflake GuildId { get; }

        public RestDownloadable<RestGuild> Guild { get; }

        internal RestGuildVoiceRegion(RestDiscordClient client, VoiceRegionModel model, Snowflake guildId) : base(client, model)
        {
            GuildId = guildId;
            Guild = new RestDownloadable<RestGuild>(options => Client.GetGuildAsync(GuildId, options));
        }
    }
}
