namespace Disqord
{
    public interface IApplicationCommandOptionChoice : INamable
    {
        object Value { get; }
    }
}
