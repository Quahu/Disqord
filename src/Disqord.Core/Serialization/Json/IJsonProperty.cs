namespace Disqord.Serialization.Json
{
    public interface IJsonProperty : IJsonToken
    {
        string Name { get; }

        IJsonToken Value { get; }
    }
}
