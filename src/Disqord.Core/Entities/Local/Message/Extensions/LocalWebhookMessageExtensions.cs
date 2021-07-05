namespace Disqord
{
    public static class LocalWebhookMessageExtensions
    {
        public static TMessage WithName<TMessage>(this TMessage message, string name)
            where TMessage : LocalWebhookMessage
        {
            message.Name = name;
            return message;
        }

        public static TMessage WithAvatarUrl<TMessage>(this TMessage message, string avatarUrl)
            where TMessage : LocalWebhookMessage
        {
            message.AvatarUrl = avatarUrl;
            return message;
        }
    }
}
