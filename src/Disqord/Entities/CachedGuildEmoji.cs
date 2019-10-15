using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Disqord.Models;

namespace Disqord
{
    public sealed class CachedGuildEmoji : CachedSnowflakeEntity, IGuildEmoji
    {
        public string Name { get; private set; }

        public IReadOnlyList<Snowflake> RoleIds { get; private set; }

        public bool RequiresColons { get; }

        public bool IsManaged { get; }

        public bool IsAnimated { get; }

        public CachedGuild Guild { get; }

        public string ReactionFormat => Discord.ToReactionFormat(this);

        public string MessageFormat => Discord.ToMessageFormat(this);

        public string Tag => MessageFormat;

        Snowflake IGuildEmoji.GuildId => Guild.Id;

        internal CachedGuildEmoji(DiscordClient client, EmojiModel model, CachedGuild guild) : base(client, model.Id.Value)
        {
            Guild = guild;
            RequiresColons = model.RequireColons;
            IsManaged = model.Managed;
            IsAnimated = model.Animated;
        }

        internal void Update(EmojiModel model)
        {
            Name = model.Name;
            RoleIds = model.Roles.Select(x => new Snowflake(x)).ToImmutableArray();
        }

        public string GetUrl(int size = 2048)
            => Discord.GetCustomEmojiUrl(Id, IsAnimated, size);

        public bool Equals(IEmoji other)
        {
            if (other == null)
                return false;

            if (!(other is ICustomEmoji customEmoji))
                return false;

            return Id.Equals(customEmoji.Id);
        }

        public Task DeleteAsync(RestRequestOptions options = null)
            => Client.RestClient.DeleteGuildEmojiAsync(Guild.Id, Id, options);

        public async Task ModifyAsync(Action<ModifyGuildEmojiProperties> action, RestRequestOptions options = null)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            var properties = new ModifyGuildEmojiProperties();
            action(properties);
            Update(await Client.RestClient.ApiClient.ModifyGuildEmojiAsync(Guild.Id, Id, properties, options).ConfigureAwait(false));
        }
    }
}