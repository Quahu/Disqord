namespace Disqord.Serialization.Json
{
    public interface IJsonValue : IJsonToken
    {
        object? Value { get; }
    }
}
