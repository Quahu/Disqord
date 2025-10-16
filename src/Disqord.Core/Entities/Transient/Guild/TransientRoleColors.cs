using Disqord.Models;

namespace Disqord;

public class TransientRoleColors : TransientEntity<RoleColorsJsonModel>, IRoleColors
{
    /// <inheritdoc/>
    public Color PrimaryColor { get; }
    
    /// <inheritdoc/>
    public Color? SecondaryColor { get; }
    
    /// <inheritdoc/>
    public Color? TertiaryColor { get; }
    
    public TransientRoleColors(RoleColorsJsonModel model) 
        : base(model)
    {
        PrimaryColor = model.PrimaryColor;
        SecondaryColor = model.SecondaryColor;
        TertiaryColor = model.TertiaryColor;
    }
}