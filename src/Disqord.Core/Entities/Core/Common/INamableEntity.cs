namespace Disqord;

/// <summary>
///     Represents an entity with a name.
/// </summary>
/// <remarks>
///     Example names:
///     <list type="bullet">
///         <item>
///             <term> User </term>
///             <description> <c>Clyde</c> </description>
///         </item>
///         <item>
///             <term> Text Channel </term>
///             <description> <c>general</c> </description>
///         </item>
///         <item>
///             <term> Role </term>
///             <description> <c>everyone</c> </description>
///         </item>
///     </list>
/// </remarks>
public interface INamableEntity : IPossiblyNamableEntity
{
    /// <summary>
    ///     Gets the name of this entity.
    /// </summary>
    new string Name { get; }

    string IPossiblyNamableEntity.Name => Name;
}
