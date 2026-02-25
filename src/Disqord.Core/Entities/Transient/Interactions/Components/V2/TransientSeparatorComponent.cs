using Disqord.Models;
using Qommon;

namespace Disqord;

public class TransientSeparatorComponent(SeparatorComponentJsonModel model)
    : TransientBaseComponent<SeparatorComponentJsonModel>(model), ISeparatorComponent
{
    public bool IsDivider => Model.Divider.GetValueOrDefault(true);

    public SeparatorComponentSpacingSize SpacingSize => Model.Spacing.GetValueOrDefault();
}
