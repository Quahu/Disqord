using Disqord.Tests.Models;
using Qommon;

namespace Disqord.Tests.Serialization;

[TestFixture]
public class OptionalSerializationTests : SerializationTestBase
{
    [Test]
    public void Deserialize_OptionalString_Present()
    {
        const string json = """{"name":"hello"}""";

        var result = Deserialize<OptionalTestModel>(json);

        Assert.That(result.Name.HasValue, Is.True);
        Assert.That(result.Name.Value, Is.EqualTo("hello"));
    }

    [Test]
    public void Deserialize_OptionalString_Absent()
    {
        const string json = """{}""";

        var result = Deserialize<OptionalTestModel>(json);

        Assert.That(result.Name.HasValue, Is.False);
    }

    [Test]
    public void Serialize_OptionalString_Present_Included()
    {
        var model = new OptionalTestModel { Name = "hello" };

        var json = Serialize(model);

        Assert.That(json, Does.Contain(""""name":"hello""""));
    }

    [Test]
    public void Serialize_OptionalString_Absent_Omitted()
    {
        var model = new OptionalTestModel();

        var json = Serialize(model);

        Assert.That(json, Does.Not.Contain("name"));
    }

    [Test]
    public void Deserialize_OptionalInt_Present()
    {
        const string json = """{"count":42}""";

        var result = Deserialize<OptionalTestModel>(json);

        Assert.That(result.Count.HasValue, Is.True);
        Assert.That(result.Count.Value, Is.EqualTo(42));
    }

    [Test]
    public void Deserialize_OptionalInt_Absent()
    {
        const string json = """{}""";

        var result = Deserialize<OptionalTestModel>(json);

        Assert.That(result.Count.HasValue, Is.False);
    }

    [Test]
    public void Deserialize_OptionalNullableString_PresentWithValue()
    {
        const string json = """{"nullable_name":"hello"}""";

        var result = Deserialize<OptionalTestModel>(json);

        Assert.That(result.NullableName.HasValue, Is.True);
        Assert.That(result.NullableName.Value, Is.EqualTo("hello"));
    }

    [Test]
    public void Deserialize_OptionalNullableString_PresentWithNull()
    {
        const string json = """{"nullable_name":null}""";

        var result = Deserialize<OptionalTestModel>(json);

        Assert.That(result.NullableName.HasValue, Is.True);
        Assert.That(result.NullableName.Value, Is.Null);
    }

    [Test]
    public void Deserialize_OptionalNullableString_Absent()
    {
        const string json = """{}""";

        var result = Deserialize<OptionalTestModel>(json);

        Assert.That(result.NullableName.HasValue, Is.False);
    }

    [Test]
    public void Serialize_OptionalNullableString_PresentWithNull_WritesNull()
    {
        var model = new OptionalTestModel { NullableName = new Optional<string?>(null) };

        var json = Serialize(model);

        Assert.That(json, Does.Contain(""""nullable_name":null""""));
    }

    [Test]
    public void Serialize_OptionalNullableString_Absent_Omitted()
    {
        var model = new OptionalTestModel();

        var json = Serialize(model);

        Assert.That(json, Does.Not.Contain("nullable_name"));
    }

    [Test]
    public void Deserialize_OptionalNullableSnowflake_PresentWithValue()
    {
        const string json = """{"nullable_snowflake":"999"}""";

        var result = Deserialize<OptionalTestModel>(json);

        Assert.That(result.NullableSnowflake.HasValue, Is.True);
        Assert.That(result.NullableSnowflake.Value, Is.Not.Null);
        Assert.That(result.NullableSnowflake.Value!.Value.RawValue, Is.EqualTo(999UL));
    }

    [Test]
    public void Deserialize_OptionalNullableSnowflake_PresentWithNull()
    {
        const string json = """{"nullable_snowflake":null}""";

        var result = Deserialize<OptionalTestModel>(json);

        Assert.That(result.NullableSnowflake.HasValue, Is.True);
        Assert.That(result.NullableSnowflake.Value, Is.Null);
    }

    [Test]
    public void Deserialize_OptionalNullableSnowflake_Absent()
    {
        const string json = """{}""";

        var result = Deserialize<OptionalTestModel>(json);

        Assert.That(result.NullableSnowflake.HasValue, Is.False);
    }

    [Test]
    public void Deserialize_OptionalArray_Present()
    {
        const string json = """{"items":[1,2,3]}""";

        var result = Deserialize<OptionalTestModel>(json);

        Assert.That(result.Items.HasValue, Is.True);
        Assert.That(result.Items.Value, Is.EqualTo(new[] { 1, 2, 3 }));
    }

    [Test]
    public void Deserialize_OptionalArray_Absent()
    {
        const string json = """{}""";

        var result = Deserialize<OptionalTestModel>(json);

        Assert.That(result.Items.HasValue, Is.False);
    }

    [Test]
    public void Deserialize_OptionalNestedModel_Present()
    {
        const string json = """{"child":{"name":"test","value":42}}""";

        var result = Deserialize<OptionalTestModel>(json);

        Assert.That(result.Child.HasValue, Is.True);
        Assert.That(result.Child.Value.Name, Is.EqualTo("test"));
        Assert.That(result.Child.Value.Value, Is.EqualTo(42));
    }

    [Test]
    public void Deserialize_OptionalNestedModel_Absent()
    {
        const string json = """{}""";

        var result = Deserialize<OptionalTestModel>(json);

        Assert.That(result.Child.HasValue, Is.False);
    }

    [Test]
    public void RoundTrip_AllOptionalStates_Preserved()
    {
        var original = new OptionalTestModel
        {
            Name = "hello",
            Count = 42,
            NullableName = new Optional<string?>(null),
            Items = new[] { 1, 2, 3 }
        };

        var json = Serialize(original);
        var result = Deserialize<OptionalTestModel>(json);

        Assert.That(result.Name.Value, Is.EqualTo("hello"));
        Assert.That(result.Count.Value, Is.EqualTo(42));
        Assert.That(result.NullableName.HasValue, Is.True);
        Assert.That(result.NullableName.Value, Is.Null);
        Assert.That(result.Items.Value, Is.EqualTo(new[] { 1, 2, 3 }));
        Assert.That(result.NullableSnowflake.HasValue, Is.False);
    }
}
