using Qommon;

namespace Disqord.Bot.Commands.Application;

internal sealed class UnfocusedAutoComplete<T> : AutoComplete<T>
    where T : notnull
{
    public override bool IsFocused => false;

    public override string? RawArgument => null;

    public override Optional<T> Argument { get; }

    public override ChoiceCollection? Choices => null;

    public UnfocusedAutoComplete(Optional<T> argument)
    {
        Argument = argument;
    }
}
