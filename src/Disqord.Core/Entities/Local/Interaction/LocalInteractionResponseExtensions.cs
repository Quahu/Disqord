namespace Disqord
{
    public static class LocalInteractionResponseExtensions
    {
        public static TResponse WithType<TResponse>(this TResponse response, InteractionResponseType type)
            where TResponse : LocalInteractionResponse
        {
            response.Type = type;
            return response;
        }

        public static TResponse WithIsEphemeral<TResponse>(this TResponse response, bool isEphemeral = true)
            where TResponse : LocalInteractionResponse
        {
            response.IsEphemeral = true;
            return response;
        }
    }
}
