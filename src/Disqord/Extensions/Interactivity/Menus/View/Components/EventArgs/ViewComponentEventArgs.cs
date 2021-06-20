using Disqord.Gateway;

namespace Disqord.Extensions.Interactivity.Menus
{
    public class ViewComponentEventArgs : InteractionReceivedEventArgs
    {
        public ViewComponentEventArgs(InteractionReceivedEventArgs e)
            : base(e.Interaction, e.Member)
        { }
    }
}
