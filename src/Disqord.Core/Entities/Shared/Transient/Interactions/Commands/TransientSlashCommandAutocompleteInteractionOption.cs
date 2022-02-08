using Disqord.Models;

namespace Disqord
{
    public class TransientSlashCommandAutocompleteInteractionOption : TransientSlashCommandInteractionOption, ISlashCommandAutocompleteInteractionOption
    {
        /// <inheritdoc/>
        public bool IsFocused => Model.Focused.GetValueOrDefault();

        public TransientSlashCommandAutocompleteInteractionOption(IClient client, ApplicationCommandInteractionDataOptionJsonModel model)
            : base(client, model)
        { }
    }
}
