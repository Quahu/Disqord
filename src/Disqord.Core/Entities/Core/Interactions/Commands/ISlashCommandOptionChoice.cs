using Disqord.Models;

namespace Disqord
{
    public interface ISlashCommandOptionChoice : INamable, IJsonUpdatable<ApplicationCommandOptionChoiceJsonModel>
    {
        object Value { get; }
    }
}
