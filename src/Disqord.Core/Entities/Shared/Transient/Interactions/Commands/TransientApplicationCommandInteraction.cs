using Disqord.Interaction;
using Disqord.Models;

namespace Disqord
{
    public class TransientApplicationCommandInteraction : TransientInteraction, IApplicationCommandInteraction
    {
        /// <inheritdoc/>
        public Snowflake CommandId => Model.Data.Value.Id.Value;

        /// <inheritdoc/>
        public string CommandName => Model.Data.Value.Name.Value;

        /// <inheritdoc/>
        public ApplicationCommandType CommandType => Model.Data.Value.Type.Value;

        /// <inheritdoc/>
        public IApplicationCommandInteractionEntities Entities => _entities ??= new TransientApplicationCommandInteractionEntities(Client, GuildId, Model.Data.Value.Resolved.Value);
        private IApplicationCommandInteractionEntities _entities;

        public TransientApplicationCommandInteraction(IClient client, InteractionJsonModel model)
            : base(client, model)
        { }
    }
}
