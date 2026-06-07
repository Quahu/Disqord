using Disqord.Bot.Commands;
using Disqord.Bot.Commands.Application;
using Disqord.Serialization.Json.Default;
using Qmmands;

namespace Disqord.TestBot.Modules.ApplicationCommands;

public class ApplicationModule : DiscordApplicationModuleBase
{
    // API enabled globally, but fails in guilds because of the check
    [SlashCommand("private-only")]
    [RequirePrivate]
    public IResult PrivateOnly()
    {
        return Response("Success!");
    }

    // API enabled in guilds only
    [SlashCommand("guild-only")]
    [RequireGuild]
    public IResult GuildOnly()
    {
        return Response("Success!");
    }

    [SlashCommand("choice-enum")]
    public IResult ChoiceEnum(ChoiceTestEnum value)
    {
        return Response($"Enum value: {value} ({(int) value})");
    }

    [SlashCommand("choice-string")]
    public IResult ChoiceString(
        [Choice("Hello", "hello")] [Choice("World", "world")] [Choice("Foo Bar", "foo_bar")] string value)
    {
        return Response($"String value: {value}");
    }

    [SlashCommand("choice-int")]
    public IResult ChoiceInt(
        [Choice("One", 1)] [Choice("Forty Two", 42)] [Choice("Max", int.MaxValue)] int value)
    {
        return Response($"Int value: {value}");
    }

    [SlashCommand("choice-long")]
    public IResult ChoiceLong(
        [Choice("One", 1L)] [Choice("Big", 1234567890123L)] [Choice("Max", JsonUtilities.MaxSafeInteger)] long value)
    {
        return Response($"Long value: {value}");
    }

    [SlashCommand("choice-double")]
    public IResult ChoiceDouble(
        [Choice("Pi", double.Pi)] [Choice("Zero", 0.0)] [Choice("Negative", -1.5)] double value)
    {
        return Response($"Double value: {value}");
    }

    [SlashCommand("choice-enum-string")]
    public IResult ChoiceEnumString(ChoiceTestEnum enumValue,
        [Choice("Extra A", "a")] [Choice("Extra B", "b")] string stringValue)
    {
        return Response($"Enum: {enumValue}, String: {stringValue}");
    }

    public enum ChoiceTestEnum
    {
        [ChoiceName("One")]
        Alpha,

        [ChoiceName("Two")]
        Beta,

        Gamma
    }
}
