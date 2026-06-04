using Disqord.Serialization.Json;
using Disqord.Tests.Models;

namespace Disqord.Tests.Serialization;

[TestFixture]
public class JsonNodeSerializationTests : SerializationTestBase
{
    [Test]
    public void Deserialize_JsonNode_Object()
    {
        const string json = """{"node":{"foo":"bar","num":42},"value":"test"}""";

        var result = Deserialize<JsonNodeTestModel>(json);

        Assert.That(result.Node.HasValue, Is.True);
        Assert.That(result.Node.Value!.Type, Is.EqualTo(JsonValueType.Object));
    }

    [Test]
    public void Deserialize_JsonNode_Array()
    {
        const string json = """{"node":[1,2,3],"value":"test"}""";

        var result = Deserialize<JsonNodeTestModel>(json);

        Assert.That(result.Node.HasValue, Is.True);
        Assert.That(result.Node.Value!.Type, Is.EqualTo(JsonValueType.Array));
    }

    [Test]
    public void Deserialize_JsonNode_String()
    {
        const string json = """{"node":"hello","value":"test"}""";

        var result = Deserialize<JsonNodeTestModel>(json);

        Assert.That(result.Node.HasValue, Is.True);
        Assert.That(result.Node.Value!.Type, Is.EqualTo(JsonValueType.String));
    }

    [Test]
    public void Deserialize_JsonNode_Number()
    {
        const string json = """{"node":42,"value":"test"}""";

        var result = Deserialize<JsonNodeTestModel>(json);

        Assert.That(result.Node.HasValue, Is.True);
        Assert.That(result.Node.Value!.Type, Is.EqualTo(JsonValueType.Number));
    }

    [Test]
    public void Deserialize_JsonNode_BoolTrue()
    {
        const string json = """{"node":true,"value":"test"}""";

        var result = Deserialize<JsonNodeTestModel>(json);

        Assert.That(result.Node.HasValue, Is.True);
        Assert.That(result.Node.Value!.Type, Is.EqualTo(JsonValueType.True));
    }

    [Test]
    public void Deserialize_JsonNode_BoolFalse()
    {
        const string json = """{"node":false,"value":"test"}""";

        var result = Deserialize<JsonNodeTestModel>(json);

        Assert.That(result.Node.HasValue, Is.True);
        Assert.That(result.Node.Value!.Type, Is.EqualTo(JsonValueType.False));
    }

    [Test]
    public void Deserialize_OptionalJsonNode_Absent()
    {
        const string json = """{"value":"test"}""";

        var result = Deserialize<JsonNodeTestModel>(json);

        Assert.That(result.Node.HasValue, Is.False);
    }

    [Test]
    public void JsonNode_ToType_String()
    {
        const string json = """{"node":"hello","value":"test"}""";

        var result = Deserialize<JsonNodeTestModel>(json);
        var stringValue = result.Node.Value!.ToType<string>();

        Assert.That(stringValue, Is.EqualTo("hello"));
    }

    [Test]
    public void JsonNode_ToType_Int()
    {
        const string json = """{"node":42,"value":"test"}""";

        var result = Deserialize<JsonNodeTestModel>(json);
        var intValue = result.Node.Value!.ToType<int>();

        Assert.That(intValue, Is.EqualTo(42));
    }

    [Test]
    public void JsonNode_ToType_Object()
    {
        const string json = """{"node":{"name":"test","value":42},"value":"test"}""";

        var result = Deserialize<JsonNodeTestModel>(json);
        var model = result.Node.Value!.ToType<SimpleTestModel>();

        Assert.That(model, Is.Not.Null);
        Assert.That(model!.Name, Is.EqualTo("test"));
        Assert.That(model.Value, Is.EqualTo(42));
    }

    [Test]
    public void JsonNode_RoundTrip_Object()
    {
        const string json = """{"node":{"foo":"bar"},"value":"test"}""";

        var deserialized = Deserialize<JsonNodeTestModel>(json);
        var serialized = Serialize(deserialized);
        var reDeserialized = Deserialize<JsonNodeTestModel>(serialized);

        Assert.That(reDeserialized.Node.HasValue, Is.True);
        Assert.That(reDeserialized.Node.Value!.Type, Is.EqualTo(JsonValueType.Object));
    }

    [Test]
    public void GetJsonNode_FromObject()
    {
        var model = new SimpleTestModel { Name = "hello", Value = 42 };

        var node = Serializer.GetJsonNode(model);

        Assert.That(node, Is.Not.Null);
        Assert.That(node!.Type, Is.EqualTo(JsonValueType.Object));
    }

    [Test]
    public void GetJsonNode_FromNull()
    {
        var node = Serializer.GetJsonNode(null);

        Assert.That(node, Is.Null);
    }
}
