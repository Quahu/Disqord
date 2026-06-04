using Disqord.Serialization.Json;
using Disqord.Tests.Models;

namespace Disqord.Tests.Serialization;

[TestFixture]
public class EnumSerializationTests : SerializationTestBase
{
    [Test]
    public void Deserialize_NumericEnum_FromNumber()
    {
        const string json = """{"component_type":2,"team_role":"admin"}""";

        var result = Deserialize<EnumTestModel>(json);

        Assert.That(result.NumericEnum, Is.EqualTo(ComponentType.Button));
    }

    [Test]
    public void Deserialize_NumericEnum_FromString()
    {
        const string json = """{"component_type":"2","team_role":"admin"}""";

        var result = Deserialize<EnumTestModel>(json);

        Assert.That(result.NumericEnum, Is.EqualTo(ComponentType.Button));
    }

    [Test]
    public void Serialize_NumericEnum_WritesNumber()
    {
        var model = new EnumTestModel
        {
            NumericEnum = ComponentType.Button,
            StringEnum = TeamMemberRole.Administrator
        };

        var json = Serialize(model);

        Assert.That(json, Does.Contain(""""component_type":2""""));
    }

    [Test]
    public void Deserialize_StringEnum_FromString()
    {
        const string json = """{"component_type":1,"team_role":"admin"}""";

        var result = Deserialize<EnumTestModel>(json);

        Assert.That(result.StringEnum, Is.EqualTo(TeamMemberRole.Administrator));
    }

    [Test]
    public void Deserialize_StringEnum_FromEnumMemberValue()
    {
        const string json = """{"component_type":1,"team_role":"read_only"}""";

        var result = Deserialize<EnumTestModel>(json);

        Assert.That(result.StringEnum, Is.EqualTo(TeamMemberRole.ReadOnly));
    }

    [Test]
    public void Serialize_StringEnum_WritesEnumMemberValue()
    {
        var model = new EnumTestModel
        {
            NumericEnum = ComponentType.Row,
            StringEnum = TeamMemberRole.ReadOnly
        };

        var json = Serialize(model);

        Assert.That(json, Does.Contain(""""team_role":"read_only""""));
    }

    [Test]
    public void Serialize_StringEnum_WritesEnumMemberValue_Admin()
    {
        var model = new EnumTestModel
        {
            NumericEnum = ComponentType.Row,
            StringEnum = TeamMemberRole.Administrator
        };

        var json = Serialize(model);

        Assert.That(json, Does.Contain(""""team_role":"admin""""));
    }

    [Test]
    public void Deserialize_UnknownNumericEnum_Preserved()
    {
        const string json = """{"component_type":200,"team_role":"admin"}""";

        var result = Deserialize<EnumTestModel>(json);

        Assert.That((int) result.NumericEnum, Is.EqualTo(200));
    }

    [Test]
    public void Deserialize_OptionalEnum_Present()
    {
        const string json = """{"component_type":1,"team_role":"admin","optional_type":3}""";

        var result = Deserialize<EnumTestModel>(json);

        Assert.That(result.OptionalEnum.HasValue, Is.True);
        Assert.That(result.OptionalEnum.Value, Is.EqualTo(ComponentType.StringSelection));
    }

    [Test]
    public void Deserialize_OptionalEnum_Absent()
    {
        const string json = """{"component_type":1,"team_role":"admin"}""";

        var result = Deserialize<EnumTestModel>(json);

        Assert.That(result.OptionalEnum.HasValue, Is.False);
    }

    [Test]
    public void Serialize_OptionalEnum_Present_Included()
    {
        var model = new EnumTestModel
        {
            NumericEnum = ComponentType.Row,
            StringEnum = TeamMemberRole.Administrator,
            OptionalEnum = ComponentType.Button
        };

        var json = Serialize(model);

        Assert.That(json, Does.Contain(""""optional_type":2""""));
    }

    [Test]
    public void Serialize_OptionalEnum_Absent_Omitted()
    {
        var model = new EnumTestModel
        {
            NumericEnum = ComponentType.Row,
            StringEnum = TeamMemberRole.Administrator
        };

        var json = Serialize(model);

        Assert.That(json, Does.Not.Contain("optional_type"));
    }

    [Test]
    public void RoundTrip_Enum_Preserved()
    {
        var original = new EnumTestModel
        {
            NumericEnum = ComponentType.Container,
            StringEnum = TeamMemberRole.Developer,
            OptionalEnum = ComponentType.TextInput
        };

        var json = Serialize(original);
        var result = Deserialize<EnumTestModel>(json);

        Assert.That(result.NumericEnum, Is.EqualTo(ComponentType.Container));
        Assert.That(result.StringEnum, Is.EqualTo(TeamMemberRole.Developer));
        Assert.That(result.OptionalEnum.Value, Is.EqualTo(ComponentType.TextInput));
    }
}
