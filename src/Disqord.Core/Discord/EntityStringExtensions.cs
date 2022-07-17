namespace Disqord;

internal static class EntityStringExtensions
{
    public static string GetString(this IEntity entity)
    {
        var name = (entity as IPossiblyNamableEntity)?.Name;
        var identifiable = entity as IIdentifiableEntity;
        if (name != null && identifiable != null)
            return $"'{name}' (ID: {identifiable.Id})";

        if (name != null)
            return $"'{name}'";

        if (identifiable != null)
            return $"ID: {identifiable.Id}";

        return entity.GetType().ToString();
    }

    public static string GetString(this IUser user)
    {
        return user.Tag;
    }

    public static string GetString(this IPartialSticker sticker)
    {
        return $"'{sticker.Name}' ({sticker.Id}, {sticker.FormatType})";
    }

    public static string GetString(this IMessageReference reference)
    {
        return $"{reference.GuildId?.ToString() ?? "<no guild ID>"}/{reference.ChannelId}/{reference.MessageId?.ToString() ?? "<no message ID>"}";
    }

    public static string GetString(this IMessageReaction reaction)
    {
        return $"'{reaction.Emoji}': {reaction.Count} (own reaction: {reaction.HasOwnReaction})";
    }

    public static string GetString(this IBan ban)
    {
        return $"{ban.User.Tag}: {ban.Reason}";
    }

    public static string GetString(this IVoiceRegion region)
    {
        return $"'{region.Name}' ({region.Id})";
    }

    public static string GetString(this IEmoji emoji)
    {
        if (emoji is ICustomEmoji customEmoji)
        {
            if (customEmoji.IsAnimated)
                return $"<a:{customEmoji.Name ?? "_"}:{customEmoji.Id}>";

            return $"<:{customEmoji.Name ?? "_"}:{customEmoji.Id}>";
        }

        return emoji.Name ?? emoji.GetType().ToString();
    }
}
