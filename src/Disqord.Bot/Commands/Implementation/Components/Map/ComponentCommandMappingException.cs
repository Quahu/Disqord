using System;

namespace Disqord.Bot.Commands.Components;

public class ComponentCommandMappingException : Exception
{
    public ComponentCommand Command { get; }

    public ComponentCommandMappingException(ComponentCommand command, string? message)
        : base(message)
    {
        Command = command;
    }

    public ComponentCommandMappingException(ComponentCommand command, string? message, Exception? innerException)
        : base(message, innerException)
    {
        Command = command;
    }
}
