using System.Collections.Generic;
using Disqord.Models;

namespace Disqord
{
    public interface IApplicationCommandOption : INamable, IJsonUpdatable<ApplicationCommandOptionJsonModel>
    {
        ApplicationCommandOptionType Type { get; }

        string Description { get; }

        bool IsRequired { get; }

        IReadOnlyList<IApplicationCommandOptionChoice> Choices { get; }

        IReadOnlyList<IApplicationCommandOption> Options { get; }
    }
}