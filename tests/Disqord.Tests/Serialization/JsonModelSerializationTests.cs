using Disqord.Tests.Models;

namespace Disqord.Tests.Serialization;

[TestFixture]
public class JsonModelSerializationTests : SerializationTestBase
{
    [Test]
    public void Serialize_UsesJsonPropertyName()
    {
        var model = new SimpleTestModel { Name = "test", Value = 42 };

        var json = Serialize(model);

        Assert.That(json, Does.Contain(""""name":"test""""));
        Assert.That(json, Does.Contain(""""value":42""""));
        Assert.That(json, Does.Not.Contain("Name"));
        Assert.That(json, Does.Not.Contain("Value"));
    }

    [Test]
    public void Deserialize_UsesJsonPropertyName()
    {
        const string json = """{"name":"test","value":42}""";

        var result = Deserialize<SimpleTestModel>(json);

        Assert.That(result.Name, Is.EqualTo("test"));
        Assert.That(result.Value, Is.EqualTo(42));
    }

    [Test]
    public void Serialize_JsonIgnoreField_Excluded()
    {
        var model = new JsonIgnoreTestModel
        {
            Included = "yes",
            Ignored = "no",
            NoAttribute = "also_no"
        };

        var json = Serialize(model);

        Assert.That(json, Does.Contain(""""included":"yes""""));
        Assert.That(json, Does.Not.Contain("Ignored"));
        Assert.That(json, Does.Not.Contain("NoAttribute"));
        Assert.That(json, Does.Not.Contain("no_attribute"));
    }

    [Test]
    public void Serialize_NestedModel()
    {
        var model = new NestedTestModel
        {
            Child = new SimpleTestModel { Name = "child", Value = 1 },
            Children =
            [
                new SimpleTestModel { Name = "a", Value = 2 },
                new SimpleTestModel { Name = "b", Value = 3 }
            ]
        };

        var json = Serialize(model);

        Assert.That(json, Does.Contain(""""child":{"name":"child","value":1}""""));
        Assert.That(json, Does.Contain(""""children":[""""));
    }

    [Test]
    public void Deserialize_NestedModel()
    {
        const string json = """
            {
                "child": { "name": "child", "value": 1 },
                "children": [{ "name": "a", "value": 2 }]
            }
            """;

        var result = Deserialize<NestedTestModel>(json);

        Assert.That(result.Child.Name, Is.EqualTo("child"));
        Assert.That(result.Child.Value, Is.EqualTo(1));
        Assert.That(result.Children, Has.Length.EqualTo(1));
        Assert.That(result.Children[0].Name, Is.EqualTo("a"));
    }

    [Test]
    public void Deserialize_OptionalNestedModel_Present()
    {
        const string json = """
            {
                "child": { "name": "c", "value": 0 },
                "optional_child": { "name": "opt", "value": 99 },
                "children": []
            }
            """;

        var result = Deserialize<NestedTestModel>(json);

        Assert.That(result.OptionalChild.HasValue, Is.True);
        Assert.That(result.OptionalChild.Value.Name, Is.EqualTo("opt"));
    }

    [Test]
    public void Deserialize_OptionalNestedModel_Absent()
    {
        const string json = """{"child":{"name":"c","value":0},"children":[]}""";

        var result = Deserialize<NestedTestModel>(json);

        Assert.That(result.OptionalChild.HasValue, Is.False);
    }

    [Test]
    public void RoundTrip_ComplexModel_Preserved()
    {
        var original = new NestedTestModel
        {
            Child = new SimpleTestModel { Name = "child", Value = 1 },
            OptionalChild = new SimpleTestModel { Name = "opt", Value = 99 },
            Children =
            [
                new SimpleTestModel { Name = "a", Value = 2 },
                new SimpleTestModel { Name = "b", Value = 3 }
            ]
        };

        var json = Serialize(original);
        var result = Deserialize<NestedTestModel>(json);

        Assert.That(result.Child.Name, Is.EqualTo("child"));
        Assert.That(result.Child.Value, Is.EqualTo(1));
        Assert.That(result.OptionalChild.HasValue, Is.True);
        Assert.That(result.OptionalChild.Value.Name, Is.EqualTo("opt"));
        Assert.That(result.Children, Has.Length.EqualTo(2));
        Assert.That(result.Children[0].Name, Is.EqualTo("a"));
        Assert.That(result.Children[1].Name, Is.EqualTo("b"));
    }

    [Test]
    public void Deserialize_FieldWithoutJsonPropertyAttribute_Ignored()
    {
        const string json = """{"included":"yes","NoAttribute":"should_ignore"}""";

        var result = Deserialize<JsonIgnoreTestModel>(json);

        Assert.That(result.Included, Is.EqualTo("yes"));
        Assert.That(result.NoAttribute, Is.Null);
    }
}
