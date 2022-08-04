using Qommon;

namespace Disqord.Bot.Commands.Application;

internal sealed class FocusedAutoComplete<T> : AutoComplete<T>
    where T : notnull
{
    public override bool IsFocused => true;

    public override string? RawArgument { get; }

    public override Optional<T> Argument => default;

    public override ChoiceCollection? Choices { get; }

    public FocusedAutoComplete(string rawArgument)
    {
        RawArgument = rawArgument;
        Choices = new ChoiceCollection();
    }
}
