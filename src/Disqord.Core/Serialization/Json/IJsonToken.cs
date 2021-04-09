namespace Disqord.Serialization.Json
{
    public interface IJsonToken
    {
        T ToType<T>();
    }
}