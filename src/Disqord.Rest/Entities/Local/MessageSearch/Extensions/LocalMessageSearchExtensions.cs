using System.Collections.Generic;
using System.ComponentModel;
using Qommon;

namespace Disqord;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class LocalMessageSearchExtensions
{
    public static TSearch WithContents<TSearch>(this TSearch search, IEnumerable<string> contents)
        where TSearch : LocalMessageSearch
    {
        Guard.IsNotNull(contents);

        if (search.Contents.With(contents, out var list))
        {
            search.Contents = new(list);
        }

        return search;
    }

    public static TSearch WithContents<TSearch>(this TSearch search, params string[] contents)
        where TSearch : LocalMessageSearch
    {
        return search.WithContents(contents as IEnumerable<string>);
    }

    public static TSearch AddContent<TSearch>(this TSearch search, string content)
        where TSearch : LocalMessageSearch
    {
        if (search.Contents.Add(content, out var list))
        {
            search.Contents = new(list);
        }

        return search;
    }

    public static TSearch WithWordProximity<TSearch>(this TSearch search, int wordProximity)
        where TSearch : LocalMessageSearch
    {
        search.WordProximity = wordProximity;
        return search;
    }

    public static TSearch WithAuthorIds<TSearch>(this TSearch search, IEnumerable<Snowflake> authorIds)
        where TSearch : LocalMessageSearch
    {
        Guard.IsNotNull(authorIds);

        if (search.AuthorIds.With(authorIds, out var list))
        {
            search.AuthorIds = new(list);
        }

        return search;
    }

    public static TSearch WithAuthorIds<TSearch>(this TSearch search, params Snowflake[] authorIds)
        where TSearch : LocalMessageSearch
    {
        return search.WithAuthorIds(authorIds as IEnumerable<Snowflake>);
    }

    public static TSearch AddAuthorId<TSearch>(this TSearch search, Snowflake authorId)
        where TSearch : LocalMessageSearch
    {
        if (search.AuthorIds.Add(authorId, out var list))
        {
            search.AuthorIds = new(list);
        }

        return search;
    }

    public static TSearch WithIncludedAuthorTypes<TSearch>(this TSearch search, IEnumerable<MessageSearchAuthorType> authorTypes)
        where TSearch : LocalMessageSearch
    {
        Guard.IsNotNull(authorTypes);

        if (search.IncludedAuthorTypes.With(authorTypes, out var list))
        {
            search.IncludedAuthorTypes = new(list);
        }

        return search;
    }

    public static TSearch WithIncludedAuthorTypes<TSearch>(this TSearch search, params MessageSearchAuthorType[] authorTypes)
        where TSearch : LocalMessageSearch
    {
        return search.WithIncludedAuthorTypes(authorTypes as IEnumerable<MessageSearchAuthorType>);
    }

    public static TSearch AddIncludedAuthorType<TSearch>(this TSearch search, MessageSearchAuthorType authorType)
        where TSearch : LocalMessageSearch
    {
        if (search.IncludedAuthorTypes.Add(authorType, out var list))
        {
            search.IncludedAuthorTypes = new(list);
        }

        return search;
    }

    public static TSearch WithExcludedAuthorTypes<TSearch>(this TSearch search, IEnumerable<MessageSearchAuthorType> authorTypes)
        where TSearch : LocalMessageSearch
    {
        Guard.IsNotNull(authorTypes);

        if (search.ExcludedAuthorTypes.With(authorTypes, out var list))
        {
            search.ExcludedAuthorTypes = new(list);
        }

        return search;
    }

    public static TSearch WithExcludedAuthorTypes<TSearch>(this TSearch search, params MessageSearchAuthorType[] authorTypes)
        where TSearch : LocalMessageSearch
    {
        return search.WithExcludedAuthorTypes(authorTypes as IEnumerable<MessageSearchAuthorType>);
    }

    public static TSearch AddExcludedAuthorType<TSearch>(this TSearch search, MessageSearchAuthorType authorType)
        where TSearch : LocalMessageSearch
    {
        if (search.ExcludedAuthorTypes.Add(authorType, out var list))
        {
            search.ExcludedAuthorTypes = new(list);
        }

        return search;
    }

    public static TSearch WithMentionedUserIds<TSearch>(this TSearch search, IEnumerable<Snowflake> userIds)
        where TSearch : LocalMessageSearch
    {
        Guard.IsNotNull(userIds);

        if (search.MentionedUserIds.With(userIds, out var list))
        {
            search.MentionedUserIds = new(list);
        }

        return search;
    }

