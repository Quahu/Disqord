using System;

namespace Disqord.Extensions.Interactivity.Menus;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class SelectionOptionAttribute : Attribute
{
    public string? Label { get; }

    public string? Value { get; init; }

    public string? Description { get; init; }

    public object? Emoji { get; init; }

    public bool IsDefault { get; init; }

    public SelectionOptionAttribute(string label)
    {
        Label = label;
    }
}