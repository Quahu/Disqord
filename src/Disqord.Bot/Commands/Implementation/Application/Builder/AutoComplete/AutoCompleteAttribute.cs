using System;
using Qmmands;
using Qommon;

namespace Disqord.Bot.Commands.Application;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class AutoCompleteAttribute : ApplicationCommandAttribute
{
    public const ApplicationCommandType CommandType = (ApplicationCommandType) byte.MaxValue - 1;

    public AutoCompleteAttribute(string alias)
        : base(alias)
    { }

    protected override ApplicationCommandType GetCommandType()
    {
        return CommandType;
    }

    public override void Apply(ICommandBuilder builder)
    {
        var slashBuilder = Guard.IsAssignableToType<ApplicationCommandBuilder>(builder);
        slashBuilder.Alias = slashBuilder.Alias == null
            ? $"auto-complete:{Alias}"
            : $"{slashBuilder.Alias};{Alias}";

        slashBuilder.Type = GetCommandType();
        slashBuilder.CustomAttributes.Add(this);
    }
}
