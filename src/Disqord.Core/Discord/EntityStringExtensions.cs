namespace Disqord
{
    internal static class EntityStringExtensions
    {
        public static string GetString(this IEntity entity)
        {
            var namable = entity as INamableEntity;
            var identifiable = entity as IIdentifiableEntity;
            if (namable != null && identifiable != null)
                return $"'{namable.Name}' (ID: {identifiable.Id})";

            if (namable != null)
                return $"'{namable.Name}'";

            if (identifiable != null)
                return $"ID: {identifiable.Id}";

            return entity.GetType().ToString();
        }

        public static string GetString(this IUser user)
            => user.Tag;

        public static string GetString(this IPartialSticker sticker)
            => $"'{sticker.Name}' ({sticker.Id}, {sticker.FormatType})";

        public static string GetString(this IMessageReference reference)
            => $"{reference.GuildId?.ToString() ?? "<no guild ID>"}/{reference.ChannelId}/{reference.MessageId?.ToString() ?? "<no message ID>"}";

        public static string GetString(this IMessageReaction reaction)
            => $"'{reaction.Emoji}': {reaction.Count} (own reaction: {reaction.HasOwnReaction})";

        public static string GetString(this IBan ban)
            => $"{ban.User.Tag}: {ban.Reason}";

        public static string GetString(this IVoiceRegion region)
            => $"'{region.Name}' ({region.Id})";

        public static string GetString(this IEmoji emoji)
        {
            if (emoji is ICustomEmoji customEmoji)
            {
                if (customEmoji.IsAnimated)
                    return $"<a:{customEmoji.Name ?? "_"}:{customEmoji.Id}>";

                return $"<:{customEmoji.Name ?? "_"}:{customEmoji.Id}>";
            }

            return emoji.Name;
        }
    }
}
