using Disqord.Models;

namespace Disqord.Rest
{
    public sealed class RestGuildVoiceRegion : RestVoiceRegion
    {
        public Snowflake GuildId { get; }

        public RestDownloadable<RestGuild> Guild { get; }

        internal RestGuildVoiceRegion(RestDiscordClient client, Snowflake guildId, VoiceRegionModel model) : base(client, model)
        {
            GuildId = guildId;
            Guild = new RestDownloadable<RestGuild>(options => Client.GetGuildAsync(GuildId, options));
        }
    }
}
