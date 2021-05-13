namespace Disqord
{
    /// <summary>
    ///     Represents a type that can be mentioned.
    ///     E.g. a user (<c>&lt;@183319356489465856&gt;</c>).
    /// </summary>
    public interface IMentionable
    {
        /// <summary>
        ///     Gets the mention of this object.
        /// </summary>
        string Mention { get; }
    }
}
