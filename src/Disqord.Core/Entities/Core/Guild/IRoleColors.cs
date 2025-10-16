namespace Disqord;

/// <summary>
///     Represents a guild role's color(s).
/// </summary>
public interface IRoleColors
{
    /// <summary>
    ///     Gets the primary color of this role.
    /// </summary>
    Color PrimaryColor { get; }
    
    /// <summary>
    ///     Gets the secondary color of this role.
    /// </summary>
    /// <returns>
    ///     The secondary color of the role or <see langword="null"/> if the role has no secondary color.
    /// </returns>
    Color? SecondaryColor { get; }
    
    /// <summary>
    ///     Gets the tertiary color of this role.
    /// </summary>
    /// <returns>
    ///     The tertiary color of the role or <see langword="null"/> if the role has no tertiary color.
    /// </returns>
    Color? TertiaryColor { get; }
}