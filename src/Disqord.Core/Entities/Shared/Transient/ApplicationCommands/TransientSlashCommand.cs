using System.Collections.Generic;
using Qommon.Collections;
using Disqord.Models;

namespace Disqord
{
    public class TransientSlashCommand : TransientApplicationCommand, ISlashCommand
    {
        /// <inheritdoc/>
        public string Description => Model.Description;

        /// <inheritdoc/>
        public IReadOnlyList<ISlashCommandOption> Options => _options ??= Optional.ConvertOrDefault(Model.Options, (models, client) => models.ToReadOnlyList(client, (model, client) => new TransientSlashCommandOption(client, model)), Client) ?? ReadOnlyList<ISlashCommandOption>.Empty;
        private IReadOnlyList<ISlashCommandOption> _options;

        public TransientSlashCommand(IClient client, ApplicationCommandJsonModel model)
            : base(client, model)
        { }
    }
}
