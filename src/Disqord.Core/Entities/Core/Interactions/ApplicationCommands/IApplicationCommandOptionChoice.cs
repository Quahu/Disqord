using System;
using Disqord.Models;

namespace Disqord
{
    public interface IApplicationCommandOptionChoice : INamable, IJsonUpdatable<ApplicationCommandOptionChoiceJsonModel>
    {
        IConvertible Value { get; }
    }
}