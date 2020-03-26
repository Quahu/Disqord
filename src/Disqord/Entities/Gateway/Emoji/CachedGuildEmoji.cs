using System.Collections.Generic;
using System.Linq;
using Disqord.Collections;
using Disqord.Models;

namespace Disqord
{
    public sealed partial class CachedGuildEmoji : CachedSnowflakeEntity, IGuildEmoji
    {
        public CachedGuild Guild { get; }

        public string Name { get; private set; }

        public IReadOnlyList<Snowflake> RoleIds { get; private set; }

        public bool RequiresColons { get; }

        public bool IsManaged { get; }

        public bool IsAnimated { get; }

        public bool IsAvailable { get; private set; }

        public string ReactionFormat => Discord.ToReactionFormat(this);

        public string MessageFormat => Discord.ToMessageFormat(this);

        public string Tag => MessageFormat;

        Snowflake IGuildEmoji.GuildId => Guild.Id;

        internal CachedGuildEmoji(CachedGuild guild, EmojiModel model) : base(guild.Client, model.Id.Value)
        {
            Guild = guild;
            RequiresColons = model.RequireColons;
            IsManaged = model.Managed;
            IsAnimated = model.Animated;

            Update(model);
        }

        internal void Update(EmojiModel model)
        {
            Name = model.Name;
            RoleIds = model.Roles.ToSnowflakeList();
            IsAvailable = model.Available;
        }

        public string GetUrl(int size = 2048)
            => Discord.Cdn.GetCustomEmojiUrl(Id, IsAnimated, size);

        public bool Equals(IEmoji other)
            => Discord.Comparers.Emoji.Equals(this, other);

        public override bool Equals(object obj)
            => obj is IEmoji emoji && Equals(emoji);

        public override int GetHashCode()
            => Discord.Comparers.Emoji.GetHashCode(this);
    }
}