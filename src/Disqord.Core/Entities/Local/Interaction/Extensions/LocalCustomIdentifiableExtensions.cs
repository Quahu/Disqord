namespace Disqord
{
    public static class LocalCustomIdentifiableExtensions
    {
        public static TCustomIdentifiable WithCustomId<TCustomIdentifiable>(this TCustomIdentifiable localEntity, string customId)
            where TCustomIdentifiable : ILocalCustomIdentifiable
        {
            localEntity.CustomId = customId;
            return localEntity;
        }
    }
}
