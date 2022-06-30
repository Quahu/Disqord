using System.Linq;
using System.Threading.Tasks;
using Disqord.Bot.Commands.Components;
using Disqord.Rest;
using Qmmands;

namespace Disqord.Test.Modules.ComponentCommands;

public class TestComponentModule : DiscordComponentModuleBase
{
    // The components for the commands below are all defined in TestTextModule.
    [ButtonCommand("Button1")]
    public Task Button1()
    {
        var modal = new LocalInteractionModalResponse()
            .WithCustomId(nameof(Modal1))
            .WithTitle("Favorite Food")
            .WithComponents(
                LocalComponent.Row(LocalComponent.TextInput("name", "Name", TextInputComponentStyle.Short)),
                LocalComponent.Row(LocalComponent.TextInput("favoriteFood", "Favorite Food", TextInputComponentStyle.Short)));

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
    public IResult Modal1(string name, string favoriteFood)
    {
        return Response(new LocalInteractionMessageResponse()
            .WithContent($"Your name is {name} and your favorite food is {favoriteFood}.")
            .WithIsEphemeral());
    }

    // ---
}
