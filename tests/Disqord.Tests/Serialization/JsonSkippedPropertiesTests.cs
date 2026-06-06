using Disqord.Serialization.Json;
using Disqord.Tests.Models;

namespace Disqord.Tests.Serialization;

[TestFixture]
public class JsonSkippedPropertiesTests : SerializationTestBase
{
    [Test]
    public void Deserialize_SkippedProperty_NotInExtensionData()
    {
        const string json = """{"name":"test","skipped":"should_be_ignored"}""";

        var result = Deserialize<SkippedPropertiesTestModel>(json);

        Assert.That(result.Name, Is.EqualTo("test"));
        if (JsonModel.TryGetExtensionData(result, out var ext))
        {
            Assert.That(ext, Does.Not.ContainKey("skipped"));
        }
    }

    [Test]
    public void Deserialize_SecondSkippedProperty_NotInExtensionData()
    {
        const string json = """{"name":"test","also_skipped":"should_be_ignored"}""";

        var result = Deserialize<SkippedPropertiesTestModel>(json);

        if (JsonModel.TryGetExtensionData(result, out var ext))
        {
            Assert.That(ext, Does.Not.ContainKey("also_skipped"));
        }
    }

    [Test]
    public void Deserialize_NonSkippedUnknownProperty_InExtensionData()
    {
        const string json = """{"name":"test","skipped":"no","other":"yes"}""";

        var result = Deserialize<SkippedPropertiesTestModel>(json);

        Assert.That(JsonModel.TryGetExtensionData(result, out var ext), Is.True);
        Assert.That(ext, Does.Not.ContainKey("skipped"));
        Assert.That(ext, Does.ContainKey("other"));
    }

    [Test]
    public void Deserialize_AllSkipped_NoExtensionData()
    {
        const string json = """{"name":"test","skipped":"a","also_skipped":"b"}""";

        var result = Deserialize<SkippedPropertiesTestModel>(json);

        if (JsonModel.TryGetExtensionData(result, out var ext))
        {
            Assert.That(ext, Is.Empty);
        }
    }

    [Test]
    public void Deserialize_InteractionJsonModel_DataNotInExtensionData()
    {
        const string json = """
            {
                "id": "123",
                "application_id": "456",
                "type": 2,
                "data": { "id": "789", "name": "test", "type": 1 },
                "token": "tok",
                "version": 1,
                "entitlements": []
            }
            """;

        var result = Deserialize<Disqord.Models.InteractionJsonModel>(json);

        if (JsonModel.TryGetExtensionData(result, out var ext))
        {
            Assert.That(ext, Does.Not.ContainKey("data"));
        }
    }

    [Test]
    public void Deserialize_MessageJsonModel_StickersNotInExtensionData()
    {
        const string json = """
            {
                "id": "1",
                "channel_id": "2",
                "content": "hi",
                "timestamp": "2024-01-01T00:00:00+00:00",
                "tts": false,
                "mention_everyone": false,
                "mentions": [],
                "mention_roles": [],
                "attachments": [],
                "embeds": [],
                "pinned": false,
                "type": 0,
                "stickers": [{ "id": "123" }]
            }
            """;

        var result = Deserialize<Disqord.Models.MessageJsonModel>(json);

        if (JsonModel.TryGetExtensionData(result, out var ext))
        {
            Assert.That(ext, Does.Not.ContainKey("stickers"));
        }
    }
}
