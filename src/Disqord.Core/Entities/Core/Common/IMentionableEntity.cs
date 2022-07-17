namespace Disqord;

/// <summary>
///     Represents a type that can be mentioned.
///     E.g. a user ().
/// </summary>
/// <remarks>
///     Example mentions:
///     <list type="bullet">
///         <item>
///             <term> User </term>
///             <description> <c>&lt;@183319356489465856&gt;</c> </description>
///         </item>
///         <item>
///             <term> Text Channel </term>
///             <description> <c>&lt;#566751130307264522&gt;</c> </description>
///         </item>
///         <item>
///             <term> Role </term>
///             <description> <c>&lt;@&#38;416256456505950215&gt;</c> </description>
///         </item>
///     </list>
/// </remarks>
public interface IMentionableEntity : IEntity
{
    /// <summary>
    ///     Gets the mention of this object.
    /// </summary>
    /// <remarks>
    ///     <inheritdoc cref="IMentionableEntity"/>
    /// </remarks>
    string Mention { get; }
}
