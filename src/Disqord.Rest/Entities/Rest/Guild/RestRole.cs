using Disqord.Models;

namespace Disqord.Rest
{
    public sealed partial class RestRole : RestSnowflakeEntity, IRole
    {
        public Snowflake GuildId { get; }

        public RestFetchable<RestGuild> Guild { get; }

        public string Name { get; private set; }

        public Color? Color { get; private set; }

        public bool IsHoisted { get; private set; }

        public int Position { get; private set; }

        public GuildPermissions Permissions { get; private set; }

        public bool IsManaged { get; private set; }

        public bool IsMentionable { get; private set; }

        public string Mention => Discord.MentionRole(this);

        public bool IsDefault => Id == GuildId;

        internal RestRole(RestDiscordClient client, Snowflake guildId, RoleModel model) : base(client, model.Id)
        {
            GuildId = guildId;
            Guild = RestFetchable.Create(this, (@this, options) =>
                @this.Client.GetGuildAsync(@this.GuildId, options));
            Update(model);
        }

        internal void Update(RoleModel model)
        {
            Name = model.Name;
            Color = model.Color != 0
                ? (int?) model.Color
                : null;
            IsHoisted = model.Hoist;
            Position = model.Position;
            Permissions = model.Permissions;
            IsManaged = model.Managed;
            IsMentionable = model.Mentionable;
        }

        public override string ToString()
            => Name;
    }
}