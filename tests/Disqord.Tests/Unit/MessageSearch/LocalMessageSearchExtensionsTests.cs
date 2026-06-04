namespace Disqord.Tests.Unit.MessageSearch;

[TestFixture]
public class LocalMessageSearchExtensionsTests
{
    [Test]
    public void WithContents_SetsContents()
    {
        // Arrange
        var search = new LocalMessageSearch();

        // Act
        var result = search.WithContents("hello", "world");

        // Assert
        Assert.That(result, Is.SameAs(search));
        Assert.That(result.Contents.Value, Is.EqualTo(new[] { "hello", "world" }));
    }

    [Test]
    public void AddContent_AddsToEmptyOptional()
    {
        // Arrange & Act
        var search = new LocalMessageSearch().AddContent("hello");

        // Assert
        Assert.That(search.Contents.Value, Has.Count.EqualTo(1));
        Assert.That(search.Contents.Value[0], Is.EqualTo("hello"));
    }

    [Test]
    public void AddContent_AppendsToExistingList()
    {
        // Arrange & Act
        var search = new LocalMessageSearch()
            .AddContent("hello")
            .AddContent("world");

        // Assert
        Assert.That(search.Contents.Value, Is.EqualTo(new[] { "hello", "world" }));
    }

    [Test]
    public void WithWordProximity_SetsWordProximity()
    {
        // Arrange & Act
        var search = new LocalMessageSearch().WithWordProximity(42);

        // Assert
        Assert.That(search.WordProximity.Value, Is.EqualTo(42));
    }

    [Test]
    public void WithAuthorIds_SetsAuthorIds()
    {
        // Arrange & Act
        Snowflake authorId1 = 111;
        Snowflake authorId2 = 222;
        var search = new LocalMessageSearch().WithAuthorIds(authorId1, authorId2);

        // Assert
        Assert.That(search.AuthorIds.Value, Is.EqualTo(new[] { authorId1, authorId2 }));
    }

    [Test]
    public void AddAuthorId_AddsToEmptyOptional()
    {
        // Arrange & Act
        Snowflake authorId = 111;
        var search = new LocalMessageSearch().AddAuthorId(authorId);

        // Assert
        Assert.That(search.AuthorIds.Value, Has.Count.EqualTo(1));
        Assert.That(search.AuthorIds.Value[0], Is.EqualTo(authorId));
    }

    [Test]
    public void WithIncludedAuthorTypes_SetsIncludedAuthorTypes()
    {
        // Arrange & Act
        var search = new LocalMessageSearch()
            .WithIncludedAuthorTypes(MessageSearchAuthorType.User, MessageSearchAuthorType.Bot);

        // Assert
        Assert.That(search.IncludedAuthorTypes.Value, Has.Count.EqualTo(2));
        Assert.That(search.IncludedAuthorTypes.Value, Does.Contain(MessageSearchAuthorType.User));
        Assert.That(search.IncludedAuthorTypes.Value, Does.Contain(MessageSearchAuthorType.Bot));
    }

    [Test]
    public void AddIncludedAuthorType_AddsToEmptyOptional()
    {
        // Arrange & Act
        var search = new LocalMessageSearch().AddIncludedAuthorType(MessageSearchAuthorType.User);

        // Assert
        Assert.That(search.IncludedAuthorTypes.Value, Has.Count.EqualTo(1));
        Assert.That(search.IncludedAuthorTypes.Value[0], Is.EqualTo(MessageSearchAuthorType.User));
    }

    [Test]
    public void WithExcludedAuthorTypes_SetsExcludedAuthorTypes()
    {
        // Arrange & Act
        var search = new LocalMessageSearch()
            .WithExcludedAuthorTypes(MessageSearchAuthorType.Webhook);

        // Assert
        Assert.That(search.ExcludedAuthorTypes.Value, Has.Count.EqualTo(1));
        Assert.That(search.ExcludedAuthorTypes.Value[0], Is.EqualTo(MessageSearchAuthorType.Webhook));
    }

    [Test]
    public void AddExcludedAuthorType_AddsToEmptyOptional()
    {
        // Arrange & Act
        var search = new LocalMessageSearch().AddExcludedAuthorType(MessageSearchAuthorType.Bot);

        // Assert
        Assert.That(search.ExcludedAuthorTypes.Value, Has.Count.EqualTo(1));
        Assert.That(search.ExcludedAuthorTypes.Value[0], Is.EqualTo(MessageSearchAuthorType.Bot));
    }

    [Test]
    public void AddMentionedUserId_AddsToEmptyOptional()
    {
        // Arrange & Act
        Snowflake userId = 555;
        var search = new LocalMessageSearch().AddMentionedUserId(userId);

        // Assert
        Assert.That(search.MentionedUserIds.Value, Has.Count.EqualTo(1));
        Assert.That(search.MentionedUserIds.Value[0], Is.EqualTo(userId));
    }

