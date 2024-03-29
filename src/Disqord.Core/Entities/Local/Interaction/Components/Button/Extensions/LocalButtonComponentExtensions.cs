﻿using System.ComponentModel;

namespace Disqord;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class LocalButtonComponentExtensions
{
    public static TComponent WithStyle<TComponent>(this TComponent component, LocalButtonComponentStyle style)
        where TComponent : LocalButtonComponent
    {
        component.Style = style;
        return component;
    }
}