    public static TSearch WithMentionedUserIds<TSearch>(this TSearch search, params Snowflake[] userIds)
        where TSearch : LocalMessageSearch
    {
        return search.WithMentionedUserIds(userIds as IEnumerable<Snowflake>);
    }

    public static TSearch AddMentionedUserId<TSearch>(this TSearch search, Snowflake userId)
        where TSearch : LocalMessageSearch
    {
        if (search.MentionedUserIds.Add(userId, out var list))
        {
            search.MentionedUserIds = new(list);
        }

        return search;
    }

    public static TSearch WithMentionsEveryone<TSearch>(this TSearch search, bool mentionsEveryone = true)
        where TSearch : LocalMessageSearch
    {
        search.MentionsEveryone = mentionsEveryone;
        return search;
    }

    public static TSearch WithIncludedFilters<TSearch>(this TSearch search, IEnumerable<MessageSearchFilter> filters)
        where TSearch : LocalMessageSearch
    {
        Guard.IsNotNull(filters);

        if (search.IncludedFilters.With(filters, out var list))
        {
            search.IncludedFilters = new(list);
        }

        return search;
    }

    public static TSearch WithIncludedFilters<TSearch>(this TSearch search, params MessageSearchFilter[] filters)
        where TSearch : LocalMessageSearch
    {
        return search.WithIncludedFilters(filters as IEnumerable<MessageSearchFilter>);
    }

    public static TSearch AddIncludedFilter<TSearch>(this TSearch search, MessageSearchFilter filter)
        where TSearch : LocalMessageSearch
    {
        if (search.IncludedFilters.Add(filter, out var list))
        {
            search.IncludedFilters = new(list);
        }

        return search;
    }

    public static TSearch WithExcludedFilters<TSearch>(this TSearch search, IEnumerable<MessageSearchFilter> filters)
        where TSearch : LocalMessageSearch
    {
        Guard.IsNotNull(filters);

        if (search.ExcludedFilters.With(filters, out var list))
        {
            search.ExcludedFilters = new(list);
        }

        return search;
    }

    public static TSearch WithExcludedFilters<TSearch>(this TSearch search, params MessageSearchFilter[] filters)
        where TSearch : LocalMessageSearch
    {
        return search.WithExcludedFilters(filters as IEnumerable<MessageSearchFilter>);
    }

    public static TSearch AddExcludedFilter<TSearch>(this TSearch search, MessageSearchFilter filter)
        where TSearch : LocalMessageSearch
    {
        if (search.ExcludedFilters.Add(filter, out var list))
        {
            search.ExcludedFilters = new(list);
        }

        return search;
    }

    public static TSearch WithLinkHostNames<TSearch>(this TSearch search, IEnumerable<string> hostNames)
        where TSearch : LocalMessageSearch
    {
        Guard.IsNotNull(hostNames);

        if (search.LinkHostNames.With(hostNames, out var list))
        {
            search.LinkHostNames = new(list);
        }

        return search;
    }

    public static TSearch WithLinkHostNames<TSearch>(this TSearch search, params string[] hostNames)
        where TSearch : LocalMessageSearch
    {
        return search.WithLinkHostNames(hostNames as IEnumerable<string>);
    }

    public static TSearch AddLinkHostName<TSearch>(this TSearch search, string hostName)
        where TSearch : LocalMessageSearch
    {
        if (search.LinkHostNames.Add(hostName, out var list))
        {
            search.LinkHostNames = new(list);
        }

        return search;
    }

    public static TSearch WithEmbedProviders<TSearch>(this TSearch search, IEnumerable<string> providers)
        where TSearch : LocalMessageSearch
    {
        Guard.IsNotNull(providers);

        if (search.EmbedProviders.With(providers, out var list))
        {
            search.EmbedProviders = new(list);
        }

        return search;
    }

    public static TSearch WithEmbedProviders<TSearch>(this TSearch search, params string[] providers)
        where TSearch : LocalMessageSearch
    {
        return search.WithEmbedProviders(providers as IEnumerable<string>);
    }

    public static TSearch AddEmbedProvider<TSearch>(this TSearch search, string provider)
        where TSearch : LocalMessageSearch
    {
        if (search.EmbedProviders.Add(provider, out var list))
        {
            search.EmbedProviders = new(list);
        }

        return search;
    }

