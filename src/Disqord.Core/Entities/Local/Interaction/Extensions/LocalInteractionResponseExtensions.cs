using System.Collections.Generic;
using Qommon;

namespace Disqord
{
    public static class LocalInteractionResponseExtensions
    {
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

            if (!@this.Choices.Add(choice, out var list))
                @this.Choices = new(list);

            return @this;
        }

        public static TResponse WithChoices<TResponse>(this TResponse @this, IEnumerable<LocalSlashCommandOptionChoice> choices)
            where TResponse : LocalInteractionAutoCompleteResponse
        {
            Guard.IsNotNull(choices);

            if (!@this.Choices.With(choices, out var list))
                @this.Choices = new(list);

            return @this;
        }

        public static TResponse WithChoices<TResponse>(this TResponse @this, params LocalSlashCommandOptionChoice[] choices)
            where TResponse : LocalInteractionAutoCompleteResponse
            => @this.WithChoices(choices as IEnumerable<LocalSlashCommandOptionChoice>);
    }
}
