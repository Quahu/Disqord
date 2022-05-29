using System;
using Qmmands;

namespace Disqord.Bot.Commands.Application;

[AttributeUsage(AttributeTargets.Class)]
public class SlashGroupAttribute : Attribute,
    IModuleBuilderAttribute<ApplicationModuleBuilder>
{
    public string Alias { get; }

    public SlashGroupAttribute(string alias)
    {
        Alias = alias;
    }

    /// <inheritdoc/>
    public void Apply(ApplicationModuleBuilder builder)
    {
        builder.Alias = Alias;
    }
}
