using System;

namespace Disqord.Bot.Commands.Application;

public class ApplicationCommandMappingException : Exception
{
    public ApplicationModule? Module { get; init; }

    public ApplicationCommand? Command { get; init; }

    public ApplicationCommandMappingException(string? message)
        : base(message)
    { }

    public ApplicationCommandMappingException(string? message, Exception? innerException)
        : base(message, innerException)
    { }
}
