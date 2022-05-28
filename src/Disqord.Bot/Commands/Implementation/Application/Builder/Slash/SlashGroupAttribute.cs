using System;
using Qmmands;

namespace Disqord.Bot.Commands.Application;

public class SlashGroupAttribute : Attribute,
    IModuleBuilderAttribute<ApplicationModuleBuilder>
{
    public string Alias { get; }

    public SlashGroupAttribute(string alias)
    {
        Alias = alias;
    }

    public void Apply(ApplicationModuleBuilder builder)
    {
        builder.Alias = Alias;
    }
}
