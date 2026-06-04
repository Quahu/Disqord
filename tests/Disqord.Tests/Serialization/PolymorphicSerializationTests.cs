using Disqord.Models;

namespace Disqord.Tests.Serialization;

[TestFixture]
public class PolymorphicSerializationTests : SerializationTestBase
{
    [Test]
    public void Deserialize_ButtonComponent_ReturnsComponentJsonModel()
    {
        const string json = """{"type":2,"label":"Click me","custom_id":"btn1"}""";

        var result = Deserialize<BaseComponentJsonModel>(json);

        Assert.That(result, Is.InstanceOf<ComponentJsonModel>());
        Assert.That(result.Type, Is.EqualTo(ComponentType.Button));
        var component = (ComponentJsonModel) result;
        Assert.That(component.Label.Value, Is.EqualTo("Click me"));
        Assert.That(component.CustomId.Value, Is.EqualTo("btn1"));
    }

    [Test]
    public void Deserialize_RowComponent_ReturnsComponentJsonModel()
    {
        const string json = """
            {
                "type": 1,
                "components": [{ "type": 2, "label": "Btn", "custom_id": "b1" }]
            }
            """;

        var result = Deserialize<BaseComponentJsonModel>(json);

        Assert.That(result, Is.InstanceOf<ComponentJsonModel>());
        Assert.That(result.Type, Is.EqualTo(ComponentType.Row));
        var row = (ComponentJsonModel) result;
        Assert.That(row.Components.Value, Has.Length.EqualTo(1));
        Assert.That(row.Components.Value[0], Is.InstanceOf<ComponentJsonModel>());
        Assert.That(row.Components.Value[0].Type, Is.EqualTo(ComponentType.Button));
    }

    [Test]
    public void Deserialize_SectionComponent_ReturnsSectionComponentJsonModel()
    {
        const string json = """
            {
                "type": 9,
                "components": [{ "type": 10, "content": "hello" }],
                "accessory": { "type": 11, "media": { "url": "https://example.com/img.png" } }
            }
            """;

        var result = Deserialize<BaseComponentJsonModel>(json);

        Assert.That(result, Is.InstanceOf<SectionComponentJsonModel>());
        var section = (SectionComponentJsonModel) result;
        Assert.That(section.Components, Has.Length.EqualTo(1));
        Assert.That(section.Components[0], Is.InstanceOf<TextDisplayComponentJsonModel>());
        Assert.That(section.Accessory, Is.InstanceOf<ThumbnailComponentJsonModel>());
    }

    [Test]
    public void Deserialize_ContainerComponent_ReturnsContainerComponentJsonModel()
    {
        const string json = """
            {
                "type": 17,
                "components": [{ "type": 10, "content": "inside container" }],
                "accent_color": 16711680,
                "spoiler": false
            }
            """;

        var result = Deserialize<BaseComponentJsonModel>(json);

        Assert.That(result, Is.InstanceOf<ContainerComponentJsonModel>());
        var container = (ContainerComponentJsonModel) result;
        Assert.That(container.AccentColor.Value, Is.EqualTo(16711680));
        Assert.That(container.Spoiler.Value, Is.False);
        Assert.That(container.Components, Has.Length.EqualTo(1));
        Assert.That(container.Components[0], Is.InstanceOf<TextDisplayComponentJsonModel>());
    }

    [Test]
    public void Deserialize_TextDisplayComponent_ReturnsTextDisplayComponentJsonModel()
    {
        const string json = """{"type":10,"content":"Hello world"}""";

        var result = Deserialize<BaseComponentJsonModel>(json);

        Assert.That(result, Is.InstanceOf<TextDisplayComponentJsonModel>());
    }

    [Test]
    public void Deserialize_SeparatorComponent_ReturnsSeparatorComponentJsonModel()
    {
        const string json = """{"type":14}""";

        var result = Deserialize<BaseComponentJsonModel>(json);

        Assert.That(result, Is.InstanceOf<SeparatorComponentJsonModel>());
    }

