using Disqord.Models;

namespace Disqord
{
    public interface IApplicationCommandOptionChoice : INamable, IJsonUpdatable<ApplicationCommandOptionChoiceJsonModel>
    {
        object Value { get; }
    }
}
