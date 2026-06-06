using Disqord.Tests.Models;

namespace Disqord.Tests.Serialization;

[TestFixture]
public class StringConverterTests : SerializationTestBase
{
    [Test]
    public void Deserialize_String_FromStringToken()
    {
        const string json = """{"name":"hello","value":0}""";

        var result = Deserialize<SimpleTestModel>(json);

        Assert.That(result.Name, Is.EqualTo("hello"));
    }

    [Test]
    public void Deserialize_String_FromNumberToken()
    {
        const string json = """{"name":12345,"value":0}""";

        var result = Deserialize<SimpleTestModel>(json);

        Assert.That(result.Name, Is.EqualTo("12345"));
    }

    [Test]
    public void Deserialize_String_FromBoolTrue()
    {
        const string json = """{"name":true,"value":0}""";

        var result = Deserialize<SimpleTestModel>(json);

        Assert.That(result.Name, Is.EqualTo("True"));
    }

    [Test]
    public void Deserialize_String_FromBoolFalse()
    {
        const string json = """{"name":false,"value":0}""";

        var result = Deserialize<SimpleTestModel>(json);

        Assert.That(result.Name, Is.EqualTo("False"));
    }

    [Test]
    public void Deserialize_NullableString_Null()
    {
        const string json = """{"nullable_string":null}""";

        var result = Deserialize<NullableTestModel>(json);

        Assert.That(result.NullableString, Is.Null);
    }

    [Test]
    public void Serialize_String_WritesString()
    {
        var model = new SimpleTestModel { Name = "hello", Value = 0 };

        var json = Serialize(model);

        Assert.That(json, Does.Contain(""""name":"hello""""));
    }

    [Test]
    public void Serialize_NullString_WritesNull()
    {
        var model = new NullableTestModel { NullableString = null };

        var json = Serialize(model);

        Assert.That(json, Does.Contain(""""nullable_string":null""""));
    }
}
