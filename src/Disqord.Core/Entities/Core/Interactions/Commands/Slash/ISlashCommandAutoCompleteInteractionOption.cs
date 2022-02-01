namespace Disqord
{
    public interface ISlashCommandAutoCompleteInteractionOption : ISlashCommandInteractionOption
    {
        /// <summary>
        ///     Gets whether this option is focused.
        /// </summary>
        bool IsFocused { get; }
    }
}
