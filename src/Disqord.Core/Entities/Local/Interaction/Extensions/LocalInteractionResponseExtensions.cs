using System.Collections.Generic;
using Qommon;
using Qommon.Collections;

namespace Disqord
{
    public static class LocalInteractionResponseExtensions
    {
        public static TResponse WithType<TResponse>(this TResponse response, InteractionResponseType type)
            where TResponse : ILocalInteractionResponse
        {
            response.Type = type;
            return response;
        }

        public static TResponse WithIsEphemeral<TResponse>(this TResponse response, bool isEphemeral = true)
            where TResponse : LocalInteractionMessageResponse
        {
            response.IsEphemeral = isEphemeral;
            return response;
        }

        public static TResponse AddChoice<TResponse>(this TResponse @this, LocalSlashCommandOptionChoice choice)
            where TResponse : LocalInteractionAutoCompleteResponse
        {
            Guard.IsNotNull(choice);

            @this.Choices.Add(choice);
            return @this;
        }

        public static TResponse WithChoices<TResponse>(this TResponse @this, IEnumerable<LocalSlashCommandOptionChoice> choices)
            where TResponse : LocalInteractionAutoCompleteResponse
        {
            Guard.IsNotNull(choices);

            @this.Choices.Clear();
            @this.Choices.AddRange(choices);
            return @this;
        }

        public static TResponse WithChoices<TResponse>(this TResponse @this, params LocalSlashCommandOptionChoice[] choices)
            where TResponse : LocalInteractionAutoCompleteResponse
            => @this.WithChoices(choices as IEnumerable<LocalSlashCommandOptionChoice>);
    }
}
