namespace Disqord;

public static class LocalTextDisplayComponentExtensions
{
    public static TTextDisplayComponent WithContent<TTextDisplayComponent>(this TTextDisplayComponent textDisplayComponent, string content)
        where TTextDisplayComponent : LocalTextDisplayComponent
    {
        textDisplayComponent.Content = content;
        return textDisplayComponent;
    }
}
