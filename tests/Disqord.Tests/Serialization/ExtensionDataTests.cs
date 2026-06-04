using Disqord.Serialization.Json;
using Disqord.Tests.Models;

namespace Disqord.Tests.Serialization;

[TestFixture]
public class ExtensionDataTests : SerializationTestBase
{
    [Test]
    public void Deserialize_UnknownProperties_CapturedInExtensionData()
    {
        const string json = """{"name":"test","unknown_field":"value"}""";

        var result = Deserialize<ExtensionDataTestModel>(json);

        Assert.That(result.Name, Is.EqualTo("test"));
        Assert.That(JsonModel.TryGetExtensionData(result, out var ext), Is.True);
        Assert.That(ext, Does.ContainKey("unknown_field"));
    }

    [Test]
    public void Deserialize_KnownProperties_NotInExtensionData()
    {
        const string json = """{"name":"test"}""";

        var result = Deserialize<ExtensionDataTestModel>(json);

        Assert.That(result.Name, Is.EqualTo("test"));
        if (JsonModel.TryGetExtensionData(result, out var ext))
        {
            Assert.That(ext, Does.Not.ContainKey("name"));
        }
    }

    [Test]
    public void Deserialize_ExtensionData_StringValue()
    {
        const string json = """{"name":"test","extra":"hello"}""";

        var result = Deserialize<ExtensionDataTestModel>(json);

        Assert.That(JsonModel.TryGetExtensionDatum<string>(result, "extra", out var value), Is.True);
        Assert.That(value, Is.EqualTo("hello"));
    }

    [Test]
    public void Deserialize_ExtensionData_NumberValue()
    {
        const string json = """{"name":"test","extra":42}""";

        var result = Deserialize<ExtensionDataTestModel>(json);

        Assert.That(JsonModel.TryGetExtensionDatum<int>(result, "extra", out var value), Is.True);
        Assert.That(value, Is.EqualTo(42));
    }

    [Test]
    public void Deserialize_ExtensionData_ObjectValue()
    {
        const string json = """{"name":"test","extra":{"nested":"value"}}""";

        var result = Deserialize<ExtensionDataTestModel>(json);

        Assert.That(JsonModel.TryGetExtensionData(result, out var ext), Is.True);
        Assert.That(ext!["extra"]!.Type, Is.EqualTo(JsonValueType.Object));
    }

    [Test]
    public void Deserialize_ExtensionData_ArrayValue()
    {
        const string json = """{"name":"test","extra":[1,2,3]}""";

        var result = Deserialize<ExtensionDataTestModel>(json);

        Assert.That(JsonModel.TryGetExtensionData(result, out var ext), Is.True);
        Assert.That(ext!["extra"]!.Type, Is.EqualTo(JsonValueType.Array));
    }

    [Test]
    public void Deserialize_ExtensionData_NullValue()
    {
        const string json = """{"name":"test","extra":null}""";

        var result = Deserialize<ExtensionDataTestModel>(json);

        Assert.That(JsonModel.TryGetExtensionData(result, out var ext), Is.True);
        Assert.That(ext!.ContainsKey("extra"), Is.True);
        Assert.That(ext["extra"], Is.Null);
    }

    [Test]
    public void RoundTrip_ExtensionData_Preserved()
    {
        const string json = """{"name":"test","extra_string":"hello","extra_num":42}""";

        var deserialized = Deserialize<ExtensionDataTestModel>(json);
        var serialized = Serialize(deserialized);
        var reDeserialized = Deserialize<ExtensionDataTestModel>(serialized);

        Assert.That(reDeserialized.Name, Is.EqualTo("test"));
        Assert.That(JsonModel.TryGetExtensionDatum<string>(reDeserialized, "extra_string", out var str), Is.True);
        Assert.That(str, Is.EqualTo("hello"));
        Assert.That(JsonModel.TryGetExtensionDatum<int>(reDeserialized, "extra_num", out var num), Is.True);
        Assert.That(num, Is.EqualTo(42));
    }

    [Test]
    public void Deserialize_MultipleUnknownProperties_AllCaptured()
    {
        const string json = """{"name":"test","a":"1","b":"2","c":"3"}""";

        var result = Deserialize<ExtensionDataTestModel>(json);

        Assert.That(JsonModel.TryGetExtensionData(result, out var ext), Is.True);
        Assert.That(ext, Has.Count.EqualTo(3));
        Assert.That(ext, Does.ContainKey("a"));
        Assert.That(ext, Does.ContainKey("b"));
        Assert.That(ext, Does.ContainKey("c"));
    }

    [Test]
    public void Deserialize_NoExtensionData_EmptyOrMissing()
    {
        const string json = """{"name":"test"}""";

        var result = Deserialize<ExtensionDataTestModel>(json);

        if (JsonModel.TryGetExtensionData(result, out var ext))
        {
            Assert.That(ext, Is.Empty);
        }
    }
}
