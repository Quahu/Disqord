using System.Threading.Tasks;
using Disqord.Models;

namespace Disqord.Rest
{
    /// <summary>
    ///     Represents a guild ban.
    /// </summary>
    public sealed class RestBan : RestDiscordEntity, IDeletable
    {
        /// <summary>
        ///     Gets the guild's id.
        /// </summary>
        public Snowflake GuildId { get; }

        /// <summary>
        ///     Gets the guild.
        /// </summary>
        public RestFetchable<RestGuild> Guild { get; }

        /// <summary>
        ///     Gets the banned user.
        /// </summary>
        public RestUser User { get; }

        /// <summary>
        ///     Gets the reason.
        /// </summary>
        public string Reason { get; }

        internal RestBan(RestDiscordClient client, Snowflake guildId, BanModel model) : base(client)
        {
            GuildId = guildId;
            Guild = RestFetchable.Create(this, (@this, options) =>
                @this.Client.GetGuildAsync(@this.GuildId, options));
            User = new RestUser(client, model.User);
            Reason = model.Reason;
        }

        /// <summary>
        ///     Revokes this <see cref="RestBan"/>, unbanning the <see cref="User"/>.
        /// </summary>
        /// <param name="options"></param>
        public Task RevokeAsync(RestRequestOptions options = null)
            => Client.UnbanMemberAsync(GuildId, User.Id, options);

        Task IDeletable.DeleteAsync(RestRequestOptions options)
            => RevokeAsync(options);
    }
}
