using Disqord.Models;

namespace Disqord
{
    /// <summary>
    ///     Gets the type of this slash command option choice.
    /// </summary>
    public interface ISlashCommandOptionChoice : INamable, IJsonUpdatable<ApplicationCommandOptionChoiceJsonModel>
    {
        /// <summary>
        ///     Gets the value of this slash command option choice.
        /// </summary>
        object Value { get; }
    }
}
