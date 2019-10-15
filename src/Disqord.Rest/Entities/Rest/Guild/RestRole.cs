using System;
using System.Threading.Tasks;
using Disqord.Models;

namespace Disqord.Rest
{
    public sealed class RestRole : RestSnowflakeEntity, IRole
    {
        public string Name { get; private set; }

        public Color Color { get; private set; }

        public bool IsHoisted { get; private set; }

        public int Position { get; private set; }

        public GuildPermissions Permissions { get; private set; }

        public bool IsManaged { get; private set; }

        public bool IsMentionable { get; private set; }

        public string Mention => Discord.MentionRole(this);

        public Snowflake GuildId { get; }

        public RestDownloadable<RestGuild> Guild { get; }

        internal RestRole(RestDiscordClient client, RoleModel model, ulong guildId) : base(client, model.Id)
        {
            GuildId = guildId;
            Guild = new RestDownloadable<RestGuild>(options => Client.GetGuildAsync(GuildId, options));
            Update(model);
        }

        internal void Update(RoleModel model)
        {
            Name = model.Name;
            Color = model.Color;
            IsHoisted = model.Hoist;
            Position = model.Position;
            Permissions = model.Permissions;
            IsManaged = model.Managed;
            IsMentionable = model.Mentionable;
        }

        public async Task ModifyAsync(Action<ModifyRoleProperties> action, RestRequestOptions options = null)
        {
            var model = await Client.InternalModifyRoleAsync(GuildId, Id, action, options).ConfigureAwait(false);
            Update(model);
        }

        public Task DeleteAsync(RestRequestOptions options = null)
            => Client.DeleteRoleAsync(GuildId, Id, options);

        public override string ToString()
            => Name;
    }
}