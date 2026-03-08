using System.Collections.Generic;

namespace Disqord.Tests.Unit.MessageSearch;

[TestFixture]
public class LocalMessageSearchTests
{
    [Test]
    public void DefaultConstructor_AllPropertiesAreDefault()
    {
        // Arrange & Act
        var search = new LocalMessageSearch();

        // Assert
        Assert.That(search.Contents.HasValue, Is.False);
        Assert.That(search.WordProximity.HasValue, Is.False);
        Assert.That(search.AuthorIds.HasValue, Is.False);
        Assert.That(search.IncludedAuthorTypes.HasValue, Is.False);
        Assert.That(search.ExcludedAuthorTypes.HasValue, Is.False);
        Assert.That(search.MentionedUserIds.HasValue, Is.False);
        Assert.That(search.MentionsEveryone.HasValue, Is.False);
        Assert.That(search.IncludedFilters.HasValue, Is.False);
        Assert.That(search.ExcludedFilters.HasValue, Is.False);
        Assert.That(search.LinkHostNames.HasValue, Is.False);
        Assert.That(search.EmbedProviders.HasValue, Is.False);
        Assert.That(search.EmbedTypes.HasValue, Is.False);
        Assert.That(search.AttachmentExtensions.HasValue, Is.False);
        Assert.That(search.AttachmentFileNames.HasValue, Is.False);
        Assert.That(search.IsPinned.HasValue, Is.False);
        Assert.That(search.CommandId.HasValue, Is.False);
        Assert.That(search.CommandName.HasValue, Is.False);
        Assert.That(search.IncludesAgeRestrictedChannels.HasValue, Is.False);
        Assert.That(search.ChannelIds.HasValue, Is.False);
    }

    [Test]
    public void SetProperties_ValuesAreRetained()
    {
        // Arrange & Act
        var search = new LocalMessageSearch
        {
            Contents = new List<string> { "hello" },
            WordProximity = 5,
            AuthorIds = new List<Snowflake> { 123456789 },
            IsPinned = true,
            IncludesAgeRestrictedChannels = false,
        };

        // Assert
        Assert.That(search.Contents.Value, Has.Count.EqualTo(1));
        Assert.That(search.Contents.Value[0], Is.EqualTo("hello"));
        Assert.That(search.WordProximity.Value, Is.EqualTo(5));
        Assert.That(search.AuthorIds.Value, Has.Count.EqualTo(1));
        Assert.That(search.IsPinned.Value, Is.True);
        Assert.That(search.IncludesAgeRestrictedChannels.Value, Is.False);
    }

    [Test]
    public void Clone_CreatesIndependentCopy()
    {
        // Arrange
        var original = new LocalMessageSearch
        {
            Contents = new List<string> { "test" },
            WordProximity = 3,
            IsPinned = true,
        };

        // Act
        var clone = original.Clone();
        clone.Contents.Value.Add("another");

        // Assert
        Assert.That(original.Contents.Value, Has.Count.EqualTo(1));
        Assert.That(clone.Contents.Value, Has.Count.EqualTo(2));
    }

    [Test]
    public void Clone_CopiesAllProperties()
    {
        // Arrange
        var original = new LocalMessageSearch
        {
            Contents = new List<string> { "a", "b" },
            WordProximity = 10,
            AuthorIds = new List<Snowflake> { 1, 2 },
            IncludedAuthorTypes = new List<MessageSearchAuthorType> { MessageSearchAuthorType.User },
            ExcludedAuthorTypes = new List<MessageSearchAuthorType> { MessageSearchAuthorType.Bot },
            MentionedUserIds = new List<Snowflake> { 3 },
            MentionsEveryone = true,
            IncludedFilters = new List<MessageSearchFilter> { MessageSearchFilter.Link },
            ExcludedFilters = new List<MessageSearchFilter> { MessageSearchFilter.File },
            LinkHostNames = new List<string> { "example.com" },
            EmbedProviders = new List<string> { "YouTube" },
            EmbedTypes = new List<MessageSearchEmbedType> { MessageSearchEmbedType.Video },
            AttachmentExtensions = new List<string> { "png" },
            AttachmentFileNames = new List<string> { "test.png" },
            IsPinned = false,
            CommandId = (Snowflake) 999,
            CommandName = "cmd",
            IncludesAgeRestrictedChannels = true,
            ChannelIds = new List<Snowflake> { 100 },
        };

        // Act
        var clone = original.Clone();

        // Assert
        Assert.That(clone.Contents.Value, Is.EqualTo(original.Contents.Value));
        Assert.That(clone.WordProximity.Value, Is.EqualTo(original.WordProximity.Value));
        Assert.That(clone.AuthorIds.Value, Is.EqualTo(original.AuthorIds.Value));
        Assert.That(clone.IncludedAuthorTypes.Value, Is.EqualTo(original.IncludedAuthorTypes.Value));
        Assert.That(clone.ExcludedAuthorTypes.Value, Is.EqualTo(original.ExcludedAuthorTypes.Value));
        Assert.That(clone.MentionedUserIds.Value, Is.EqualTo(original.MentionedUserIds.Value));
        Assert.That(clone.MentionsEveryone.Value, Is.EqualTo(original.MentionsEveryone.Value));
        Assert.That(clone.IncludedFilters.Value, Is.EqualTo(original.IncludedFilters.Value));
        Assert.That(clone.ExcludedFilters.Value, Is.EqualTo(original.ExcludedFilters.Value));
        Assert.That(clone.LinkHostNames.Value, Is.EqualTo(original.LinkHostNames.Value));
        Assert.That(clone.EmbedProviders.Value, Is.EqualTo(original.EmbedProviders.Value));
        Assert.That(clone.EmbedTypes.Value, Is.EqualTo(original.EmbedTypes.Value));
        Assert.That(clone.AttachmentExtensions.Value, Is.EqualTo(original.AttachmentExtensions.Value));
        Assert.That(clone.AttachmentFileNames.Value, Is.EqualTo(original.AttachmentFileNames.Value));
        Assert.That(clone.IsPinned.Value, Is.EqualTo(original.IsPinned.Value));
        Assert.That(clone.CommandId.Value, Is.EqualTo(original.CommandId.Value));
        Assert.That(clone.CommandName.Value, Is.EqualTo(original.CommandName.Value));
        Assert.That(clone.IncludesAgeRestrictedChannels.Value, Is.EqualTo(original.IncludesAgeRestrictedChannels.Value));
        Assert.That(clone.ChannelIds.Value, Is.EqualTo(original.ChannelIds.Value));
    }
}
