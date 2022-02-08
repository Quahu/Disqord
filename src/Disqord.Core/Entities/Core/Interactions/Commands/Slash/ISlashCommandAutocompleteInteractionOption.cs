namespace Disqord
{
    public interface ISlashCommandAutocompleteInteractionOption : ISlashCommandInteractionOption
    {
        /// <summary>
        ///     Gets whether this option is focused.
        /// </summary>
        bool IsFocused { get; }
    }
}
