using System;
using System.Collections.Generic;
using Disqord.Models;
using Qommon;
using Qommon.Collections;
using Qommon.Collections.ReadOnly;

namespace Disqord
{
    public class TransientSlashCommandInteractionOption : TransientClientEntity<ApplicationCommandInteractionDataOptionJsonModel>, ISlashCommandInteractionOption
    {
        /// <inheritdoc/>
        public string Name => Model.Name;

        /// <inheritdoc/>
        public SlashCommandOptionType Type => Model.Type;

        /// <inheritdoc/>
        public object Value => Model.Value.GetValueOrDefault()?.Value;

        /// <inheritdoc/>
        public IReadOnlyDictionary<string, ISlashCommandInteractionOption> Options
        {
            get
            {
                if (!Model.Options.HasValue)
                    return ReadOnlyDictionary<string, ISlashCommandInteractionOption>.Empty;

                return _options ??= Model.Options.Value.ToReadOnlyDictionary(Client,
                    (model, _) => model.Name,
                    (model, client) => new TransientSlashCommandInteractionOption(client, model) as ISlashCommandInteractionOption, StringComparer.OrdinalIgnoreCase);
            }
        }
        private IReadOnlyDictionary<string, ISlashCommandInteractionOption> _options;

        public TransientSlashCommandInteractionOption(IClient client, ApplicationCommandInteractionDataOptionJsonModel model)
            : base(client, model)
        { }
    }
}
