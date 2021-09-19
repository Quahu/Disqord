﻿using Disqord.Models;

namespace Disqord
{
    public class TransientRole : TransientClientEntity<RoleJsonModel>, IRole
    {
        /// <inheritdoc/>
        public Snowflake Id => Model.Id;

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
        public string IconHash => Model.Icon;

        /// <inheritdoc/>
        public int Position => Model.Position;

        /// <inheritdoc/>
        public GuildPermissions Permissions => Model.Permissions;

        /// <inheritdoc/>
        public bool IsManaged => Model.Managed;

        /// <inheritdoc/>
        public bool IsMentionable => Model.Mentionable;

        /// <inheritdoc/>
        public IEmoji Emoji
        {
            get
            {
                if (Model.UnicodeEmoji is null)
                    return null;

                return _emoji ??= new TransientEmoji(Model.UnicodeEmoji);
            }
        }
        private IEmoji _emoji;

        /// <inheritdoc/>
        public IRoleTags Tags
        {
            get
            {
                if (!Model.Tags.HasValue)
                    return IRoleTags.Empty;

                return _tags ??= new TransientRoleTags(Model.Tags.Value);
            }
        }
        private IRoleTags _tags;

        public TransientRole(IClient client, Snowflake guildId, RoleJsonModel model)
            : base(client, model)
        {
            GuildId = guildId;
        }
    }
}
