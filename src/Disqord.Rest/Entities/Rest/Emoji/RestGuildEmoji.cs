using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Disqord.Models;

namespace Disqord.Rest
{
    public sealed class RestGuildEmoji : RestSnowflakeEntity, IGuildEmoji
    {
        public string Name { get; private set; }

        public IReadOnlyList<Snowflake> RoleIds { get; private set; }

        public bool RequiresColons { get; }

        public bool IsManaged { get; }

        public bool IsAnimated { get; }

        public Snowflake GuildId { get; }

        public RestDownloadable<RestGuild> Guild { get; }

        public string ReactionFormat => Discord.ToReactionFormat(this);

        public string MessageFormat => Discord.ToMessageFormat(this);

        public string Tag => MessageFormat;

        internal RestGuildEmoji(RestDiscordClient client, Snowflake guildId, EmojiModel model) : base(client, model.Id.Value)
        {
            GuildId = guildId;
            Guild = new RestDownloadable<RestGuild>(options => Client.GetGuildAsync(GuildId, options));
            RequiresColons = model.RequireColons;
            IsManaged = model.Managed;
            IsAnimated = model.Animated;
            Update(model);
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

        public override string ToString()
            => MessageFormat;

        public Task DeleteAsync(RestRequestOptions options = null)
            => Client.DeleteGuildEmojiAsync(GuildId, Id, options);

        public async Task ModifyAsync(Action<ModifyGuildEmojiProperties> action, RestRequestOptions options = null)
        {
            var model = await Client.InternalModifyGuildEmojiAsync(GuildId, Id, action, options).ConfigureAwait(false);
            Update(model);
        }
    }
}