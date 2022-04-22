using System;
using System.Collections.Generic;
using Disqord.Models;
using Qommon.Collections;
using Qommon.Collections.ReadOnly;

namespace Disqord
{
    public class TransientSlashCommandInteraction : TransientApplicationCommandInteraction, ISlashCommandInteraction
    {
        /// <inheritdoc/>
        public IReadOnlyDictionary<string, ISlashCommandInteractionOption> Options
        {
            get
            {
                if (!Model.Data.Value.Options.HasValue)
                    return ReadOnlyDictionary<string, ISlashCommandInteractionOption>.Empty;

                return _options ??= Model.Data.Value.Options.Value.ToReadOnlyDictionary(Client,
                    (model, _) => model.Name,
                    (model, client) => new TransientSlashCommandInteractionOption(client, model) as ISlashCommandInteractionOption, StringComparer.OrdinalIgnoreCase);
            }
        }
        private IReadOnlyDictionary<string, ISlashCommandInteractionOption> _options;

        public TransientSlashCommandInteraction(IClient client, InteractionJsonModel model)
            : base(client, model)
        { }
    }
}
