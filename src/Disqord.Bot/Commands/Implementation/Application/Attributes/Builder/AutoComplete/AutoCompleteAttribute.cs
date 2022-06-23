using System;
using Qmmands;
using Qommon;

namespace Disqord.Bot.Commands.Application;

/// <summary>
///     Marks the decorated method as an auto-complete handler for the command with the specified alias.
/// </summary>
/// <remarks>
///     The method must exist within the same module as the command.
/// </remarks>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class AutoCompleteAttribute : ApplicationCommandAttribute
{
    /// <summary>
    ///     Gets the custom <see cref="ApplicationCommandType"/> that represents an auto-complete command.
    /// </summary>
    public const ApplicationCommandType CommandType = (ApplicationCommandType) byte.MaxValue - 1;

    /// <summary>
    ///     Instantiates a new <see cref="AutoCompleteAttribute"/> with the command alias.
    /// </summary>
    /// <param name="alias"> The alias of the command to auto-complete. </param>
    public AutoCompleteAttribute(string alias)
        : base(alias)
    { }

    /// <inheritdoc/>
    protected override ApplicationCommandType GetCommandType()
    {
        return CommandType;
    }

    /// <inheritdoc/>
    public override void Apply(ICommandBuilder builder)
    {
        var applicationBuilder = Guard.IsAssignableToType<ApplicationCommandBuilder>(builder);
        applicationBuilder.Alias = applicationBuilder.Alias == null
            ? $"auto-complete:{Alias}"
            : $"{applicationBuilder.Alias};{Alias}";

        applicationBuilder.Type = GetCommandType();
        applicationBuilder.CustomAttributes.Add(this);
    }
}
