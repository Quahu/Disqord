namespace Disqord
{
    /// <summary>
    ///     Represents the file format of a sticker.
    /// </summary>
    public enum StickerFormatType : byte
    {
        /// <summary>
        ///     Represents the PNG file format.
        /// </summary>
        Png = 1,

        /// <summary>
        ///     Represents the APNG file format.
        /// </summary>
        APng = 2,

        /// <summary>
        ///     Represents the Lottie file format.
        /// </summary>
        Lottie = 3
    }
}
