using Disqord.Models;

namespace Disqord.Gateway
{
    public class CachedRole : CachedSnowflakeEntity, IRole
    {
        public Snowflake GuildId { get; }

        public string Name { get; private set; }

        public Color? Color { get; private set; }

        public bool IsHoisted { get; private set; }

        public string IconHash { get; private set; }

        public int Position { get; private set; }

        public GuildPermissions Permissions { get; private set; }

        public bool IsManaged { get; private set; }

        public bool IsMentionable { get; private set; }

        public IRoleTags Tags { get; private set; }

        public string Mention => Disqord.Mention.Role(this);

        public CachedRole(IGatewayClient client, Snowflake guildId, RoleJsonModel model)
            : base(client, model.Id)
        {
            GuildId = guildId;

            Update(model);
        }

        public void Update(RoleJsonModel model)
        {
            Name = model.Name;
            Color = model.Color != 0
                ? model.Color
                : null;
            IsHoisted = model.Hoist;
            IconHash = model.Icon;
            Position = model.Position;
            Permissions = model.Permissions;
            IsManaged = model.Managed;
            IsMentionable = model.Mentionable;
            Tags = Optional.ConvertOrDefault(model.Tags, model => new TransientRoleTags(model), IRoleTags.Empty);
        }
    }
}
