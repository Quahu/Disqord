namespace Disqord
{
    /// <summary>
    ///     Represents a type that can be tagged.
    /// </summary>
    /// <remarks>
    ///     Example tags:
    ///     <list type="bullet">
    ///         <item>
    ///             <term> User </term>
    ///             <description> <c>Clyde#0000</c> </description>
    ///         </item>
    ///         <item>
    ///             <term> Text Channel </term>
    ///             <description> <c>#general</c> </description>
    ///         </item>
    ///         <item>
    ///             <term> Guild Emoji </term>
    ///             <description> <c>&lt;:professor:667582610431803437&gt;</c> </description>
    ///         </item>
    ///     </list>
    /// </remarks>
    public interface ITaggable
    {
        /// <summary>
        ///     Gets the tag of this object.
        /// </summary>
        /// <remarks>
        ///     <inheritdoc cref="ITaggable"/>
        /// </remarks>
        string Tag { get; }
    }
}
