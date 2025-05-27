using System.ComponentModel;

namespace Disqord;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class LocalForumTagExtensions
{
    public static TForumTag WithId<TForumTag>(this TForumTag forumTag, Snowflake id)
        where TForumTag : LocalForumTag
    {
        forumTag.Id = id;
        return forumTag;
    }

    public static TForumTag WithName<TForumTag>(this TForumTag forumTag, string name)
        where TForumTag : LocalForumTag
    {
        forumTag.Name = name;
        return forumTag;
    }

    public static TForumTag WithIsModerated<TForumTag>(this TForumTag forumTag, bool isModerated = true)
        where TForumTag : LocalForumTag
    {
        forumTag.IsModerated = isModerated;
        return forumTag;
    }

    public static TForumTag WithEmoji<TForumTag>(this TForumTag forumTag, LocalEmoji emoji)
        where TForumTag : LocalForumTag
    {
        forumTag.Emoji = emoji;
        return forumTag;
    }
}
