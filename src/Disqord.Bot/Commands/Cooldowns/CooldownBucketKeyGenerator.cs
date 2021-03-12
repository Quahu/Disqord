using System;
using Qmmands.Delegates;

namespace Disqord.Bot
{
    public static class CooldownBucketKeyGenerator
    {
        public static readonly CooldownBucketKeyGeneratorDelegate Instance = (_, __) =>
        {
            if (_ is not CooldownBucketType bucketType)
                throw new ArgumentException($"Bucket type must be a {typeof(CooldownBucketType)}.", nameof(bucketType));

            if (__ is not DiscordCommandContext context)
                throw new ArgumentException($"Context must be a {typeof(DiscordCommandContext)}.", nameof(context));

            return bucketType switch
            {
                CooldownBucketType.User => context.Author.Id,
                CooldownBucketType.Member => (context.GuildId, context.Author.Id),
                CooldownBucketType.Guild => context.GuildId ?? context.Author.Id,
                CooldownBucketType.Channel => context.ChannelId,
                _ => throw new ArgumentOutOfRangeException(nameof(bucketType))
            };
        };
    }
}
