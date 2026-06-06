using Disqord.Models;
using Disqord.Serialization.Json;
using Disqord.Serialization.Json.Default;
using Disqord.Serialization.Json.Newtonsoft;
using Disqord.Tests.Models;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace Disqord.Tests.Serialization;

[TestFixture]
public class CrossSerializerParityTests
{
    private DefaultJsonSerializer _stj = null!;
    private NewtonsoftJsonSerializer _newtonsoft = null!;

    [SetUp]
    public void SetUp()
    {
        _stj = new DefaultJsonSerializer();
        _newtonsoft = new NewtonsoftJsonSerializer(
            Options.Create(new NewtonsoftJsonSerializerConfiguration()),
            NullLogger<NewtonsoftJsonSerializer>.Instance);
    }

    private T DeserializeStj<T>(string json)
    {
        using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json));
        return _stj.Deserialize<T>(stream)!;
    }

    private T DeserializeNewtonsoft<T>(string json)
    {
        using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json));
        return _newtonsoft.Deserialize<T>(stream)!;
    }

    private string SerializeStj(object obj)
    {
        using var stream = new MemoryStream();
        _stj.Serialize(stream, obj);
        stream.Position = 0;
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    private string SerializeNewtonsoft(object obj)
    {
        using var stream = new MemoryStream();
        _newtonsoft.Serialize(stream, obj);
        stream.Position = 0;
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    [Test]
    public void Parity_Snowflake_SerializedWithId()
    {
        var model = new SnowflakeTestModel { Id = new Snowflake(123456789) };

        var stjJson = SerializeStj(model);
        var newtonsoftJson = SerializeNewtonsoft(model);

        Assert.That(stjJson, Does.Contain("123456789"));
        Assert.That(newtonsoftJson, Does.Contain("123456789"));
    }

    [Test]
    public void Parity_Snowflake_DeserializesFromString()
    {
        const string json = """{"id":"123456789"}""";

        var stjResult = DeserializeStj<SnowflakeTestModel>(json);
        var newtonsoftResult = DeserializeNewtonsoft<SnowflakeTestModel>(json);

        Assert.That(stjResult.Id, Is.EqualTo(newtonsoftResult.Id));
    }

    [Test]
    public void Parity_Optional_AbsentOmitted()
    {
        var model = new OptionalTestModel();

        var stjJson = SerializeStj(model);
        var newtonsoftJson = SerializeNewtonsoft(model);

        Assert.That(stjJson, Does.Not.Contain("name"));
        Assert.That(newtonsoftJson, Does.Not.Contain("name"));
    }

    [Test]
    public void Parity_Optional_PresentIncluded()
    {
        var model = new OptionalTestModel { Name = "hello" };

        var stjJson = SerializeStj(model);
        var newtonsoftJson = SerializeNewtonsoft(model);

        Assert.That(stjJson, Does.Contain(""""name":"hello""""));
        Assert.That(newtonsoftJson, Does.Contain(""""name":"hello""""));
    }

    [Test]
    public void Parity_NumericEnum_SerializedAsNumber()
    {
        var model = new EnumTestModel
        {
            NumericEnum = ComponentType.Button,
            StringEnum = TeamMemberRole.Administrator
        };

        var stjJson = SerializeStj(model);
        var newtonsoftJson = SerializeNewtonsoft(model);

        Assert.That(stjJson, Does.Contain(""""component_type":2""""));
        Assert.That(newtonsoftJson, Does.Contain(""""component_type":2""""));
    }

    [Test]
    public void Parity_StringEnum_SerializedAsString()
    {
        var model = new EnumTestModel
        {
            NumericEnum = ComponentType.Row,
            StringEnum = TeamMemberRole.ReadOnly
        };

        var stjJson = SerializeStj(model);
        var newtonsoftJson = SerializeNewtonsoft(model);

        Assert.That(stjJson, Does.Contain(""""team_role":"read_only""""));
        Assert.That(newtonsoftJson, Does.Contain(""""team_role":"read_only""""));
    }

    [Test]
    public void Parity_ComponentDeserialization_SameType()
    {
        const string json = """{"type":2,"label":"Btn","custom_id":"b1"}""";

        var stjResult = DeserializeStj<BaseComponentJsonModel>(json);
        var newtonsoftResult = DeserializeNewtonsoft<BaseComponentJsonModel>(json);

        Assert.That(stjResult.GetType(), Is.EqualTo(newtonsoftResult.GetType()));
        Assert.That(stjResult.Type, Is.EqualTo(newtonsoftResult.Type));
    }

    [Test]
    public void Parity_UnknownComponent_SameBaseType()
    {
        const string json = """{"type":200,"id":42}""";

        var stjResult = DeserializeStj<BaseComponentJsonModel>(json);
        var newtonsoftResult = DeserializeNewtonsoft<BaseComponentJsonModel>(json);

        Assert.That(stjResult.GetType(), Is.EqualTo(typeof(BaseComponentJsonModel)));
        Assert.That(newtonsoftResult.GetType(), Is.EqualTo(typeof(BaseComponentJsonModel)));
    }

    [Test]
    public void Parity_InteractionDeserialization_SameDataType()
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

        var stjResult = DeserializeStj<InteractionJsonModel>(json);
        var newtonsoftResult = DeserializeNewtonsoft<InteractionJsonModel>(json);

        Assert.That(stjResult.Data.Value!.GetType(), Is.EqualTo(newtonsoftResult.Data.Value!.GetType()));
    }

    [Test]
    public void Parity_ExtensionData_BothCapture()
    {
        const string json = """{"name":"test","extra":"value"}""";

        var stjResult = DeserializeStj<ExtensionDataTestModel>(json);
        var newtonsoftResult = DeserializeNewtonsoft<ExtensionDataTestModel>(json);

        Assert.That(JsonModel.TryGetExtensionData(stjResult, out var stjExt), Is.True);
        Assert.That(JsonModel.TryGetExtensionData(newtonsoftResult, out var newtonsoftExt), Is.True);
        Assert.That(stjExt, Does.ContainKey("extra"));
        Assert.That(newtonsoftExt, Does.ContainKey("extra"));
    }

    [Test]
    public void Parity_SkippedProperties_BothSkip()
    {
        const string json = """{"name":"test","skipped":"should_skip","other":"should_keep"}""";

        var stjResult = DeserializeStj<SkippedPropertiesTestModel>(json);
        var newtonsoftResult = DeserializeNewtonsoft<SkippedPropertiesTestModel>(json);

        var stjHasSkipped = JsonModel.TryGetExtensionData(stjResult, out var stjExt) && stjExt.ContainsKey("skipped");
        var newtonsoftHasSkipped = JsonModel.TryGetExtensionData(newtonsoftResult, out var newtonsoftExt) && newtonsoftExt.ContainsKey("skipped");

        Assert.That(stjHasSkipped, Is.False, "STJ should not have skipped property");
        Assert.That(newtonsoftHasSkipped, Is.False, "Newtonsoft should not have skipped property");

        var stjHasOther = JsonModel.TryGetExtensionData(stjResult, out stjExt) && stjExt.ContainsKey("other");
        var newtonsoftHasOther = JsonModel.TryGetExtensionData(newtonsoftResult, out newtonsoftExt) && newtonsoftExt.ContainsKey("other");

        Assert.That(stjHasOther, Is.True, "STJ should have non-skipped property");
        Assert.That(newtonsoftHasOther, Is.True, "Newtonsoft should have non-skipped property");
    }

    [Test]
    public void Parity_Nullable_NullSerialized()
    {
        var model = new NullableTestModel { NullableInt = null };

        var stjJson = SerializeStj(model);
        var newtonsoftJson = SerializeNewtonsoft(model);

        Assert.That(stjJson, Does.Contain(""""nullable_int":null""""));
        Assert.That(newtonsoftJson, Does.Contain(""""nullable_int":null""""));
    }

    [Test]
    public void Parity_Nullable_ValueSerialized()
    {
        var model = new NullableTestModel { NullableInt = 42 };

        var stjJson = SerializeStj(model);
        var newtonsoftJson = SerializeNewtonsoft(model);

        Assert.That(stjJson, Does.Contain(""""nullable_int":42""""));
        Assert.That(newtonsoftJson, Does.Contain(""""nullable_int":42""""));
    }
}
