using Disqord.Models;

namespace Disqord
{
    public class TransientSlashCommandAutoCompleteInteractionOption : TransientSlashCommandInteractionOption, ISlashCommandAutoCompleteInteractionOption
    {
        /// <inheritdoc/>
        public bool IsFocused => Model.Focused.GetValueOrDefault();

        public TransientSlashCommandAutoCompleteInteractionOption(IClient client, ApplicationCommandInteractionDataOptionJsonModel model)
            : base(client, model)
        { }
    }
}
