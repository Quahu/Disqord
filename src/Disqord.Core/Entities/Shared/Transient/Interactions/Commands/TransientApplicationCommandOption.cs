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

        public bool IsRequired => Model.Required.GetValueOrDefault();

        public IReadOnlyList<IApplicationCommandOptionChoice> Choices => _choices ??= Optional.ConvertOrDefault(Model.Choices, (models, client) => models.ToReadOnlyList(client, (model, client) => new TransientApplicationCommandOptionChoice(client, model)), Client) ?? ReadOnlyList<IApplicationCommandOptionChoice>.Empty;
        private IReadOnlyList<IApplicationCommandOptionChoice> _choices;

        public IReadOnlyList<IApplicationCommandOption> Options => _options ??= Optional.ConvertOrDefault(Model.Options, (models, client) => models.ToReadOnlyList(client, (model, client) => new TransientApplicationCommandOption(client, model)), Client) ?? ReadOnlyList<IApplicationCommandOption>.Empty;
        private IReadOnlyList<IApplicationCommandOption> _options;

        public IReadOnlyList<ChannelType> ChannelTypes => _channelTypes ??= Optional.ConvertOrDefault(Model.ChannelTypes, x => x.ToReadOnlyList(), ReadOnlyList<ChannelType>.Empty);
        private IReadOnlyList<ChannelType> _channelTypes;

        public TransientApplicationCommandOption(IClient client, ApplicationCommandOptionJsonModel model)
            : base(client, model)
        { }
    }
}
