namespace Disqord
{
    public static class LocalInteractiveComponentExtensions
    {
        public static TComponent WithCustomId<TComponent>(this TComponent component, string customId)
            where TComponent : ILocalInteractiveComponent
        {
            component.CustomId = customId;
            return component;
        }
    }
}
