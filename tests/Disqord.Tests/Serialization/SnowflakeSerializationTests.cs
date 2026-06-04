using Disqord.Tests.Models;

namespace Disqord.Tests.Serialization;

[TestFixture]
public class SnowflakeSerializationTests : SerializationTestBase
{
    [Test]
    public void Deserialize_Snowflake_FromString()
    {
        const string json = """{"id":"123456789"}""";

        var result = Deserialize<SnowflakeTestModel>(json);

        Assert.That(result.Id.RawValue, Is.EqualTo(123456789UL));
    }

    [Test]
    public void Deserialize_Snowflake_FromNumber()
    {
        const string json = """{"id":123456789}""";

        var result = Deserialize<SnowflakeTestModel>(json);

        Assert.That(result.Id.RawValue, Is.EqualTo(123456789UL));
    }

    [Test]
    public void Serialize_Snowflake_WritesString()
    {
        var model = new SnowflakeTestModel { Id = new Snowflake(123456789) };

        var json = Serialize(model);

        Assert.That(json, Does.Contain(""""id":"123456789""""));
    }

    [Test]
    public void Deserialize_OptionalSnowflake_Present()
    {
        const string json = """{"id":"1","optional_id":"999"}""";

        var result = Deserialize<SnowflakeTestModel>(json);

        Assert.That(result.OptionalId.HasValue, Is.True);
        Assert.That(result.OptionalId.Value.RawValue, Is.EqualTo(999UL));
    }

    [Test]
    public void Deserialize_OptionalSnowflake_Absent()
    {
        const string json = """{"id":"1"}""";

        var result = Deserialize<SnowflakeTestModel>(json);

        Assert.That(result.OptionalId.HasValue, Is.False);
    }

    [Test]
    public void Serialize_OptionalSnowflake_Present_Included()
    {
        var model = new SnowflakeTestModel
        {
            Id = new Snowflake(1),
            OptionalId = new Snowflake(999)
        };

        var json = Serialize(model);

        Assert.That(json, Does.Contain(""""optional_id":"999""""));
    }

    [Test]
    public void Serialize_OptionalSnowflake_Absent_Omitted()
    {
        var model = new SnowflakeTestModel { Id = new Snowflake(1) };

        var json = Serialize(model);

        Assert.That(json, Does.Not.Contain("optional_id"));
    }

    [Test]
    public void Deserialize_NullableSnowflake_WithValue()
    {
        const string json = """{"id":"1","nullable_id":"555"}""";

        var result = Deserialize<SnowflakeTestModel>(json);

        Assert.That(result.NullableId, Is.Not.Null);
        Assert.That(result.NullableId!.Value.RawValue, Is.EqualTo(555UL));
    }

    [Test]
    public void Deserialize_NullableSnowflake_Null()
    {
        const string json = """{"id":"1","nullable_id":null}""";

        var result = Deserialize<SnowflakeTestModel>(json);

        Assert.That(result.NullableId, Is.Null);
    }

    [Test]
    public void Deserialize_SnowflakeArray()
    {
        const string json = """{"id":"1","ids":["10","20","30"]}""";

        var result = Deserialize<SnowflakeTestModel>(json);

        Assert.That(result.Ids.HasValue, Is.True);
        Assert.That(result.Ids.Value, Has.Length.EqualTo(3));
        Assert.That(result.Ids.Value[0].RawValue, Is.EqualTo(10UL));
        Assert.That(result.Ids.Value[1].RawValue, Is.EqualTo(20UL));
        Assert.That(result.Ids.Value[2].RawValue, Is.EqualTo(30UL));
    }

    [Test]
    public void Deserialize_SnowflakeDictionary()
    {
        const string json = """
            {
                "id": "1",
                "snowflake_dict": {
                    "100": "alice",
                    "200": "bob"
                }
            }
            """;

        var result = Deserialize<SnowflakeTestModel>(json);

        Assert.That(result.SnowflakeDict.HasValue, Is.True);
        Assert.That(result.SnowflakeDict.Value, Has.Count.EqualTo(2));
        Assert.That(result.SnowflakeDict.Value[new Snowflake(100)], Is.EqualTo("alice"));
        Assert.That(result.SnowflakeDict.Value[new Snowflake(200)], Is.EqualTo("bob"));
    }

    [Test]
    public void Serialize_SnowflakeDictionary_KeysAreStrings()
    {
        var model = new SnowflakeTestModel
        {
            Id = new Snowflake(1),
            SnowflakeDict = new Dictionary<Snowflake, string>
            {
                [new Snowflake(100)] = "alice"
            }
        };

        var json = Serialize(model);

        Assert.That(json, Does.Contain(""""100":"alice""""));
    }

    [Test]
    public void RoundTrip_Snowflake_Preserved()
    {
        var original = new SnowflakeTestModel
        {
            Id = new Snowflake(123456789012345678),
            OptionalId = new Snowflake(987654321),
            NullableId = new Snowflake(555)
        };

        var json = Serialize(original);
        var result = Deserialize<SnowflakeTestModel>(json);

        Assert.That(result.Id, Is.EqualTo(original.Id));
        Assert.That(result.OptionalId.Value, Is.EqualTo(original.OptionalId.Value));
        Assert.That(result.NullableId, Is.EqualTo(original.NullableId));
    }

    [Test]
    public void Deserialize_LargeSnowflake_FromString()
    {
        const string json = """{"id":"18446744073709551615"}""";

        var result = Deserialize<SnowflakeTestModel>(json);

        Assert.That(result.Id.RawValue, Is.EqualTo(ulong.MaxValue));
    }
}