    [Test]
    public void WithIncludedFilters_SetsIncludedFilters()
    {
        // Arrange & Act
        var search = new LocalMessageSearch()
            .WithIncludedFilters(MessageSearchFilter.Link, MessageSearchFilter.Embed);

        // Assert
        Assert.That(search.IncludedFilters.Value, Has.Count.EqualTo(2));
    }

    [Test]
    public void AddIncludedFilter_AddsToEmptyOptional()
    {
        // Arrange & Act
        var search = new LocalMessageSearch().AddIncludedFilter(MessageSearchFilter.Link);

        // Assert
        Assert.That(search.IncludedFilters.Value, Has.Count.EqualTo(1));
        Assert.That(search.IncludedFilters.Value[0], Is.EqualTo(MessageSearchFilter.Link));
    }

    [Test]
    public void WithExcludedFilters_SetsExcludedFilters()
    {
        // Arrange & Act
        var search = new LocalMessageSearch()
            .WithExcludedFilters(MessageSearchFilter.File);

        // Assert
        Assert.That(search.ExcludedFilters.Value, Has.Count.EqualTo(1));
    }

    [Test]
    public void AddExcludedFilter_AddsToEmptyOptional()
    {
        // Arrange & Act
        var search = new LocalMessageSearch().AddExcludedFilter(MessageSearchFilter.File);

        // Assert
        Assert.That(search.ExcludedFilters.Value, Has.Count.EqualTo(1));
        Assert.That(search.ExcludedFilters.Value[0], Is.EqualTo(MessageSearchFilter.File));
    }

    [Test]
    public void WithMentionsEveryone_SetsMentionsEveryone()
    {
        // Arrange & Act
        var search = new LocalMessageSearch().WithMentionsEveryone();

        // Assert
        Assert.That(search.MentionsEveryone.Value, Is.True);
    }

    [Test]
    public void WithMentionsEveryone_False_SetsFalse()
    {
        // Arrange & Act
        var search = new LocalMessageSearch().WithMentionsEveryone(false);

        // Assert
        Assert.That(search.MentionsEveryone.Value, Is.False);
    }

    [Test]
    public void AddLinkHostName_AddsToEmptyOptional()
    {
        // Arrange & Act
        var search = new LocalMessageSearch().AddLinkHostName("example.com");

        // Assert
        Assert.That(search.LinkHostNames.Value, Has.Count.EqualTo(1));
        Assert.That(search.LinkHostNames.Value[0], Is.EqualTo("example.com"));
    }

    [Test]
    public void AddEmbedProvider_AddsToEmptyOptional()
    {
        // Arrange & Act
        var search = new LocalMessageSearch().AddEmbedProvider("YouTube");

        // Assert
        Assert.That(search.EmbedProviders.Value, Has.Count.EqualTo(1));
        Assert.That(search.EmbedProviders.Value[0], Is.EqualTo("YouTube"));
    }

    [Test]
    public void AddEmbedType_AddsToEmptyOptional()
    {
        // Arrange & Act
        var search = new LocalMessageSearch().AddEmbedType(MessageSearchEmbedType.Video);

        // Assert
        Assert.That(search.EmbedTypes.Value, Has.Count.EqualTo(1));
        Assert.That(search.EmbedTypes.Value[0], Is.EqualTo(MessageSearchEmbedType.Video));
    }

    [Test]
    public void AddAttachmentExtension_AddsToEmptyOptional()
    {
        // Arrange & Act
        var search = new LocalMessageSearch().AddAttachmentExtension("png");

        // Assert
        Assert.That(search.AttachmentExtensions.Value, Has.Count.EqualTo(1));
        Assert.That(search.AttachmentExtensions.Value[0], Is.EqualTo("png"));
    }

    [Test]
    public void AddAttachmentFileName_AddsToEmptyOptional()
    {
        // Arrange & Act
        var search = new LocalMessageSearch().AddAttachmentFileName("test.png");

        // Assert
        Assert.That(search.AttachmentFileNames.Value, Has.Count.EqualTo(1));
        Assert.That(search.AttachmentFileNames.Value[0], Is.EqualTo("test.png"));
    }

    [Test]
    public void WithIsPinned_SetsIsPinned()
    {
        // Arrange & Act
        var search = new LocalMessageSearch().WithIsPinned();

        // Assert
        Assert.That(search.IsPinned.Value, Is.True);
    }

    [Test]
    public void WithCommandId_SetsCommandId()
    {
        // Arrange & Act
        Snowflake commandId = 999;
        var search = new LocalMessageSearch().WithCommandId(commandId);

        // Assert
        Assert.That(search.CommandId.Value, Is.EqualTo(commandId));
    }

