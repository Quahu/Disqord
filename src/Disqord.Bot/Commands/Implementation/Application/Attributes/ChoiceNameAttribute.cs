using System;

namespace Disqord.Bot.Commands.Application;

/// <summary>
///     Overrides the choice name of the decorated enum field.
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class ChoiceNameAttribute : Attribute
{
    /// <summary>
    ///     Gets the name of the choice.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Instantiates a new <see cref="ChoiceNameAttribute"/> with the specified name.
    /// </summary>
    /// <param name="name"> The name of the choice. </param>
    public ChoiceNameAttribute(string name)
    {
        Name = name;
    }
}
