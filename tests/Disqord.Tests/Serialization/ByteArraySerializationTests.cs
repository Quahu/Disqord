using Disqord.Tests.Models;

namespace Disqord.Tests.Serialization;

[TestFixture]
public class ByteArraySerializationTests : SerializationTestBase
{
    [Test]
    public void Serialize_ByteArray_WritesNumberArray()
    {
        var model = new ByteArrayTestModel { Data = [1, 2, 3, 255] };

        var json = Serialize(model);

        Assert.That(json, Does.Contain(""""data":[1,2,3,255]""""));
    }

    [Test]
    public void Serialize_ByteArray_NotBase64()
    {
        byte[] data = [1, 2, 3];
        var model = new ByteArrayTestModel { Data = data };

        var json = Serialize(model);

        Assert.That(json, Does.Not.Contain(Convert.ToBase64String(data)));
    }

    [Test]
    public void Deserialize_ByteArray_FromNumberArray()
    {
        const string json = """{"data":[10,20,30]}""";

        var result = Deserialize<ByteArrayTestModel>(json);

        Assert.That(result.Data, Is.EqualTo(new byte[] { 10, 20, 30 }));
    }

    [Test]
    public void RoundTrip_ByteArray_Preserved()
    {
        var original = new ByteArrayTestModel { Data = [0, 127, 255] };

        var json = Serialize(original);
        var result = Deserialize<ByteArrayTestModel>(json);

        Assert.That(result.Data, Is.EqualTo(original.Data));
    }

    [Test]
    public void Serialize_EmptyByteArray_WritesEmptyArray()
    {
        var model = new ByteArrayTestModel { Data = [] };

        var json = Serialize(model);

        Assert.That(json, Does.Contain(""""data":[]""""));
    }
}
