using System;
using Disqord.Models;

namespace Disqord
{
    public class TransientRole : TransientEntity<RoleJsonModel>, IRole
    {
        public Snowflake Id => Model.Id;

        public DateTimeOffset CreatedAt => Id.CreatedAt;

        public Snowflake GuildId { get; }

        public string Name => Model.Name;

        public string Mention => Disqord.Mention.Role(this);

        public Color? Color => Model.Color != 0
            ? Model.Color
            : null;

        public bool IsHoisted => Model.Hoist;

        public int Position => Model.Position;

        public GuildPermissions Permissions => Model.Permissions;

        public bool IsManaged => Model.Managed;

        public bool IsMentionable => Model.Mentionable;

        public RoleTags Tags => _tags ??= Optional.ConvertOrDefault(Model.Tags, x => new RoleTags(x), RoleTags.Empty);
        private RoleTags _tags;

        public TransientRole(IClient client, Snowflake guildId, RoleJsonModel model)
            : base(client, model)
        {
            GuildId = guildId;
        }
    }
}
