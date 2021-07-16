using System;
using System.Collections.Generic;
using Disqord.Collections;
using Disqord.Models;

namespace Disqord.Interaction
{
    public class TransientApplicationCommandInteractionOptionData : TransientEntity<ApplicationCommandInteractionDataOptionJsonModel>, IApplicationCommandInteractionOptionData
    {
        public string Name => Model.Name;

        public ApplicationCommandOptionType Type => Model.Type;

        // TODO: Map these
        public IReadOnlyList<IApplicationCommandInteractionOptionData> Options { get; }

        public IConvertible Value { get; }

        public TransientApplicationCommandInteractionOptionData(IClient client, ApplicationCommandInteractionDataOptionJsonModel model)
            : base(client, model)
        { }
    }
}