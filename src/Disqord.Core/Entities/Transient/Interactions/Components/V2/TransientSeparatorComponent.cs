using Disqord.Models;
using Qommon;

namespace Disqord;

public class TransientSeparatorComponent(IClient client, SeparatorComponentJsonModel model)
    : TransientBaseComponent<SeparatorComponentJsonModel>(client, model), ISeparatorComponent
{
    public bool IsDivider => Model.Divider.GetValueOrDefault(true);

    public SeparatorComponentSpacingSize SpacingSize => Model.Spacing.GetValueOrDefault();
}
