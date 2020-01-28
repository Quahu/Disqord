using Disqord.Models;

namespace Disqord.Rest
{
    public sealed class RestGuildVoiceRegion : RestVoiceRegion
    {
        public Snowflake GuildId { get; }

        public RestFetchable<RestGuild> Guild { get; }

        internal RestGuildVoiceRegion(RestDiscordClient client, Snowflake guildId, VoiceRegionModel model) : base(client, model)
        {
            GuildId = guildId;
            Guild = RestFetchable.Create(this, (@this, options) =>
                @this.Client.GetGuildAsync(@this.GuildId, options));
        }
    }
}
