﻿using Disqord.Models;

namespace Disqord
{
    /// <summary>
    ///     Represents a preset choice for a slash command option.
    /// </summary>
    public interface ISlashCommandOptionChoice : INamable, IJsonUpdatable<ApplicationCommandOptionChoiceJsonModel>
    {
        /// <summary>
        ///     Gets the value of this choice.
        /// </summary>
        object Value { get; }
    }
}
