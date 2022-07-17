using Disqord.Gateway;

namespace Disqord.Extensions.Interactivity.Menus;

public sealed class ButtonEventArgs : ViewComponentEventArgs
{
    public ButtonViewComponent Button { get; }

    public ButtonEventArgs(ButtonViewComponent button, InteractionReceivedEventArgs e)
        : base(e)
    {
        Button = button;
    }
}