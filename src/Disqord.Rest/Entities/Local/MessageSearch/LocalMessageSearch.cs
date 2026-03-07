using System.Collections.Generic;
using Qommon;

namespace Disqord;

/// <summary>
///     Represents a local construct for guild message search criteria.
/// </summary>
public class LocalMessageSearch : ILocalConstruct<LocalMessageSearch>
{
    /// <summary>
    ///     Gets or sets the search terms.
    /// </summary>
    /// <remarks>
    ///     Multiple values are treated as OR (matches any of them).
    /// </remarks>
    public Optional<IList<string>> Contents { get; set; }

    /// <summary>
    ///     Gets or sets the word proximity for content search.
    /// </summary>
    /// <remarks>
    ///     Specifies the maximum number of words that can appear between the search terms.
    ///     Must be between 0 and 100.
    /// </remarks>
    public Optional<int> WordProximity { get; set; }

    /// <summary>
    ///     Gets or sets the author IDs to filter by.
    /// </summary>
    /// <remarks>
    ///     Multiple values are treated as OR (matches any of them).
    /// </remarks>
    public Optional<IList<Snowflake>> AuthorIds { get; set; }

    /// <summary>
    ///     Gets or sets the author types to include in results.
    /// </summary>
    /// <remarks>
    ///     Multiple values are treated as OR (matches any of them).
    /// </remarks>
    public Optional<IList<MessageSearchAuthorType>> IncludedAuthorTypes { get; set; }

    /// <summary>
    ///     Gets or sets the author types to exclude from results.
    /// </summary>
    /// <remarks>
    ///     Multiple values are treated as AND (excludes all of them).
    /// </remarks>
    public Optional<IList<MessageSearchAuthorType>> ExcludedAuthorTypes { get; set; }

    /// <summary>
    ///     Gets or sets the user IDs that messages must mention.
    /// </summary>
    /// <remarks>
    ///     Multiple values are treated as OR (matches any of them).
    /// </remarks>
    public Optional<IList<Snowflake>> MentionedUserIds { get; set; }

    /// <summary>
    ///     Gets or sets whether to filter by messages that mention everyone.
    /// </summary>
    public Optional<bool> MentionsEveryone { get; set; }

    /// <summary>
    ///     Gets or sets the content type filters to include in results.
    /// </summary>
    /// <remarks>
    ///     Multiple values are treated as OR (matches any of them).
    /// </remarks>
    public Optional<IList<MessageSearchFilter>> IncludedFilters { get; set; }

    /// <summary>
    ///     Gets or sets the content type filters to exclude from results.
    /// </summary>
    public Optional<IList<MessageSearchFilter>> ExcludedFilters { get; set; }

    /// <summary>
    ///     Gets or sets the link hostnames to filter by.
    /// </summary>
    /// <remarks>
    ///     Multiple values are treated as OR (matches any of them).
    /// </remarks>
    public Optional<IList<string>> LinkHostNames { get; set; }

    /// <summary>
    ///     Gets or sets the embed providers to filter by.
    /// </summary>
    /// <remarks>
    ///     Multiple values are treated as OR (matches any of them).
    /// </remarks>
    public Optional<IList<string>> EmbedProviders { get; set; }

    /// <summary>
    ///     Gets or sets the embed types to filter by.
    /// </summary>
    /// <remarks>
    ///     Multiple values are treated as OR (matches any of them).
    /// </remarks>
    public Optional<IList<MessageSearchEmbedType>> EmbedTypes { get; set; }

    /// <summary>
    ///     Gets or sets the attachment file extensions to filter by.
    /// </summary>
    /// <remarks>
    ///     Multiple values are treated as OR (matches any of them).
    /// </remarks>
    public Optional<IList<string>> AttachmentExtensions { get; set; }

    /// <summary>
    ///     Gets or sets the attachment filenames to filter by.
    /// </summary>
    /// <remarks>
    ///     Multiple values are treated as OR (matches any of them).
    /// </remarks>
    public Optional<IList<string>> AttachmentFileNames { get; set; }

    /// <summary>
    ///     Gets or sets whether to filter by pinned messages.
    /// </summary>
    public Optional<bool> IsPinned { get; set; }

    /// <summary>
    ///     Gets or sets the application command ID to filter by.
    /// </summary>
    public Optional<Snowflake> CommandId { get; set; }

    /// <summary>
    ///     Gets or sets the application command name to filter by.
    /// </summary>
    public Optional<string> CommandName { get; set; }

    /// <summary>
    ///     Gets or sets whether to include results from age-restricted channels.
    /// </summary>
    public Optional<bool> IncludesAgeRestrictedChannels { get; set; }

    /// <summary>
    ///     Gets or sets the channel IDs to search within.
    /// </summary>
    /// <remarks>
    ///     Multiple values are treated as OR (matches any of them).
    ///     A maximum of 500 channel IDs can be specified.
    /// </remarks>
    public Optional<IList<Snowflake>> ChannelIds { get; set; }

    /// <summary>
    ///     Instantiates a new <see cref="LocalMessageSearch"/>.
    /// </summary>
    public LocalMessageSearch()
    { }

    /// <summary>
    ///     Instantiates a new <see cref="LocalMessageSearch"/> with the properties copied from another instance.
    /// </summary>
    /// <param name="other"> The other instance to copy properties from. </param>
    protected LocalMessageSearch(LocalMessageSearch other)
    {
        Contents = other.Contents.Clone();
        WordProximity = other.WordProximity;
        AuthorIds = other.AuthorIds.Clone();
        IncludedAuthorTypes = other.IncludedAuthorTypes.Clone();
        ExcludedAuthorTypes = other.ExcludedAuthorTypes.Clone();
        MentionedUserIds = other.MentionedUserIds.Clone();
        MentionsEveryone = other.MentionsEveryone;
        IncludedFilters = other.IncludedFilters.Clone();
        ExcludedFilters = other.ExcludedFilters.Clone();
        LinkHostNames = other.LinkHostNames.Clone();
        EmbedProviders = other.EmbedProviders.Clone();
        EmbedTypes = other.EmbedTypes.Clone();
        AttachmentExtensions = other.AttachmentExtensions.Clone();
        AttachmentFileNames = other.AttachmentFileNames.Clone();
        IsPinned = other.IsPinned;
        CommandId = other.CommandId;
        CommandName = other.CommandName;
        IncludesAgeRestrictedChannels = other.IncludesAgeRestrictedChannels;
        ChannelIds = other.ChannelIds.Clone();
    }

    /// <inheritdoc/>
    public virtual LocalMessageSearch Clone()
    {
        return new(this);
    }
}
