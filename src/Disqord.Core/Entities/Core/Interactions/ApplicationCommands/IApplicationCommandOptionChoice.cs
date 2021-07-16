using System;
using Disqord.Models;

namespace Disqord
{
    /// <summary>
    ///     Represents an application command option choice
    /// </summary>
    public interface IApplicationCommandOptionChoice : INamable, IJsonUpdatable<ApplicationCommandOptionChoiceJsonModel>
    {
        /// <summary>
        ///     Gets the value of this option
        /// </summary>
        IConvertible Value { get; }
    }
}