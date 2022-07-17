using System;

namespace Disqord.Bot.Commands.Application;

/// <summary>
///     Defines a choice for an application command parameter.
/// </summary>
/// <seealso cref="Discord.Limits.ApplicationCommand.Option.MaxChoiceAmount"/>
[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true)]
public class ChoiceAttribute : Attribute
{
    /// <summary>
    ///     Gets the name of the choice.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Gets the value of the choice.
    /// </summary>
    public object Value { get; }

    /// <summary>
    ///     Instantiates a new <see cref="ChoiceAttribute"/> with the specified name and value.
    /// </summary>
    /// <param name="name"> The name of the choice. </param>
    /// <param name="value"> The value of the choice. </param>
    public ChoiceAttribute(string name, object value)
    {
        Name = name;
        Value = value;
    }
}
