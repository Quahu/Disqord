using System.Collections.Generic;
using System.ComponentModel;
using Qommon;

namespace Disqord;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class LocalInteractionResponseExtensions
{
    public static TResponse AddChoice<TResponse>(this TResponse response, KeyValuePair<string, object> choice)
        where TResponse : LocalInteractionAutoCompleteResponse
    {
        if (response.Choices.Add(choice, out var list))
            response.Choices = new(list);

        return response;
    }

    public static TResponse WithChoices<TResponse>(this TResponse response, IEnumerable<KeyValuePair<string, object>> choices)
        where TResponse : LocalInteractionAutoCompleteResponse
    {
        Guard.IsNotNull(choices);

        if (response.Choices.With(choices, out var list))
            response.Choices = new(list);

        return response;
    }

    public static TResponse WithChoices<TResponse>(this TResponse response, params KeyValuePair<string, object>[] choices)
        where TResponse : LocalInteractionAutoCompleteResponse
    {
        return response.WithChoices(choices as IEnumerable<KeyValuePair<string, object>>);
    }

    public static TResponse WithTitle<TResponse>(this TResponse response, string title)
        where TResponse : LocalInteractionModalResponse
    {
        Guard.IsNotNull(title);
        response.Title = title;
        return response;
    }

    public static TResponse AddComponent<TResponse>(this TResponse response, LocalComponent component)
        where TResponse : LocalInteractionModalResponse
    {
        Guard.IsNotNull(component);

        if (response.Components.Add(component, out var list))
            response.Components = new(list);

        return response;
    }

    public static TResponse WithComponents<TResponse>(this TResponse response, IEnumerable<LocalComponent> components)
        where TResponse : LocalInteractionModalResponse
    {
        Guard.IsNotNull(components);

        if (response.Components.With(components, out var list))
            response.Components = new(list);

        return response;
    }

    public static TResponse WithComponents<TResponse>(this TResponse response, params LocalComponent[] components)
        where TResponse : LocalInteractionModalResponse
    {
        return response.WithComponents(components as IEnumerable<LocalComponent>);
    }
}
