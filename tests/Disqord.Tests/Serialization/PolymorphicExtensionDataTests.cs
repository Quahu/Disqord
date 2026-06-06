using Disqord.Models;
using Disqord.Serialization.Json;

namespace Disqord.Tests.Serialization;

[TestFixture]
public class PolymorphicExtensionDataTests : SerializationTestBase
{
    [Test]
    public void Deserialize_BaseComponent_ExtensionDataCaptured()
    {
        const string json = """{"type":200,"id":1,"unknown_field":"value"}""";

        var result = Deserialize<BaseComponentJsonModel>(json);

        Assert.That(result.GetType(), Is.EqualTo(typeof(BaseComponentJsonModel)));
        Assert.That(JsonModel.TryGetExtensionData(result, out var ext), Is.True);
        Assert.That(ext, Does.ContainKey("unknown_field"));
    }

    [Test]
    public void Deserialize_DerivedComponent_ExtensionDataCaptured()
    {
        const string json = """{"type":2,"label":"Btn","custom_id":"b1","unknown_field":"value"}""";

        var result = Deserialize<BaseComponentJsonModel>(json);

        Assert.That(result, Is.InstanceOf<ComponentJsonModel>());
        Assert.That(JsonModel.TryGetExtensionData(result, out var ext), Is.True);
        Assert.That(ext, Does.ContainKey("unknown_field"));
    }

    [Test]
    public void Deserialize_DerivedComponent_KnownFieldsNotInExtensionData()
    {
        const string json = """{"type":2,"label":"Btn","custom_id":"b1","unknown_field":"value"}""";

        var result = Deserialize<BaseComponentJsonModel>(json);

        Assert.That(JsonModel.TryGetExtensionData(result, out var ext), Is.True);
        Assert.That(ext, Does.Not.ContainKey("type"));
        Assert.That(ext, Does.Not.ContainKey("label"));
        Assert.That(ext, Does.Not.ContainKey("custom_id"));
    }

    [Test]
    public void Deserialize_NestedComponents_ExtensionDataOnParentAndChild()
    {
        const string json = """
            {
                "type": 1,
                "components": [{ "type": 2, "label": "Btn", "custom_id": "b1", "child_extra": true }],
                "parent_extra": "hello"
            }
            """;

        var result = Deserialize<BaseComponentJsonModel>(json);

        Assert.That(result, Is.InstanceOf<ComponentJsonModel>());
        var row = (ComponentJsonModel) result;

        Assert.That(JsonModel.TryGetExtensionData(row, out var parentExt), Is.True);
        Assert.That(parentExt, Does.ContainKey("parent_extra"));

        var child = row.Components.Value[0];
        Assert.That(child, Is.InstanceOf<ComponentJsonModel>());
        Assert.That(JsonModel.TryGetExtensionData(child, out var childExt), Is.True);
        Assert.That(childExt, Does.ContainKey("child_extra"));
    }

    [Test]
    public void Deserialize_Interaction_ExtensionDataCaptured()
    {
        const string json = """
            {
                "id": "123",
                "application_id": "456",
                "type": 2,
                "data": { "id": "789", "name": "test", "type": 1 },
                "token": "tok",
                "version": 1,
                "entitlements": [],
                "interaction_extra": "surprise"
            }
            """;

        var result = Deserialize<InteractionJsonModel>(json);

        Assert.That(JsonModel.TryGetExtensionData(result, out var ext), Is.True);
        Assert.That(ext, Does.ContainKey("interaction_extra"));
    }

    [Test]
    public void Deserialize_Interaction_SkippedPropertyNotInExtensionData()
    {
        const string json = """
            {
                "id": "123",
                "application_id": "456",
                "type": 2,
                "data": { "id": "789", "name": "test", "type": 1 },
                "token": "tok",
                "version": 1,
                "entitlements": [],
                "interaction_extra": "surprise"
            }
            """;

        var result = Deserialize<InteractionJsonModel>(json);

        Assert.That(JsonModel.TryGetExtensionData(result, out var ext), Is.True);
        Assert.That(ext, Does.Not.ContainKey("data"));
        Assert.That(ext, Does.ContainKey("interaction_extra"));
    }

    [Test]
    public void Deserialize_MessageInteractionMetadata_ExtensionDataCaptured()
    {
        const string json = """
            {
                "id": "111",
                "type": 2,
                "user": { "id": "222", "username": "test", "discriminator": "0000" },
                "authorizing_integration_owners": {},
                "metadata_extra": 42
            }
            """;

        var result = Deserialize<MessageInteractionMetadataJsonModel>(json);

        Assert.That(result, Is.InstanceOf<MessageApplicationCommandInteractionMetadataJsonModel>());
        Assert.That(JsonModel.TryGetExtensionData(result, out var ext), Is.True);
        Assert.That(ext, Does.ContainKey("metadata_extra"));
    }

    [Test]
    public void Deserialize_UnknownMetadataType_BaseExtensionDataCaptured()
    {
        const string json = """
            {
                "id": "111",
                "type": 999,
                "user": { "id": "222", "username": "test", "discriminator": "0000" },
                "authorizing_integration_owners": {},
                "extra": "value"
            }
            """;

        var result = Deserialize<MessageInteractionMetadataJsonModel>(json);

        Assert.That(result.GetType(), Is.EqualTo(typeof(MessageInteractionMetadataJsonModel)));
        Assert.That(JsonModel.TryGetExtensionData(result, out var ext), Is.True);
        Assert.That(ext, Does.ContainKey("extra"));
    }

    [Test]
    public void Deserialize_ModalComponent_ExtensionDataCaptured()
    {
        const string json = """{"type":4,"custom_id":"input1","value":"hello","modal_extra":"data"}""";

        var result = Deserialize<ModalBaseComponentJsonModel>(json);

        Assert.That(result, Is.InstanceOf<ModalTextInputComponentJsonModel>());
        Assert.That(JsonModel.TryGetExtensionData(result, out var ext), Is.True);
        Assert.That(ext, Does.ContainKey("modal_extra"));
    }

    [Test]
    public void RoundTrip_ComponentWithExtensionData_Preserved()
    {
        const string json = """{"type":2,"label":"Btn","custom_id":"b1","extra_field":"preserved"}""";

        var deserialized = Deserialize<BaseComponentJsonModel>(json);
        var serialized = Serialize(deserialized);
        var reDeserialized = Deserialize<BaseComponentJsonModel>(serialized);

        Assert.That(reDeserialized, Is.InstanceOf<ComponentJsonModel>());
        Assert.That(JsonModel.TryGetExtensionData(reDeserialized, out var ext), Is.True);
        Assert.That(ext, Does.ContainKey("extra_field"));
        Assert.That(JsonModel.TryGetExtensionDatum<string>(reDeserialized, "extra_field", out var value), Is.True);
        Assert.That(value, Is.EqualTo("preserved"));
    }

    [Test]
    public void RoundTrip_UnknownComponentWithExtensionData_Preserved()
    {
        const string json = """{"type":200,"id":1,"extra":"kept"}""";

        var deserialized = Deserialize<BaseComponentJsonModel>(json);
        var serialized = Serialize(deserialized);
        var reDeserialized = Deserialize<BaseComponentJsonModel>(serialized);

        Assert.That(reDeserialized.GetType(), Is.EqualTo(typeof(BaseComponentJsonModel)));
        Assert.That(JsonModel.TryGetExtensionData(reDeserialized, out var ext), Is.True);
        Assert.That(ext, Does.ContainKey("extra"));
    }
}
