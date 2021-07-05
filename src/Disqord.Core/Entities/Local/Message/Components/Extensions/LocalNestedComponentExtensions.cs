namespace Disqord
{
    public static class LocalNestedComponentExtensions
    {
        public static TComponent WithIsDisabled<TComponent>(this TComponent component, bool isDisabled = true)
            where TComponent : LocalNestedComponent
        {
            component.IsDisabled = isDisabled;
            return component;
        }
    }
}
