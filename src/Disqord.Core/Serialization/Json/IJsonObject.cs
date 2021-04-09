namespace Disqord.Serialization.Json
{
    public interface IJsonObject : IJsonToken
    {
        IJsonToken this[string key] { get; }
    }
}
