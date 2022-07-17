using System;

namespace Disqord.Extensions.Interactivity.Menus;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
public abstract class ComponentAttribute : Attribute
{
    /// <summary>
    ///     Gets or sets the zero-indexed row the component should appear on.
    ///     If <c>-1</c>, the component's row will be determined automatically.
    /// </summary>
    /// <exception cref="ArgumentException"> Thrown when the value is negative or higher than 4. </exception>
    public int Row { get; init; } = -1;

    /// <summary>
    ///     Gets or sets the zero-indexed position the component should appear in on the row.
    ///     If <c>-1</c>, the component's position will be determined automatically.
    /// </summary>
    /// <exception cref="ArgumentException"> Thrown when the value is negative or higher than 4. </exception>
    public int Position { get; init; } = -1;

    public bool IsDisabled { get; init; }

    protected ComponentAttribute()
    { }
}