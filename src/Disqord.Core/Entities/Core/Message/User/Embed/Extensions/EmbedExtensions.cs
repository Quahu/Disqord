namespace Disqord
{
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
            => embed.Type == "rich";
    }
}
