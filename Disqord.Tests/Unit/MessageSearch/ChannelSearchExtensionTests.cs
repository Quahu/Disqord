using System.Collections.Generic;

namespace Disqord.Tests.Unit.MessageSearch;

[TestFixture]
public class ChannelSearchExtensionTests
{
    [Test]
    public void ChannelSearch_OverwritesChannelIds_WhenEmpty()
    {
        // Arrange
        var search = new LocalMessageSearch();
        Snowflake channelId = 123456789;

        // Act
        search.ChannelIds = new List<Snowflake> { channelId };

        // Assert
        Assert.That(search.ChannelIds.Value, Has.Count.EqualTo(1));
        Assert.That(search.ChannelIds.Value[0], Is.EqualTo(channelId));
    }

    [Test]
    public void ChannelSearch_OverwritesChannelIds_WhenAlreadySet()
    {
        // Arrange
        Snowflake originalId = 111111111;
        Snowflake newId = 222222222;
        var search = new LocalMessageSearch
        {
            ChannelIds = new List<Snowflake> { originalId }
        };

        // Act
        search.ChannelIds = new List<Snowflake> { newId };

        // Assert
        Assert.That(search.ChannelIds.Value, Has.Count.EqualTo(1));
        Assert.That(search.ChannelIds.Value[0], Is.EqualTo(newId));
        Assert.That(search.ChannelIds.Value, Does.Not.Contain(originalId));
    }

    [Test]
    public void ChannelSearch_OverwritesChannelIds_WhenMultipleAlreadySet()
    {
        // Arrange
        var search = new LocalMessageSearch
        {
            ChannelIds = new List<Snowflake> { new(111), new(222), new(333) }
        };

        Snowflake channelId = 444444444;

        // Act
        search.ChannelIds = new List<Snowflake> { channelId };

        // Assert
        Assert.That(search.ChannelIds.Value, Has.Count.EqualTo(1));
        Assert.That(search.ChannelIds.Value[0], Is.EqualTo(channelId));
    }
}
