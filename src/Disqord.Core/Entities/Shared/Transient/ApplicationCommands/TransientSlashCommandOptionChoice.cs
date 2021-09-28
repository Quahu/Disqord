using Disqord.Models;

namespace Disqord
{
    public class TransientSlashCommandOptionChoice : TransientEntity<ApplicationCommandOptionChoiceJsonModel>, ISlashCommandOptionChoice
    {
        /// <inheritdoc/>
        public string Name => Model.Name;

        /// <inheritdoc/>
        public object Value => Model.Value.Value;

        public TransientSlashCommandOptionChoice(IClient client, ApplicationCommandOptionChoiceJsonModel model)
            : base(client, model)
        { }
    }
}
