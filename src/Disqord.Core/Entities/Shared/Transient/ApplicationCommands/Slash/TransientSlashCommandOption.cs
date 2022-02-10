using System.Collections.Generic;
using Disqord.Models;
using Qommon.Collections;

namespace Disqord
{
    public class TransientSlashCommandOption : TransientClientEntity<ApplicationCommandOptionJsonModel>, ISlashCommandOption
    {
        /// <inheritdoc/>
        public string Name => Model.Name;

        /// <inheritdoc/>
        public SlashCommandOptionType Type => Model.Type;

        /// <inheritdoc/>
        public string Description => Model.Description;

        /// <inheritdoc/>
        public bool IsRequired => Model.Required.GetValueOrDefault();

        /// <inheritdoc/>
        public IReadOnlyList<ISlashCommandOptionChoice> Choices
        {
            get
            {
                if (!Model.Choices.HasValue)
                    return ReadOnlyList<ISlashCommandOptionChoice>.Empty;

                return _choices ??= Model.Choices.Value.ToReadOnlyList(Client, (model, client) => new TransientSlashCommandOptionChoice(client, model));
            }
        }
        private IReadOnlyList<ISlashCommandOptionChoice> _choices;

        /// <inheritdoc/>
        public bool HasAutoComplete => Model.AutoComplete.GetValueOrDefault();

        /// <inheritdoc/>
        public IReadOnlyList<ISlashCommandOption> Options
        {
            get
            {
                if (!Model.Options.HasValue)
                    return ReadOnlyList<ISlashCommandOption>.Empty;

                return _options ??= Model.Options.Value.ToReadOnlyList(Client, (model, client) => new TransientSlashCommandOption(client, model));
            }
        }
        private IReadOnlyList<ISlashCommandOption> _options;

        /// <inheritdoc/>
        public IReadOnlyList<ChannelType> ChannelTypes
        {
            get
            {
                if (!Model.ChannelTypes.HasValue)
                    return ReadOnlyList<ChannelType>.Empty;

                return Model.ChannelTypes.Value.ReadOnly();
            }
        }

        public TransientSlashCommandOption(IClient client, ApplicationCommandOptionJsonModel model)
            : base(client, model)
        { }
    }
}
