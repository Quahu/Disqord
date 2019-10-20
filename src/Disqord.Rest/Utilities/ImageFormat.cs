namespace Disqord
{
    /// <summary>
    ///     Represents the format of an image.
    /// </summary>
    public enum ImageFormat : byte
    {
        /// <summary>
        ///     The default format for the specific image.
        /// </summary>
        Default,

        /// <summary>
        ///     The PNG format.
        /// </summary>
        Png,

        /// <summary>
        ///     The JPG (JPEG) format.
        /// </summary>
        Jpg,

        /// <summary>
        ///     The WebP format.
        /// </summary>
        WebP,

        /// <summary>
        ///     The GIF format.
        /// </summary>
        Gif
    }
}