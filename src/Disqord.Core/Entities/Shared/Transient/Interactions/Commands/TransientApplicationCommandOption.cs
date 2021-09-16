using System.Collections.Generic;
using Disqord.Collections;
using Disqord.Models;

namespace Disqord
{
    public class TransientApplicationCommandOption : TransientEntity<ApplicationCommandOptionJsonModel>, IApplicationCommandOption
    {
        public string Name => Model.Name;

        public ApplicationCommandOptionType Type => Model.Type;

        public string Description => Model.Description;

        public bool Required => Model.Required.GetValueOrDefault();

        public IReadOnlyList<IApplicationCommandOptionChoice> Choices => _choices ??= Model.Choices.Value.ToReadOnlyList(Client, (model, client) => new TransientApplicationCommandOptionChoice(client, model));
        private IReadOnlyList<IApplicationCommandOptionChoice> _choices;

        public virtual IReadOnlyList<IApplicationCommandOption> Options => _options ??= Model.Options.Value.ToReadOnlyList(Client, (model, client) => new TransientApplicationCommandOption(client, model));
        private IReadOnlyList<IApplicationCommandOption> _options;

        public TransientApplicationCommandOption(IClient client, ApplicationCommandOptionJsonModel model)
            : base(client, model)
        { }
    }
}
