using System.Collections.Generic;
using System.Linq;
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

        public static TResponse WithTitle<TResponse>(this TResponse @this, string title)
            where TResponse : LocalInteractionModalResponse
        {
            Guard.IsNotNull(title);
            @this.Title = title;
            return @this;
        }

        public static TResponse AddComponent<TResponse>(this TResponse @this, LocalComponent component)
            where TResponse : LocalInteractionModalResponse
        {
            Guard.IsNotNull(component);

            if (!@this.Components.Add(component, out var list))
                @this.Components = new(list);

            return @this;
        }

        public static TResponse WithComponents<TResponse>(this TResponse @this, IEnumerable<LocalComponent> components)
            where TResponse : LocalInteractionModalResponse
        {
            Guard.IsNotNull(components);

            if (!@this.Components.With(components, out var list))
                @this.Components = new(list);

            return @this;
        }

        public static TResponse WithComponents<TResponse>(this TResponse @this, params LocalComponent[] components)
            where TResponse : LocalInteractionModalResponse
            => @this.WithComponents(components as IEnumerable<LocalComponent>);
    }
}
