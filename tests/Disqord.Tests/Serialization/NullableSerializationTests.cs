using Disqord.Tests.Models;

namespace Disqord.Tests.Serialization;

[TestFixture]
public class NullableSerializationTests : SerializationTestBase
{
    [Test]
    public void Deserialize_NullableInt_WithValue()
    {
        const string json = """{"nullable_int":42}""";

        var result = Deserialize<NullableTestModel>(json);

        Assert.That(result.NullableInt, Is.EqualTo(42));
    }

    [Test]
    public void Deserialize_NullableInt_Null()
    {
        const string json = """{"nullable_int":null}""";

        var result = Deserialize<NullableTestModel>(json);

        Assert.That(result.NullableInt, Is.Null);
    }

    [Test]
    public void Deserialize_NullableString_WithValue()
    {
        const string json = """{"nullable_string":"hello"}""";

        var result = Deserialize<NullableTestModel>(json);

        Assert.That(result.NullableString, Is.EqualTo("hello"));
    }

    [Test]
    public void Deserialize_NullableString_Null()
    {
        const string json = """{"nullable_string":null}""";

        var result = Deserialize<NullableTestModel>(json);

        Assert.That(result.NullableString, Is.Null);
    }

    [Test]
    public void Deserialize_NullableSnowflake_WithValue()
    {
        const string json = """{"nullable_snowflake":"999"}""";

        var result = Deserialize<NullableTestModel>(json);

        Assert.That(result.NullableSnowflake, Is.Not.Null);
        Assert.That(result.NullableSnowflake!.Value.RawValue, Is.EqualTo(999UL));
    }

    [Test]
    public void Deserialize_NullableSnowflake_Null()
    {
        const string json = """{"nullable_snowflake":null}""";

        var result = Deserialize<NullableTestModel>(json);

        Assert.That(result.NullableSnowflake, Is.Null);
    }

    [Test]
    public void Deserialize_NullableDateTimeOffset_WithValue()
    {
        const string json = """{"nullable_date":"2024-01-15T12:00:00+00:00"}""";

        var result = Deserialize<NullableTestModel>(json);

        Assert.That(result.NullableDate, Is.Not.Null);
        Assert.That(result.NullableDate!.Value.Year, Is.EqualTo(2024));
    }

    [Test]
    public void Deserialize_NullableDateTimeOffset_Null()
    {
        const string json = """{"nullable_date":null}""";

        var result = Deserialize<NullableTestModel>(json);

        Assert.That(result.NullableDate, Is.Null);
    }

    [Test]
    public void Serialize_NullableInt_WithValue()
    {
        var model = new NullableTestModel { NullableInt = 42 };

        var json = Serialize(model);

        Assert.That(json, Does.Contain(""""nullable_int":42""""));
    }

    [Test]
    public void Serialize_NullableInt_Null()
    {
        var model = new NullableTestModel { NullableInt = null };

        var json = Serialize(model);

        Assert.That(json, Does.Contain(""""nullable_int":null""""));
    }

    [Test]
    public void Serialize_NullValueHandling_Ignore_NullOmitted()
    {
        var model = new NullHandlingTestModel { Value = null, Always = null };

        var json = Serialize(model);

        Assert.That(json, Does.Not.Contain("value"));
        Assert.That(json, Does.Contain(""""always":null""""));
    }

    [Test]
    public void Serialize_NullValueHandling_Ignore_NonNullIncluded()
    {
        var model = new NullHandlingTestModel { Value = "present", Always = "also" };

        var json = Serialize(model);

        Assert.That(json, Does.Contain(""""value":"present""""));
        Assert.That(json, Does.Contain(""""always":"also""""));
    }

    [Test]
    public void RoundTrip_Nullable_Preserved()
    {
        var original = new NullableTestModel
        {
            NullableString = "hello",
            NullableInt = 42,
            NullableSnowflake = new Snowflake(999),
            NullableDate = new DateTimeOffset(2024, 1, 15, 12, 0, 0, TimeSpan.Zero)
        };

        var json = Serialize(original);
        var result = Deserialize<NullableTestModel>(json);

        Assert.That(result.NullableString, Is.EqualTo("hello"));
        Assert.That(result.NullableInt, Is.EqualTo(42));
        Assert.That(result.NullableSnowflake!.Value.RawValue, Is.EqualTo(999UL));
        Assert.That(result.NullableDate!.Value.Year, Is.EqualTo(2024));
    }
}
