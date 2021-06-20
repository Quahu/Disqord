namespace Disqord.Extensions.Interactivity.Menus
{
    /// <summary>
    ///     Marks a method as a <see cref="ButtonViewComponent"/>.
    /// </summary>
    public class ButtonAttribute : ButtonBaseAttribute
    {
        public ButtonComponentStyle Style { get; init; } = ButtonComponentStyle.Primary;
    }
}
