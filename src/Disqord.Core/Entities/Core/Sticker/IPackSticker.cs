namespace Disqord
{
    public interface IPackSticker : ISticker
    {
        Snowflake PackId { get; }

        int SortValue { get; }
    }
}