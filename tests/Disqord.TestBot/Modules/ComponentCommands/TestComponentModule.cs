using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Disqord.Bot.Commands.Components;
using Disqord.Rest;
using Qmmands;

namespace Disqord.TestBot.Modules.ComponentCommands;

public class TestComponentModule : DiscordComponentModuleBase
{
    // The components for the commands below are all defined in TestTextModule.
    [ButtonCommand("Button1")]
    public Task Button1()
    {
        var foodOptions = new LocalSelectionComponentOption[]
        {
            new("🍕", "pizza"),
            new("🥩", "steak"),
            new("🍣", "sushi"),
        };

        var modal = new LocalInteractionModalResponse()
            .WithCustomId(nameof(Modal1))
            .WithTitle("Favorite Food")
            .WithComponents(
                LocalComponent.TextDisplay($"Hello, {Context.Author.Mention}!"),
                LocalComponent.Label("Name", "Please provide your name.", LocalComponent.TextInput("name")),
                LocalComponent.Label("Favorite Food", "Choose your favorite food", LocalComponent.Selection("favoriteFood", foodOptions).WithMaximumSelectedOptions(foodOptions.Length)),
                LocalComponent.Label("Favorite Members", "Choose your favorite members", LocalComponent.Selection("favoriteMembers").WithType(SelectionComponentType.User).WithMaximumSelectedOptions(10).WithMinimumSelectedOptions(2)),
                LocalComponent.Label("Files", "Upload your favorite files", LocalComponent.FileUpload("favoriteFiles").WithMaximumUploadedFiles(1))
            );

        return Context.Interaction.Response().SendModalAsync(modal);
    }

    [ButtonCommand("Button2:*")]
    public IResult Button2(Snowflake id)
    {
        return Response($"This is the callback of {nameof(Button2)} with ID wildcard `{id}`.");
    }

    [SelectionCommand("Selection1:*:*")]
    public IResult Selection1(string value1, Snowflake value2, string[] selectedValues)
    {
        return Response($"This is the callback of {nameof(Selection1)} with wildcards: `{value1}`, `{value2}` "
            + $"and selected values {string.Join(", ", selectedValues.Select(Markdown.Code))}.");
    }

    [ModalCommand("Modal1")]
    public IResult Modal1(string name, string[] favoriteFood, IMember[] favoriteMembers, IAttachment secretFile)
    {
        return Response(new LocalInteractionMessageResponse()
            .WithContent($"Your name is {name} and your favorite food is: {string.Join(", ", favoriteFood)}.\n\nYour favorite members are: {string.Join(", ", favoriteMembers.Select(x => x.Mention))}\n\nI got your secret file: {secretFile.FileName}")
            .WithIsEphemeral());
    }

    // ---
}
