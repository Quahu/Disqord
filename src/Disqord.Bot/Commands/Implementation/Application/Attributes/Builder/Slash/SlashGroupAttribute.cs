using System;
using Qmmands;

namespace Disqord.Bot.Commands.Application;

/// <summary>
///     Marks the decorated class as a slash group module.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class SlashGroupAttribute : Attribute,
    IModuleBuilderAttribute<ApplicationModuleBuilder>
{
    /// <summary>
    ///     Gets the alias of the group.
    /// </summary>
    public string Alias { get; }

    /// <summary>
    ///     Instantiates a new <see cref="SlashGroupAttribute"/> with the specified alias.
    /// </summary>
    /// <param name="alias"> The alias of the group. </param>
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
