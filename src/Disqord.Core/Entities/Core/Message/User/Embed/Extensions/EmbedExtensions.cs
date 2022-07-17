using System.ComponentModel;

namespace Disqord;

/// <summary>
///     Represents embed extensions.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public static class EmbedExtensions
{
    /// <summary>
    ///     Checks if the type of this embed is <c>rich</c>.
    /// </summary>
    /// <param name="embed"> The embed to check. </param>
    /// <returns>
    ///     <see langword="true"/> if the embed is <c>rich</c>.
    /// </returns>
    public static bool IsRich(this IEmbed embed)
    {
        return embed.Type == "rich";
    }
}
