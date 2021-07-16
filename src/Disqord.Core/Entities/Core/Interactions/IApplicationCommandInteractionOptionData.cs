using System;
using System.Collections.Generic;
using Disqord.Models;

namespace Disqord
{
    public interface IApplicationCommandInteractionOptionData : INamable, IJsonUpdatable<ApplicationCommandInteractionDataOptionJsonModel>
    {
        ApplicationCommandOptionType Type { get; }

        IConvertible Value { get; }

        IReadOnlyList<IApplicationCommandInteractionOptionData> Options { get; }
    }
}