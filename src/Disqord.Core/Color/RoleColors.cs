namespace Disqord;

public readonly struct RoleColors
{
    public const int HolographicRolePrimaryColor = 11127295;
    public const int HolographicRoleSecondaryColor = 16759788;
    public const int HolographicRoleTertiaryColor = 16761760;
    
    public Color PrimaryColor { get; }
    
    public Color? SecondaryColor { get; }
    
    public Color? TertiaryColor { get; }

    public RoleColors(Color primaryColor, Color? secondaryColor = null)
    {
        PrimaryColor = primaryColor;
        SecondaryColor = secondaryColor;
    }

    public RoleColors(Color primaryColor, Color? secondaryColor, Color? tertiaryColor)
    {
        PrimaryColor = primaryColor;
        SecondaryColor = secondaryColor;
        TertiaryColor = tertiaryColor;
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
    ///     Creates a gradient, triple role color.
    /// </summary>
    /// <returns> The output <see cref="RoleColors"/>. </returns>
    public static RoleColors Holographic => new(HolographicRolePrimaryColor, HolographicRoleSecondaryColor, HolographicRoleTertiaryColor);

    public static implicit operator RoleColors(Color color) => Solid(color);
}