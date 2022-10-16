using System.Collections.Generic;
using System.ComponentModel;
using Qommon;

namespace Disqord;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class LocalSelectionComponentExtensions
{
    public static TComponent WithType<TComponent>(this TComponent selection, SelectionComponentType type)
        where TComponent : LocalSelectionComponent
    {
        selection.Type = type;
        return selection;
    }

    public static TComponent AddChannelType<TComponent>(this TComponent @this, ChannelType channelType)
        where TComponent : LocalSelectionComponent
    {
        Guard.IsDefined(channelType);

        if (@this.ChannelTypes.Add(channelType, out var list))
            @this.ChannelTypes = new(list);

        return @this;
    }

    public static TComponent WithChannelTypes<TComponent>(this TComponent @this, IEnumerable<ChannelType> channelTypes)
        where TComponent : LocalSelectionComponent
    {
        Guard.IsNotNull(channelTypes);

        if (@this.ChannelTypes.With(channelTypes, out var list))
            @this.ChannelTypes = new(list);

        return @this;
    }

    public static TComponent WithChannelTypes<TComponent>(this TComponent @this, params ChannelType[] channelTypes)
        where TComponent : LocalSelectionComponent
    {
        return @this.WithChannelTypes(channelTypes as IEnumerable<ChannelType>);
    }

    public static TComponent WithPlaceholder<TComponent>(this TComponent selection, string placeholder)
        where TComponent : LocalSelectionComponent
    {
        selection.Placeholder = placeholder;
        return selection;
    }

    public static TComponent WithMinimumSelectedOptions<TComponent>(this TComponent selection, int minimumSelectedOptions)
        where TComponent : LocalSelectionComponent
    {
        selection.MinimumSelectedOptions = minimumSelectedOptions;
        return selection;
    }

    public static TComponent WithMaximumSelectedOptions<TComponent>(this TComponent selection, int maximumSelectedOptions)
        where TComponent : LocalSelectionComponent
    {
        selection.MaximumSelectedOptions = maximumSelectedOptions;
        return selection;
    }

    public static TComponent WithIsDisabled<TComponent>(this TComponent selection, bool isDisabled = true)
        where TComponent : LocalSelectionComponent
    {
        selection.IsDisabled = isDisabled;
        return selection;
    }

    public static TComponent AddOption<TComponent>(this TComponent selection, LocalSelectionComponentOption option)
        where TComponent : LocalSelectionComponent
    {
        if (selection.Options.Add(option, out var list))
            selection.Options = new(list);

        return selection;
    }

    public static TComponent WithOptions<TComponent>(this TComponent selection, IEnumerable<LocalSelectionComponentOption> options)
        where TComponent : LocalSelectionComponent
    {
        Guard.IsNotNull(options);

        if (selection.Options.With(options, out var list))
            selection.Options = new(list);

        return selection;
    }

    public static TComponent WithOptions<TComponent>(this TComponent selection, params LocalSelectionComponentOption[] options)
        where TComponent : LocalSelectionComponent
    {
        return selection.WithOptions(options as IEnumerable<LocalSelectionComponentOption>);
    }
}
