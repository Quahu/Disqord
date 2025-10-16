using Disqord.Models;
using Qommon;

namespace Disqord;

/// <summary>
///     Represents local role color(s) to be applied to a new or existing role.
/// </summary>
public class LocalRoleColors : ILocalConstruct<LocalRoleColors>, IJsonConvertible<RoleColorsJsonModel>
{
    public const int HolographicRolePrimaryColor = 11127295;
    public const int HolographicRoleSecondaryColor = 16759788;
    public const int HolographicRoleTertiaryColor = 16761760;
    
    
    /// <summary>
    ///     Creates a solid, single role color.
    /// </summary>
    /// <param name="primaryColor"> The primary color for the role. </param>
    /// <returns> The output <see cref="LocalRoleColors"/>. </returns>
    public static LocalRoleColors Solid(Color primaryColor)
    {
        return new LocalRoleColors
        {
            PrimaryColor = primaryColor
        };
    }
    
    /// <summary>
    ///     Creates a gradient, double role color.
    /// </summary>
    /// <param name="primaryColor"> The primary color for the role. </param>
    /// <param name="secondaryColor"> The secondary color for the role. </param>
    /// <returns> The output <see cref="LocalRoleColors"/>. </returns>
    public static LocalRoleColors Gradient(Color primaryColor, Color secondaryColor)
    {
        return new LocalRoleColors
        {
            PrimaryColor = primaryColor,
            SecondaryColor = secondaryColor
        };
    }

    /// <summary>
    ///     Creates a gradient, triple role color.
    /// </summary>
    /// <returns> The output <see cref="LocalRoleColors"/>. </returns>
    public static LocalRoleColors Holographic
    {
        get
        {
            return new LocalRoleColors
            {
                PrimaryColor = new Color(HolographicRolePrimaryColor),
                SecondaryColor = new Color(HolographicRoleSecondaryColor),
                TertiaryColor = new Color(HolographicRoleTertiaryColor)
            };
        }
    }
    
    /// <summary>
    ///     Gets or set the primary color for the role.
    /// </summary>
    /// <remarks>
    ///     This property is required if <see cref="SecondaryColor"/> and/or <see cref="TertiaryColor"/> are set.
    /// </remarks>
    public Optional<Color> PrimaryColor { get; set; }
    
    /// <summary>
    ///     Gets or set the secondary color for the role.
    /// </summary>
    /// <remarks>
    ///     This property is required if <see cref="TertiaryColor"/> is set.
    /// </remarks>
    public Optional<Color?> SecondaryColor { get; set; }
    
    /// <summary>
    ///     Gets or set the tertiary color for the role.
    /// </summary>
    public Optional<Color?> TertiaryColor { get; set; }
    
    /// <summary>
    ///     Instantiates a new <see cref="LocalRoleColors"/>.
    /// </summary>
    public LocalRoleColors()
    { }

    /// <summary>
    ///     Instantiates a new <see cref="LocalRoleColors"/> with the properties copied from another instance.
    /// </summary>
    /// <param name="other"> The other instance to copy properties from. </param>
    protected LocalRoleColors(LocalRoleColors other)
    {
        PrimaryColor = other.PrimaryColor;
        SecondaryColor = other.SecondaryColor;
        TertiaryColor = other.TertiaryColor;
    }
    
    /// <inheritdoc/>
    public LocalRoleColors Clone()
    {
        return new(this);
    }

    /// <inheritdoc/>
    public RoleColorsJsonModel ToModel()
    {
        if (SecondaryColor.HasValue)
            OptionalGuard.HasValue(PrimaryColor);
        
        if (TertiaryColor.HasValue)
            OptionalGuard.HasValue(SecondaryColor);

        return new RoleColorsJsonModel
        {
            PrimaryColor = PrimaryColor.GetValueOrDefault(),
            SecondaryColor = SecondaryColor.GetValueOrDefault(),
            TertiaryColor = TertiaryColor.GetValueOrDefault()
        };
    }

    /// <summary>
    ///     Converts the specified role colors to a <see cref="LocalRoleColors"/>.
    /// </summary>
    /// <param name="roleColors"> The role colors to convert. </param>
    /// <returns>
    ///     The output <see cref="LocalRoleColors"/>.
    /// </returns>
    public static LocalRoleColors CreateFrom(IRoleColors roleColors)
    {
        return new LocalRoleColors
        {
            PrimaryColor = roleColors.PrimaryColor,
            SecondaryColor = roleColors.SecondaryColor,
            TertiaryColor = roleColors.TertiaryColor
        };
    }
}