using System.ComponentModel;

namespace Disqord;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class SlashCommandOptionTypeExtensions
{
    public static bool IsEntity(this SlashCommandOptionType type)
    {
        return type is SlashCommandOptionType.User or SlashCommandOptionType.Channel or SlashCommandOptionType.Role or SlashCommandOptionType.Mentionable or SlashCommandOptionType.Attachment;
    }

    public static bool IsPrimitive(this SlashCommandOptionType type)
    {
        return type is SlashCommandOptionType.String or SlashCommandOptionType.Integer or SlashCommandOptionType.Boolean or SlashCommandOptionType.Number;
    }
}
