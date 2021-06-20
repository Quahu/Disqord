namespace Disqord
{
    public static class LocalWebhookMessageExtensions
    {
        public static TLocalWebhookMessage WithName<TLocalWebhookMessage>(this TLocalWebhookMessage message, string name)
            where TLocalWebhookMessage : LocalWebhookMessage
        {
            message.Name = name;
            return message;
        }

        public static TLocalWebhookMessage WithAvatarUrl<TLocalWebhookMessage>(this TLocalWebhookMessage message, string avatarUrl)
            where TLocalWebhookMessage : LocalWebhookMessage
        {
            message.AvatarUrl = avatarUrl;
            return message;
        }
    }
}
