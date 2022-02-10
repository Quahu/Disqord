namespace Disqord
{
    public interface ISlashCommandAutoCompleteInteractionOption : ISlashCommandInteractionOption
    {
        /// <summary>
        ///     Gets whether this option is focused for auto-complete.
        /// </summary>
        bool IsFocused { get; }
    }
}
