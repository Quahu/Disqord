using System.Collections.Generic;

namespace Disqord
{
    public interface IApplicationCommandInteraction : IInteraction, INamable
    {
        Snowflake CommandId { get; }

        IApplicationCommandInteractionResolvedData Resolved { get; }

        IReadOnlyList<IApplicationCommandInteractionOptionData> Options { get; }
    }
}