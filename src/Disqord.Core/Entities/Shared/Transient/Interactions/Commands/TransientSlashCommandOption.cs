using System.Collections.Generic;
using Qommon.Collections;
using Disqord.Models;

namespace Disqord
{
    public class TransientSlashCommandOption : TransientEntity<ApplicationCommandOptionJsonModel>, ISlashCommandOption
    {
        public string Name => Model.Name;

        public SlashCommandOptionType Type => Model.Type;

        public string Description => Model.Description;

        public bool IsRequired => Model.Required.GetValueOrDefault();

        public IReadOnlyList<ISlashCommandOptionChoice> Choices => _choices ??= Optional.ConvertOrDefault(Model.Choices, (models, client) => models.ToReadOnlyList(client, (model, client) => new TransientSlashCommandOptionChoice(client, model)), Client) ?? ReadOnlyList<ISlashCommandOptionChoice>.Empty;
        private IReadOnlyList<ISlashCommandOptionChoice> _choices;

        public IReadOnlyList<ISlashCommandOption> Options => _options ??= Optional.ConvertOrDefault(Model.Options, (models, client) => models.ToReadOnlyList(client, (model, client) => new TransientSlashCommandOption(client, model)), Client) ?? ReadOnlyList<ISlashCommandOption>.Empty;
        private IReadOnlyList<ISlashCommandOption> _options;

        public IReadOnlyList<ChannelType> ChannelTypes => _channelTypes ??= Optional.ConvertOrDefault(Model.ChannelTypes, x => x.ToReadOnlyList(), ReadOnlyList<ChannelType>.Empty);
        private IReadOnlyList<ChannelType> _channelTypes;

        public TransientSlashCommandOption(IClient client, ApplicationCommandOptionJsonModel model)
            : base(client, model)
        { }
    }
}
