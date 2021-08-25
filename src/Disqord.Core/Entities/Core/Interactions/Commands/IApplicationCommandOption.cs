using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Disqord
{
    public interface IApplicationCommandOption : INamable
    {
        ApplicationCommandOptionType Type { get; }

        string Description { get; }

        bool? Required { get; }

        IReadOnlyList<IApplicationCommandOptionChoice> Choices { get; }

        IReadOnlyList<IApplicationCommandOption> Options { get; }
    }
}
