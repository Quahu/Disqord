using Disqord.Models;

namespace Disqord
{
    public class TransientApplicationCommandOptionChoice : TransientEntity<ApplicationCommandOptionChoiceJsonModel>, IApplicationCommandOptionChoice
    {
        public string Name => Model.Name;

        public object Value => Model.Value.Value;

        public TransientApplicationCommandOptionChoice(IClient client, ApplicationCommandOptionChoiceJsonModel model)
            : base(client, model)
        { }
    }
}
