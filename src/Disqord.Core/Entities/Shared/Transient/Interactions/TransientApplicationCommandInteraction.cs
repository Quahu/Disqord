using System.Collections.Generic;
using Disqord.Models;

namespace Disqord.Interaction
{
    public class TransientApplicationCommandInteraction : TransientInteraction, IApplicationCommandInteraction
    {
        public string Name => Model.Data.Value.Name.Value;

        public Snowflake CommandId => Model.Data.Value.Id.Value;

        // TODO: Map these
        public IApplicationCommandInteractionResolvedData Resolved { get; }

        public IReadOnlyList<IApplicationCommandInteractionOptionData> Options { get; }

        public TransientApplicationCommandInteraction(IClient client, InteractionJsonModel model)
            : base(client, model)
        { }
    }
}