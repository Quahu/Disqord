using System.Collections.Generic;
using Disqord.Models;
using Qommon.Collections;
using Qommon.Collections.ReadOnly;

namespace Disqord
{
    public class TransientSlashCommand : TransientApplicationCommand, ISlashCommand
    {
        /// <inheritdoc/>
        public string Description => Model.Description;

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

        public TransientSlashCommand(IClient client, ApplicationCommandJsonModel model)
            : base(client, model)
        { }
    }
}
