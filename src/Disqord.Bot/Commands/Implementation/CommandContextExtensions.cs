using System.Collections.Generic;
using Qmmands;
using Qommon;

namespace Disqord.Bot.Commands;

internal static class CommandContextExtensions
{
    public static void SetRawArgument(this ICommandContext context, IParameter parameter, MultiString rawArgument)
    {
        (context.RawArguments ??= new Dictionary<IParameter, MultiString>())[parameter] = rawArgument;
    }

    public static void SetArgument(this ICommandContext context, IParameter parameter, object? argument)
    {
        (context.Arguments ??= new Dictionary<IParameter, object?>())[parameter] = argument;
    }
}