    public static TSearch WithEmbedTypes<TSearch>(this TSearch search, IEnumerable<MessageSearchEmbedType> embedTypes)
        where TSearch : LocalMessageSearch
    {
        Guard.IsNotNull(embedTypes);

        if (search.EmbedTypes.With(embedTypes, out var list))
        {
            search.EmbedTypes = new(list);
        }

        return search;
    }

    public static TSearch WithEmbedTypes<TSearch>(this TSearch search, params MessageSearchEmbedType[] embedTypes)
        where TSearch : LocalMessageSearch
    {
        return search.WithEmbedTypes(embedTypes as IEnumerable<MessageSearchEmbedType>);
    }

    public static TSearch AddEmbedType<TSearch>(this TSearch search, MessageSearchEmbedType embedType)
        where TSearch : LocalMessageSearch
    {
        if (search.EmbedTypes.Add(embedType, out var list))
        {
            search.EmbedTypes = new(list);
        }

        return search;
    }

    public static TSearch WithAttachmentExtensions<TSearch>(this TSearch search, IEnumerable<string> extensions)
        where TSearch : LocalMessageSearch
    {
        Guard.IsNotNull(extensions);

        if (search.AttachmentExtensions.With(extensions, out var list))
        {
            search.AttachmentExtensions = new(list);
        }

        return search;
    }

    public static TSearch WithAttachmentExtensions<TSearch>(this TSearch search, params string[] extensions)
        where TSearch : LocalMessageSearch
    {
        return search.WithAttachmentExtensions(extensions as IEnumerable<string>);
    }

    public static TSearch AddAttachmentExtension<TSearch>(this TSearch search, string extension)
        where TSearch : LocalMessageSearch
    {
        if (search.AttachmentExtensions.Add(extension, out var list))
        {
            search.AttachmentExtensions = new(list);
        }

        return search;
    }

    public static TSearch WithAttachmentFileNames<TSearch>(this TSearch search, IEnumerable<string> fileNames)
        where TSearch : LocalMessageSearch
    {
        Guard.IsNotNull(fileNames);

        if (search.AttachmentFileNames.With(fileNames, out var list))
        {
            search.AttachmentFileNames = new(list);
        }

        return search;
    }

    public static TSearch WithAttachmentFileNames<TSearch>(this TSearch search, params string[] fileNames)
        where TSearch : LocalMessageSearch
    {
        return search.WithAttachmentFileNames(fileNames as IEnumerable<string>);
    }

    public static TSearch AddAttachmentFileName<TSearch>(this TSearch search, string fileName)
        where TSearch : LocalMessageSearch
    {
        if (search.AttachmentFileNames.Add(fileName, out var list))
        {
            search.AttachmentFileNames = new(list);
        }

        return search;
    }

    public static TSearch WithIsPinned<TSearch>(this TSearch search, bool isPinned = true)
        where TSearch : LocalMessageSearch
    {
        search.IsPinned = isPinned;
        return search;
    }

    public static TSearch WithCommandId<TSearch>(this TSearch search, Snowflake commandId)
        where TSearch : LocalMessageSearch
    {
        search.CommandId = commandId;
        return search;
    }

    public static TSearch WithCommandName<TSearch>(this TSearch search, string commandName)
        where TSearch : LocalMessageSearch
    {
        search.CommandName = commandName;
        return search;
    }

    public static TSearch WithIncludesAgeRestrictedChannels<TSearch>(this TSearch search, bool includesAgeRestrictedChannels = true)
        where TSearch : LocalMessageSearch
    {
        search.IncludesAgeRestrictedChannels = includesAgeRestrictedChannels;
        return search;
    }

    public static TSearch WithChannelIds<TSearch>(this TSearch search, IEnumerable<Snowflake> channelIds)
        where TSearch : LocalMessageSearch
    {
        Guard.IsNotNull(channelIds);

        if (search.ChannelIds.With(channelIds, out var list))
        {
            search.ChannelIds = new(list);
        }

        return search;
    }

    public static TSearch WithChannelIds<TSearch>(this TSearch search, params Snowflake[] channelIds)
        where TSearch : LocalMessageSearch
    {
        return search.WithChannelIds(channelIds as IEnumerable<Snowflake>);
    }

    public static TSearch AddChannelId<TSearch>(this TSearch search, Snowflake channelId)
        where TSearch : LocalMessageSearch
    {
        if (search.ChannelIds.Add(channelId, out var list))
        {
            search.ChannelIds = new(list);
        }

        return search;
    }
}
