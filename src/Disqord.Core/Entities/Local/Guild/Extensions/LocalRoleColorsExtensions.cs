using System.ComponentModel;
using Qommon;

namespace Disqord;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class LocalRoleColorsExtensions
{
    public static TRoleColors WithPrimaryColor<TRoleColors>(this TRoleColors roleColors, Color? primaryColor)
        where TRoleColors : LocalRoleColors
    {
        roleColors.PrimaryColor = Optional.FromNullable(primaryColor);
        return roleColors;
    }
    
    public static TRoleColors WithSecondaryColor<TRoleColors>(this TRoleColors roleColors, Color? secondaryColor)
        where TRoleColors : LocalRoleColors
    {
        roleColors.SecondaryColor = secondaryColor;
        return roleColors;
    }
    
    public static TRoleColors WithTertiaryColor<TRoleColors>(this TRoleColors roleColors, Color? tertiaryColor)
        where TRoleColors : LocalRoleColors
    {
        roleColors.TertiaryColor = tertiaryColor;
        return roleColors;
    }
}