    [Test]
    public void WithCommandName_SetsCommandName()
    {
        // Arrange & Act
        var search = new LocalMessageSearch().WithCommandName("my-command");

        // Assert
        Assert.That(search.CommandName.Value, Is.EqualTo("my-command"));
    }

    [Test]
    public void WithIncludeNsfw_SetsIncludeNsfw()
    {
        // Arrange & Act
        var search = new LocalMessageSearch().WithIncludesAgeRestrictedChannels();

        // Assert
        Assert.That(search.IncludesAgeRestrictedChannels.Value, Is.True);
    }

    [Test]
    public void WithChannelIds_SetsChannelIds()
    {
        // Arrange & Act
        Snowflake channelId1 = 100;
        Snowflake channelId2 = 200;
        var search = new LocalMessageSearch().WithChannelIds(channelId1, channelId2);

        // Assert
        Assert.That(search.ChannelIds.Value, Is.EqualTo(new[] { channelId1, channelId2 }));
    }

    [Test]
    public void AddChannelId_AddsToEmptyOptional()
    {
        // Arrange & Act
        Snowflake channelId = 100;
        var search = new LocalMessageSearch().AddChannelId(channelId);

        // Assert
        Assert.That(search.ChannelIds.Value, Has.Count.EqualTo(1));
        Assert.That(search.ChannelIds.Value[0], Is.EqualTo(channelId));
    }

    [Test]
    public void FluentChaining_AllMethodsChain()
    {
        // Arrange & Act
        var search = new LocalMessageSearch()
            .WithContents("test")
            .WithWordProximity(5)
            .WithAuthorIds(1)
            .WithIncludedAuthorTypes(MessageSearchAuthorType.User)
            .WithExcludedAuthorTypes(MessageSearchAuthorType.Bot)
            .WithMentionedUserIds(2)
            .WithMentionsEveryone()
            .WithIncludedFilters(MessageSearchFilter.Link)
            .WithExcludedFilters(MessageSearchFilter.File)
            .WithLinkHostNames("example.com")
            .WithEmbedProviders("YouTube")
            .WithEmbedTypes(MessageSearchEmbedType.Video)
            .WithAttachmentExtensions("png")
            .WithAttachmentFileNames("test.png")
            .WithIsPinned(false)
            .WithCommandId(999)
            .WithCommandName("cmd")
            .WithIncludesAgeRestrictedChannels(true)
            .WithChannelIds(100);

        // Assert
        Assert.That(search.Contents.Value[0], Is.EqualTo("test"));
        Assert.That(search.WordProximity.Value, Is.EqualTo(5));
        Assert.That(search.IsPinned.Value, Is.False);
    }

    [Test]
    public void FluentChaining_AddMethodsChain()
    {
        // Arrange & Act
        var search = new LocalMessageSearch()
            .AddContent("test")
            .AddAuthorId(1)
            .AddIncludedAuthorType(MessageSearchAuthorType.User)
            .AddExcludedAuthorType(MessageSearchAuthorType.Bot)
            .AddMentionedUserId(2)
            .AddIncludedFilter(MessageSearchFilter.Link)
            .AddExcludedFilter(MessageSearchFilter.File)
            .AddLinkHostName("example.com")
            .AddEmbedProvider("YouTube")
            .AddEmbedType(MessageSearchEmbedType.Video)
            .AddAttachmentExtension("png")
            .AddAttachmentFileName("test.png")
            .AddChannelId(100);

        // Assert
        Assert.That(search.Contents.Value, Has.Count.EqualTo(1));
        Assert.That(search.AuthorIds.Value, Has.Count.EqualTo(1));
        Assert.That(search.IncludedAuthorTypes.Value, Has.Count.EqualTo(1));
        Assert.That(search.ExcludedAuthorTypes.Value, Has.Count.EqualTo(1));
        Assert.That(search.MentionedUserIds.Value, Has.Count.EqualTo(1));
        Assert.That(search.IncludedFilters.Value, Has.Count.EqualTo(1));
        Assert.That(search.ExcludedFilters.Value, Has.Count.EqualTo(1));
        Assert.That(search.LinkHostNames.Value, Has.Count.EqualTo(1));
        Assert.That(search.EmbedProviders.Value, Has.Count.EqualTo(1));
        Assert.That(search.EmbedTypes.Value, Has.Count.EqualTo(1));
        Assert.That(search.AttachmentExtensions.Value, Has.Count.EqualTo(1));
        Assert.That(search.AttachmentFileNames.Value, Has.Count.EqualTo(1));
        Assert.That(search.ChannelIds.Value, Has.Count.EqualTo(1));
    }
}
