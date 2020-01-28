using System.Collections.Generic;
using System.Linq;
using Disqord.Collections;
using Disqord.Models;

namespace Disqord
{
    public abstract partial class CachedGuildChannel : CachedChannel, IGuildChannel
    {
        public CachedGuild Guild { get; }

        public int Position { get; private set; }

        public IReadOnlyList<CachedOverwrite> Overwrites { get; private set; }

        Snowflake IGuildChannel.GuildId => Guild.Id;
        IReadOnlyList<IOverwrite> IGuildChannel.Overwrites => Overwrites;

        internal CachedGuildChannel(CachedGuild guild, ChannelModel model) : base(guild.Client, model)
        {
            Guild = guild;
        }

        internal override void Update(ChannelModel model)
        {
            if (model.Position.HasValue)
                Position = model.Position.Value;

            if (model.PermissionOverwrites.HasValue)
                Overwrites = model.PermissionOverwrites.Value.ToReadOnlyList(
                    this, (x, @this) => new CachedOverwrite(@this, x));

            base.Update(model);
        }

        internal static CachedGuildChannel Create(CachedGuild guild, ChannelModel model)
        {
            switch (model.Type.Value)
            {
                case ChannelType.Text:
                case ChannelType.News:
                case ChannelType.Store:
                    return new CachedTextChannel(guild, model);

                case ChannelType.Voice:
                    return new CachedVoiceChannel(guild, model);

                case ChannelType.Category:
                    return new CachedCategoryChannel(guild, model);

                default:
                    return new CachedUnknownGuildChannel(guild, model);
            }
        }
    }
}