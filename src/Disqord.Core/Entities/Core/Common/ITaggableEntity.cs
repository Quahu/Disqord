namespace Disqord;

/// <summary>
///     Represents an entity that can be tagged.
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
public interface ITaggableEntity : IEntity
{
    /// <summary>
    ///     Gets the tag of this entity.
    /// </summary>
    /// <remarks>
    ///     <inheritdoc cref="ITaggableEntity"/>
    /// </remarks>
    string Tag { get; }
}
