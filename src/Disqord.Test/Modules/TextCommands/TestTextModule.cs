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
}
