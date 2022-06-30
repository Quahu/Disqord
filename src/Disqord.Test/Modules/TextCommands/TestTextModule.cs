using System.Linq;
using Disqord.Bot.Commands.Text;
using Qmmands;
using Qmmands.Text;

namespace Disqord.Test.Modules.TextCommands;

public class TestTextModule : DiscordTextModuleBase
{
    [TextCommand("options")]
    public IResult Options(

        // Example usage:
        // options 42 -i 10 -n 1 -n 2 -n 3 -s a b c -- hello world
        int intValue,
        [Option('i', "int-option")] int intOption,
        [Option('n', "numbers")] int[] numbers,
        [Option('s', "strings", IsGreedy = true)] string[] strings,
        [Remainder] string stringValue)
    {
        return Response($"Arguments:\n{string.Join('\n', Context.RawArguments!.Select(x => $"{x.Key.Name}: {string.Join(", ", x.Value.Select(x => $"'{x}'"))}"))}");
    }

    // Components for component commands.
    [TextCommand("button1")]
    public IResult Button1()
    {
        return Response(new LocalMessage()
            .WithComponents(LocalComponent.Row(
                LocalComponent.Button(nameof(Button1), "Button 1"))));
    }

    [TextCommand("button2")]
    public IResult Button2()
    {
        return Response(new LocalMessage()
            .WithComponents(LocalComponent.Row(
                LocalComponent.Button($"{nameof(Button2)}:{Context.AuthorId}", "Button 2"))));
    }

    [TextCommand("selection1")]
    public IResult Selection1()
    {
        return Response(new LocalMessage()
            .WithComponents(LocalComponent.Row(
                LocalComponent.Selection($"{nameof(Selection1)}:hello there:{Context.AuthorId}",
                        new LocalSelectionComponentOption("First Option", "first"),
                        new LocalSelectionComponentOption("Second Option", "second"),
                        new LocalSelectionComponentOption("Third Option", "third"))
                    .WithMaximumSelectedOptions(3))));
    }

    // ---
}
