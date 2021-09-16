using System.Collections.Generic;

namespace Disqord
{
    public interface IApplicationCommandOption : INamable
    {
        ApplicationCommandOptionType Type { get; }

        string Description { get; }

        bool Required { get; }

        IReadOnlyList<IApplicationCommandOptionChoice> Choices { get; }

        IReadOnlyList<IApplicationCommandOption> Options { get; }
    }
}
