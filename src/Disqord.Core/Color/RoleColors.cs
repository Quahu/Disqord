using System;

namespace Disqord;

/// <summary>
///     Represents one or more colors assigned to a guild role.
/// </summary>
public readonly struct RoleColors : IEquatable<RoleColors>
{
    private const int HolographicRolePrimaryColor = 11127295;
    private const int HolographicRoleSecondaryColor = 16759788;
    private const int HolographicRoleTertiaryColor = 16761760;
    
    /// <summary>
    ///     Gets the primary color of the role.
    /// </summary>
    public Color PrimaryColor { get; }
    
    /// <summary>
    ///     Gets the secondary color of the role.
    /// </summary>
    /// <returns> The secondary color of the role, or <see langword="null"/> if the role does not have a secondary color. </returns>
    public Color? SecondaryColor { get; }
    
    /// <summary>
    ///     Gets the tertiary color of the role.
    /// </summary>
    /// <returns> The tertiary color of the role, or <see langword="null"/> if the role does not have a tertiary color. </returns>
    public Color? TertiaryColor { get; }

    /// <summary>
    ///     Instantiates a new <see cref="RoleColors"/> with the given primary and (optional) secondary color.
    /// </summary>
    /// <param name="primaryColor"> The primary color for the role. </param>
    /// <param name="secondaryColor"> The (optional) secondary color for the role. </param>
    public RoleColors(Color primaryColor, Color? secondaryColor = null)
    {
        PrimaryColor = primaryColor;
        SecondaryColor = secondaryColor;
    }

    /// <summary>
    ///     Instantiates a new <see cref="RoleColors"/> with the given primary, secondary, and tertiary color.
    /// </summary>
    /// <param name="primaryColor"> The primary color for the role. </param>
    /// <param name="secondaryColor"> The secondary color for the role. </param>
    /// <param name="tertiaryColor"> The tertiary color for the role. </param>
    public RoleColors(Color primaryColor, Color? secondaryColor, Color? tertiaryColor)
    {
        PrimaryColor = primaryColor;
        SecondaryColor = secondaryColor;
        TertiaryColor = tertiaryColor;
    }
    
    /// <inheritdoc />
    public bool Equals(RoleColors other)
    {
        return PrimaryColor.Equals(other.PrimaryColor) && Nullable.Equals(SecondaryColor, other.SecondaryColor) && Nullable.Equals(TertiaryColor, other.TertiaryColor);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is RoleColors other && Equals(other);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(PrimaryColor, SecondaryColor, TertiaryColor);
    }
    
    /// <summary>
    ///     Creates a solid, single role color.
    /// </summary>
    /// <param name="primaryColor"> The primary color for the role. </param>
    /// <returns> The output <see cref="RoleColors"/>. </returns>
    public static RoleColors Solid(Color primaryColor) => new(primaryColor);

    /// <summary>
    ///     Creates a gradient, double role color.
    /// </summary>
    /// <param name="primaryColor"> The primary color for the role. </param>
    /// <param name="secondaryColor"> The secondary color for the role. </param>
    /// <returns> The output <see cref="RoleColors"/>. </returns>
    public static RoleColors Gradient(Color primaryColor, Color secondaryColor) => new(primaryColor, secondaryColor);

    /// <summary>
    ///     Creates a holographic, triple role color.
    /// </summary>
    /// <returns> The output <see cref="RoleColors"/>. </returns>
    public static RoleColors Holographic => new(HolographicRolePrimaryColor, HolographicRoleSecondaryColor, HolographicRoleTertiaryColor);

    public static implicit operator RoleColors(Color color) => Solid(color);
}