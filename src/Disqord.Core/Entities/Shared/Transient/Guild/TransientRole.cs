using System;
using Disqord.Models;

namespace Disqord
{
    public class TransientRole : TransientEntity<RoleJsonModel>, IRole
    {
        /// <inheritdoc/>
        public Snowflake Id => Model.Id;

        /// <inheritdoc/>
        public DateTimeOffset CreatedAt => Id.CreatedAt;

        /// <inheritdoc/>
        public Snowflake GuildId { get; }

        /// <inheritdoc/>
        public string Name => Model.Name;

        /// <inheritdoc/>
        public string Mention => Disqord.Mention.Role(this);

        /// <inheritdoc/>
        public Color? Color => Model.Color != 0
            ? Model.Color
            : null;

        /// <inheritdoc/>
        public bool IsHoisted => Model.Hoist;

        /// <inheritdoc/>
        public int Position => Model.Position;

        /// <inheritdoc/>
        public GuildPermissions Permissions => Model.Permissions;

        /// <inheritdoc/>
        public bool IsManaged => Model.Managed;

        /// <inheritdoc/>
        public bool IsMentionable => Model.Mentionable;

        /// <inheritdoc/>
        public RoleTags Tags => _tags ??= Optional.ConvertOrDefault(Model.Tags, x => new RoleTags(x), RoleTags.Empty);
        private RoleTags _tags;

        public TransientRole(IClient client, Snowflake guildId, RoleJsonModel model)
            : base(client, model)
        {
            GuildId = guildId;
        }
    }
}
