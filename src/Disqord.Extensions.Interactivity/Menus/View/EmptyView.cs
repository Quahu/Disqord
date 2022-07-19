using System;

namespace Disqord.Extensions.Interactivity.Menus;

/// <summary>
///     Represents an empty implementation of <see cref="ViewBase"/>.
/// </summary>
/// <remarks>
///     This type can be used for views that only contain dynamically generated components.
/// </remarks>
public class EmptyView : ViewBase
{
    /// <inheritdoc/>
    /// <summary>
    ///     Instantiates a new <see cref="EmptyView"/> with the specified message template.
    /// </summary>
    public EmptyView(Action<LocalMessageBase> messageTemplate)
        : base(messageTemplate)
    { }
}