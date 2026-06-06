using Disqord.Serialization.Json.Default;

namespace Disqord.Tests.Serialization;

public abstract class SerializationTestBase
{
    protected DefaultJsonSerializer Serializer = null!;

    [SetUp]
    public void SetUp()
    {
        Serializer = new DefaultJsonSerializer();
    }

    protected T Deserialize<T>(string json)
    {
        using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json));
        return Serializer.Deserialize<T>(stream)!;
    }

    protected string Serialize(object obj)
    {
        using var stream = new MemoryStream();
        Serializer.Serialize(stream, obj);
        stream.Position = 0;
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
