using Disqord.Gateway;

namespace Disqord.Extensions.Interactivity.Menus;

public class ViewComponentEventArgs : InteractionReceivedEventArgs
{
    public override IComponentInteraction Interaction => (base.Interaction as IComponentInteraction)!;

    public ViewComponentEventArgs(InteractionReceivedEventArgs e)
        : base(e.Interaction, e.Member)
    { }
}