using System;
using System.Collections.Generic;
using Disqord.Models;

namespace Disqord
{
    public interface IApplicationCommandInteractionOptionData : INamable, IJsonUpdatable<ApplicationCommandInteractionDataOptionJsonModel>
    {
        /// <summary>
        ///     Gets the command type of this interaction
        /// </summary>
        ApplicationCommandOptionType Type { get; }

        /// <summary>
        ///     Gets the value of this option
        /// </summary>
        IConvertible Value { get; }

        /// <summary>
        ///     Gets the nested options of this option
        /// </summary>
        IReadOnlyList<IApplicationCommandInteractionOptionData> Options { get; }
    }
}