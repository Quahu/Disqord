namespace Disqord;

/// <summary>
///     Represents extension methods for <see cref="IGuild"/>.
/// </summary>
public static class GuildExtensions
{
    /// <summary>
    ///     Gets a wrapper around the given guild's feature strings.
    /// </summary>
    /// <param name="guild"> The guild to get the features of. </param>
    /// <returns>
    ///     A <see cref="GuildFeatures"/> wrapping the guild's feature strings.
    /// </returns>
    public static GuildFeatures GetFeatures(this IGuild guild)
    {
        return new(guild.Features);
    }
}
