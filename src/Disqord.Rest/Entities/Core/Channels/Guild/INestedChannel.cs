namespace Disqord
{
    public interface INestedChannel : IGuildChannel
    {
        Snowflake? CategoryId { get; }
    }
}