    [Test]
    public void Deserialize_UnknownComponentType_ReturnsBaseComponentJsonModel_NoStackOverflow()
    {
        const string json = """{"type":200,"id":42}""";

        var result = Deserialize<BaseComponentJsonModel>(json);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.GetType(), Is.EqualTo(typeof(BaseComponentJsonModel)));
        Assert.That((int) result.Type, Is.EqualTo(200));
        Assert.That(result.Id.Value, Is.EqualTo(42));
    }

    [Test]
    public void Deserialize_ComponentArray_PolymorphicElements()
    {
        const string json = """[{"type":2,"label":"Btn"},{"type":10,"content":"Hi"},{"type":14}]""";

        var result = Deserialize<BaseComponentJsonModel[]>(json);

        Assert.That(result, Has.Length.EqualTo(3));
        Assert.That(result[0], Is.InstanceOf<ComponentJsonModel>());
        Assert.That(result[1], Is.InstanceOf<TextDisplayComponentJsonModel>());
        Assert.That(result[2], Is.InstanceOf<SeparatorComponentJsonModel>());
    }

    [Test]
    public void Deserialize_NestedContainerWithUnknownChild_NoStackOverflow()
    {
        const string json = """
            {
                "type": 17,
                "components": [
                    { "type": 10, "content": "known" },
                    { "type": 200 }
                ]
            }
            """;

        var result = Deserialize<BaseComponentJsonModel>(json);

        Assert.That(result, Is.InstanceOf<ContainerComponentJsonModel>());
        var container = (ContainerComponentJsonModel) result;
        Assert.That(container.Components, Has.Length.EqualTo(2));
        Assert.That(container.Components[0], Is.InstanceOf<TextDisplayComponentJsonModel>());
        Assert.That(container.Components[1].GetType(), Is.EqualTo(typeof(BaseComponentJsonModel)));
    }

    [Test]
    public void Deserialize_FileUploadComponent_ReturnsFileUploadComponentJsonModel()
    {
        const string json = """{"type":19,"custom_id":"upload1"}""";

        var result = Deserialize<BaseComponentJsonModel>(json);

        Assert.That(result, Is.InstanceOf<FileUploadComponentJsonModel>());
        Assert.That(((FileUploadComponentJsonModel) result).CustomId, Is.EqualTo("upload1"));
    }

    [Test]
    public void Deserialize_RadioGroupComponent_ReturnsRadioGroupComponentJsonModel()
    {
        const string json = """{"type":21,"custom_id":"radio1"}""";

        var result = Deserialize<BaseComponentJsonModel>(json);

        Assert.That(result, Is.InstanceOf<RadioGroupComponentJsonModel>());
        Assert.That(((RadioGroupComponentJsonModel) result).CustomId, Is.EqualTo("radio1"));
    }

    [Test]
    public void Deserialize_CheckboxGroupComponent_ReturnsCheckboxGroupComponentJsonModel()
    {
        const string json = """{"type":22,"custom_id":"cbgroup1"}""";

        var result = Deserialize<BaseComponentJsonModel>(json);

        Assert.That(result, Is.InstanceOf<CheckboxGroupComponentJsonModel>());
        Assert.That(((CheckboxGroupComponentJsonModel) result).CustomId, Is.EqualTo("cbgroup1"));
    }

    [Test]
    public void Deserialize_CheckboxComponent_ReturnsCheckboxComponentJsonModel()
    {
        const string json = """{"type":23,"custom_id":"cb1"}""";

        var result = Deserialize<BaseComponentJsonModel>(json);

        Assert.That(result, Is.InstanceOf<CheckboxComponentJsonModel>());
        Assert.That(((CheckboxComponentJsonModel) result).CustomId, Is.EqualTo("cb1"));
    }

    [Test]
    public void Deserialize_LabelComponent_ReturnsLabelComponentJsonModel()
    {
        const string json = """{"type":18,"content":"my label"}""";

        var result = Deserialize<BaseComponentJsonModel>(json);

        Assert.That(result, Is.InstanceOf<LabelComponentJsonModel>());
    }

    [Test]
    public void Serialize_ComponentJsonModel_RoundTrip()
    {
        const string json = """{"type":2,"label":"Click me","custom_id":"btn1"}""";

        var deserialized = Deserialize<BaseComponentJsonModel>(json);
        var serialized = Serialize(deserialized);
        var reDeserialized = Deserialize<BaseComponentJsonModel>(serialized);

        Assert.That(reDeserialized, Is.InstanceOf<ComponentJsonModel>());
        Assert.That(reDeserialized.Type, Is.EqualTo(ComponentType.Button));
        Assert.That(((ComponentJsonModel) reDeserialized).Label.Value, Is.EqualTo("Click me"));
    }

    [Test]
    public void Serialize_UnknownComponentType_NoStackOverflow()
    {
        const string json = """{"type":200,"id":42}""";

        var deserialized = Deserialize<BaseComponentJsonModel>(json);

        Assert.DoesNotThrow(() => Serialize(deserialized));
    }

    [Test]
    public void Serialize_ContainerWithNestedComponents_PreservesPolymorphism()
    {
        const string json = """
            {
                "type": 17,
                "components": [
                    { "type": 2, "label": "Btn", "custom_id": "b1" },
                    { "type": 10, "content": "Hi" }
                ]
            }
            """;

        var deserialized = Deserialize<BaseComponentJsonModel>(json);
        var serialized = Serialize(deserialized);
        var reDeserialized = Deserialize<BaseComponentJsonModel>(serialized);

        Assert.That(reDeserialized, Is.InstanceOf<ContainerComponentJsonModel>());
        var container = (ContainerComponentJsonModel) reDeserialized;
        Assert.That(container.Components, Has.Length.EqualTo(2));
        Assert.That(container.Components[0], Is.InstanceOf<ComponentJsonModel>());
        Assert.That(container.Components[1], Is.InstanceOf<TextDisplayComponentJsonModel>());
    }

    [Test]
    public void Deserialize_ModalRowComponent_ReturnsModalRowComponentJsonModel()
    {
        const string json = """
            {
                "type": 1,
                "components": [{ "type": 4, "custom_id": "input1", "value": "hello" }]
            }
            """;

        var result = Deserialize<ModalBaseComponentJsonModel>(json);

        Assert.That(result, Is.InstanceOf<ModalRowComponentJsonModel>());
        var row = (ModalRowComponentJsonModel) result;
        Assert.That(row.Components, Has.Length.EqualTo(1));
        Assert.That(row.Components[0], Is.InstanceOf<ModalTextInputComponentJsonModel>());
    }

    [Test]
    public void Deserialize_ModalTextInputComponent_ReturnsModalTextInputComponentJsonModel()
    {
        const string json = """{"type":4,"custom_id":"input1","value":"hello"}""";

        var result = Deserialize<ModalBaseComponentJsonModel>(json);

        Assert.That(result, Is.InstanceOf<ModalTextInputComponentJsonModel>());
        var textInput = (ModalTextInputComponentJsonModel) result;
        Assert.That(textInput.CustomId, Is.EqualTo("input1"));
        Assert.That(textInput.Value, Is.EqualTo("hello"));
    }

    [Test]
    public void Deserialize_ModalSelectionComponent_ReturnsModalSelectionComponentJsonModel()
    {
        const string json = """{"type":3,"custom_id":"sel1","values":["a","b"]}""";

        var result = Deserialize<ModalBaseComponentJsonModel>(json);

        Assert.That(result, Is.InstanceOf<ModalSelectionComponentJsonModel>());
        var selection = (ModalSelectionComponentJsonModel) result;
        Assert.That(selection.CustomId, Is.EqualTo("sel1"));
        Assert.That(selection.Values, Is.EqualTo(new[] { "a", "b" }));
    }

    [Test]
    public void Deserialize_UnknownModalComponentType_ReturnsModalBaseComponentJsonModel_NoStackOverflow()
    {
        const string json = """{"type":200}""";

        var result = Deserialize<ModalBaseComponentJsonModel>(json);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.GetType(), Is.EqualTo(typeof(ModalBaseComponentJsonModel)));
        Assert.That((int) result.Type, Is.EqualTo(200));
    }

    [Test]
    public void Serialize_UnknownModalComponentType_NoStackOverflow()
    {
        const string json = """{"type":200}""";

        var deserialized = Deserialize<ModalBaseComponentJsonModel>(json);

        Assert.DoesNotThrow(() => Serialize(deserialized));
    }

    [Test]
    public void Deserialize_ModalTextDisplayComponent_ReturnsModalTextDisplayComponentJsonModel()
    {
        const string json = """{"type":10}""";

        var result = Deserialize<ModalBaseComponentJsonModel>(json);

        Assert.That(result, Is.InstanceOf<ModalTextDisplayComponentJsonModel>());
    }

    [Test]
    public void Deserialize_ModalLabelComponent_ReturnsModalLabelComponentJsonModel()
    {
        const string json = """{"type":18}""";

        var result = Deserialize<ModalBaseComponentJsonModel>(json);

        Assert.That(result, Is.InstanceOf<ModalLabelComponentJsonModel>());
    }

    [Test]
    public void Deserialize_ModalFileUploadComponent_ReturnsModalFileUploadComponentJsonModel()
    {
        const string json = """{"type":19,"custom_id":"upload1","values":["file1.png"]}""";

        var result = Deserialize<ModalBaseComponentJsonModel>(json);

        Assert.That(result, Is.InstanceOf<ModalFileUploadComponentJsonModel>());
        var upload = (ModalFileUploadComponentJsonModel) result;
        Assert.That(upload.CustomId, Is.EqualTo("upload1"));
        Assert.That(upload.Values, Is.EqualTo(new[] { "file1.png" }));
    }

    [Test]
    public void Deserialize_ModalRadioGroupComponent_ReturnsModalRadioGroupComponentJsonModel()
    {
        const string json = """{"type":21,"custom_id":"radio1","value":"option1"}""";

        var result = Deserialize<ModalBaseComponentJsonModel>(json);

        Assert.That(result, Is.InstanceOf<ModalRadioGroupComponentJsonModel>());
        var radio = (ModalRadioGroupComponentJsonModel) result;
        Assert.That(radio.CustomId, Is.EqualTo("radio1"));
        Assert.That(radio.Value.Value, Is.EqualTo("option1"));
    }

    [Test]
    public void Deserialize_ModalCheckboxGroupComponent_ReturnsModalCheckboxGroupComponentJsonModel()
    {
        const string json = """{"type":22,"custom_id":"cbgroup1","values":["a","b"]}""";

        var result = Deserialize<ModalBaseComponentJsonModel>(json);

        Assert.That(result, Is.InstanceOf<ModalCheckboxGroupComponentJsonModel>());
        var cbGroup = (ModalCheckboxGroupComponentJsonModel) result;
        Assert.That(cbGroup.CustomId, Is.EqualTo("cbgroup1"));
        Assert.That(cbGroup.Values, Is.EqualTo(new[] { "a", "b" }));
    }

    [Test]
    public void Deserialize_ModalCheckboxComponent_ReturnsModalCheckboxComponentJsonModel()
    {
        const string json = """{"type":23,"custom_id":"cb1","value":true}""";

        var result = Deserialize<ModalBaseComponentJsonModel>(json);

        Assert.That(result, Is.InstanceOf<ModalCheckboxComponentJsonModel>());
        var cb = (ModalCheckboxComponentJsonModel) result;
        Assert.That(cb.CustomId, Is.EqualTo("cb1"));
        Assert.That(cb.Value, Is.True);
    }

    [Test]
    public void Deserialize_ModalUserSelectionComponent_ReturnsModalSelectionComponentJsonModel()
    {
        const string json = """{"type":5,"custom_id":"sel1","values":["123"]}""";

        var result = Deserialize<ModalBaseComponentJsonModel>(json);

        Assert.That(result, Is.InstanceOf<ModalSelectionComponentJsonModel>());
    }

    [Test]
    public void Deserialize_ModalChannelSelectionComponent_ReturnsModalSelectionComponentJsonModel()
    {
        const string json = """{"type":8,"custom_id":"sel1","values":["456"]}""";

        var result = Deserialize<ModalBaseComponentJsonModel>(json);

        Assert.That(result, Is.InstanceOf<ModalSelectionComponentJsonModel>());
    }

    [Test]
    public void Deserialize_PingInteraction_NullData()
    {
        const string json = """
            {
                "id": "123",
                "application_id": "456",
                "type": 1,
                "token": "tok",
                "version": 1,
                "entitlements": []
            }
            """;

        var result = Deserialize<InteractionJsonModel>(json);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Type, Is.EqualTo(InteractionType.Ping));
        Assert.That(result.Data.HasValue, Is.False.Or.EqualTo(true).And.Property("Value").Null);
    }

    [Test]
    public void Deserialize_ApplicationCommandInteraction_CorrectDataType()
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

        var result = Deserialize<InteractionJsonModel>(json);

        Assert.That(result.Type, Is.EqualTo(InteractionType.ApplicationCommand));
        Assert.That(result.Data.Value, Is.InstanceOf<ApplicationCommandInteractionDataJsonModel>());
        var data = (ApplicationCommandInteractionDataJsonModel) result.Data.Value!;
        Assert.That(data.Name, Is.EqualTo("test"));
    }

    [Test]
    public void Deserialize_MessageComponentInteraction_CorrectDataType()
    {
        const string json = """
            {
                "id": "123",
                "application_id": "456",
                "type": 3,
                "data": { "custom_id": "btn1", "component_type": 2 },
                "token": "tok",
                "version": 1,
                "entitlements": []
            }
            """;

        var result = Deserialize<InteractionJsonModel>(json);

        Assert.That(result.Type, Is.EqualTo(InteractionType.MessageComponent));
        Assert.That(result.Data.Value, Is.InstanceOf<MessageComponentInteractionDataJsonModel>());
    }

    [Test]
    public void Deserialize_UnknownInteractionType_ReturnsBaseDataType_NoStackOverflow()
    {
        const string json = """
            {
                "id": "123",
                "application_id": "456",
                "type": 999,
                "data": { "foo": "bar" },
                "token": "tok",
                "version": 1,
                "entitlements": []
            }
            """;

        var result = Deserialize<InteractionJsonModel>(json);

        Assert.That(result, Is.Not.Null);
        Assert.That((int) result.Type, Is.EqualTo(999));
        Assert.That(result.Data.Value, Is.Not.Null);
        Assert.That(result.Data.Value!.GetType(), Is.EqualTo(typeof(InteractionDataJsonModel)));
    }

    [Test]
    public void Serialize_Interaction_NoStackOverflow()
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

        var deserialized = Deserialize<InteractionJsonModel>(json);

        Assert.DoesNotThrow(() => Serialize(deserialized));
    }

    [Test]
    public void Deserialize_ApplicationCommandMetadata_ReturnsCorrectType()
    {
        const string json = """
            {
                "id": "111",
                "type": 2,
                "user": { "id": "222", "username": "test", "discriminator": "0000" },
                "authorizing_integration_owners": {}
            }
            """;

        var result = Deserialize<MessageInteractionMetadataJsonModel>(json);

        Assert.That(result, Is.InstanceOf<MessageApplicationCommandInteractionMetadataJsonModel>());
        Assert.That(result.Type, Is.EqualTo(InteractionType.ApplicationCommand));
    }

    [Test]
    public void Deserialize_MessageComponentMetadata_ReturnsCorrectType()
    {
        const string json = """
            {
                "id": "111",
                "type": 3,
                "user": { "id": "222", "username": "test", "discriminator": "0000" },
                "authorizing_integration_owners": {},
                "interacted_message_id": "333"
            }
            """;

        var result = Deserialize<MessageInteractionMetadataJsonModel>(json);

        Assert.That(result, Is.InstanceOf<MessageComponentInteractionMetadataJsonModel>());
        var meta = (MessageComponentInteractionMetadataJsonModel) result;
        Assert.That(meta.InteractedMessageId.ToString(), Is.EqualTo("333"));
    }

    [Test]
    public void Deserialize_ModalSubmitMetadata_WithNestedPolymorphism()
    {
        const string json = """
            {
                "id": "111",
                "type": 5,
                "user": { "id": "222", "username": "test", "discriminator": "0000" },
                "authorizing_integration_owners": {},
                "triggering_interaction_metadata": {
                    "id": "444",
                    "type": 3,
                    "user": { "id": "222", "username": "test", "discriminator": "0000" },
                    "authorizing_integration_owners": {},
                    "interacted_message_id": "555"
                }
            }
            """;

        var result = Deserialize<MessageInteractionMetadataJsonModel>(json);

        Assert.That(result, Is.InstanceOf<MessageModalSubmitInteractionMetadataJsonModel>());
        var meta = (MessageModalSubmitInteractionMetadataJsonModel) result;
        Assert.That(meta.TriggeringInteractionMetadata, Is.InstanceOf<MessageComponentInteractionMetadataJsonModel>());
    }

    [Test]
    public void Deserialize_UnknownInteractionMetadataType_ReturnsBase_NoStackOverflow()
    {
        const string json = """
            {
                "id": "111",
                "type": 999,
                "user": { "id": "222", "username": "test", "discriminator": "0000" },
                "authorizing_integration_owners": {}
            }
            """;

        var result = Deserialize<MessageInteractionMetadataJsonModel>(json);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.GetType(), Is.EqualTo(typeof(MessageInteractionMetadataJsonModel)));
        Assert.That((int) result.Type, Is.EqualTo(999));
    }

    [Test]
    public void Serialize_MessageInteractionMetadata_NoStackOverflow()
    {
        const string json = """
            {
                "id": "111",
                "type": 2,
                "user": { "id": "222", "username": "test", "discriminator": "0000" },
                "authorizing_integration_owners": {}
            }
            """;

        var deserialized = Deserialize<MessageInteractionMetadataJsonModel>(json);

        Assert.DoesNotThrow(() => Serialize(deserialized));
    }

    [Test]
    public void Serialize_UnknownInteractionMetadataType_NoStackOverflow()
    {
        const string json = """
            {
                "id": "111",
                "type": 999,
                "user": { "id": "222", "username": "test", "discriminator": "0000" },
                "authorizing_integration_owners": {}
            }
            """;

        var deserialized = Deserialize<MessageInteractionMetadataJsonModel>(json);

        Assert.DoesNotThrow(() => Serialize(deserialized));
    }

    [Test]
    public void Deserialize_UnresolvedMediaItem_ReturnsUnfurledMediaItemJsonModel()
    {
        const string json = """{"url":"https://example.com/img.png"}""";

        var result = Deserialize<UnfurledMediaItemJsonModel>(json);

        Assert.That(result.GetType(), Is.EqualTo(typeof(UnfurledMediaItemJsonModel)));
        Assert.That(result.Url, Is.EqualTo("https://example.com/img.png"));
    }

    [Test]
    public void Deserialize_ResolvedMediaItem_ReturnsResolvedUnfurledMediaItemJsonModel()
    {
        const string json = """
            {
                "url": "https://example.com/img.png",
                "proxy_url": "https://proxy.example.com/img.png",
                "width": 100,
                "height": 200,
                "loading_state": 2
            }
            """;

        var result = Deserialize<UnfurledMediaItemJsonModel>(json);

        Assert.That(result, Is.InstanceOf<ResolvedUnfurledMediaItemJsonModel>());
        var resolved = (ResolvedUnfurledMediaItemJsonModel) result;
        Assert.That(resolved.ProxyUrl, Is.EqualTo("https://proxy.example.com/img.png"));
        Assert.That(resolved.Width, Is.EqualTo(100));
        Assert.That(resolved.Height, Is.EqualTo(200));
    }

    [Test]
    public void Serialize_UnresolvedMediaItem_NoStackOverflow()
    {
        const string json = """{"url":"https://example.com/img.png"}""";

        var deserialized = Deserialize<UnfurledMediaItemJsonModel>(json);

        Assert.DoesNotThrow(() => Serialize(deserialized));
    }

    [Test]
    public void Serialize_ResolvedMediaItem_NoStackOverflow()
    {
        const string json = """{"url":"https://example.com/img.png","loading_state":2}""";

        var deserialized = Deserialize<UnfurledMediaItemJsonModel>(json);

        Assert.DoesNotThrow(() => Serialize(deserialized));
    }

    [Test]
    public void Deserialize_UnknownProperties_PreservedInExtensionData()
    {
        const string json = """
            {
                "type": 2,
                "label": "Btn",
                "custom_id": "b1",
                "unknown_prop": "test_value",
                "another_unknown": 42
            }
            """;

        var result = Deserialize<BaseComponentJsonModel>(json);

        Assert.That(result, Is.InstanceOf<ComponentJsonModel>());
        Assert.That(result.ExtensionData, Does.ContainKey("unknown_prop"));
        Assert.That(result.ExtensionData, Does.ContainKey("another_unknown"));
    }

    [Test]
    public void Deserialize_UnknownComponentType_PreservesExtensionData()
    {
        const string json = """{"type":200,"id":1,"some_future_field":"value"}""";

        var result = Deserialize<BaseComponentJsonModel>(json);

        Assert.That(result.ExtensionData, Does.ContainKey("some_future_field"));
    }

    [Test]
    public void Deserialize_DeeplyNestedComponents_NoStackOverflow()
    {
        const string json = """
            {
                "type": 17,
                "components": [
                    {
                        "type": 1,
                        "components": [
                            { "type": 2, "label": "Btn1", "custom_id": "b1" },
                            { "type": 2, "label": "Btn2", "custom_id": "b2" }
                        ]
                    },
                    {
                        "type": 9,
                        "components": [{ "type": 10, "content": "Section text" }],
                        "accessory": { "type": 11, "media": { "url": "https://example.com/img.png" } }
                    },
                    { "type": 200 }
                ]
            }
            """;

        var result = Deserialize<BaseComponentJsonModel>(json);

        Assert.That(result, Is.InstanceOf<ContainerComponentJsonModel>());
        var container = (ContainerComponentJsonModel) result;
        Assert.That(container.Components, Has.Length.EqualTo(3));
        Assert.That(container.Components[0], Is.InstanceOf<ComponentJsonModel>());
        var row = (ComponentJsonModel) container.Components[0];
        Assert.That(row.Components.Value, Has.Length.EqualTo(2));
        Assert.That(row.Components.Value[0], Is.InstanceOf<ComponentJsonModel>());
        Assert.That(container.Components[1], Is.InstanceOf<SectionComponentJsonModel>());
        Assert.That(container.Components[2].GetType(), Is.EqualTo(typeof(BaseComponentJsonModel)));
    }

    [Test]
    public void Serialize_DeeplyNestedComponents_RoundTrip()
    {
        const string json = """
            {
                "type": 17,
                "components": [
                    {
                        "type": 1,
                        "components": [{ "type": 2, "label": "Btn", "custom_id": "b1" }]
                    },
                    { "type": 200 }
                ]
            }
            """;

        var deserialized = Deserialize<BaseComponentJsonModel>(json);
        var serialized = Serialize(deserialized);
        var reDeserialized = Deserialize<BaseComponentJsonModel>(serialized);

        Assert.That(reDeserialized, Is.InstanceOf<ContainerComponentJsonModel>());
        var container = (ContainerComponentJsonModel) reDeserialized;
        Assert.That(container.Components, Has.Length.EqualTo(2));
        var row = (ComponentJsonModel) container.Components[0];
        Assert.That(row.Components.Value[0], Is.InstanceOf<ComponentJsonModel>());
        Assert.That(container.Components[1].GetType(), Is.EqualTo(typeof(BaseComponentJsonModel)));
    }
}
