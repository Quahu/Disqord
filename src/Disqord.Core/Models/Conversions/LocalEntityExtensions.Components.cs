using System;
using System.Linq;

namespace Disqord.Models
{
    public static partial class LocalEntityExtensions
    {
        public static ComponentJsonModel ToModel(this LocalComponent component)
        {
            return component switch
            {
                LocalRowComponent row => new ComponentJsonModel
                {
                    Type = ComponentType.Row,
                    Components = row.Components.Select(x => x.ToModel()).ToArray()
                },
                LocalButtonComponent button => new ComponentJsonModel
                {
                    Type = ComponentType.Button,
                    Style = button.Style,
                    Label = button.Label,
                    Emoji = Optional.FromNullable(button.Emoji?.ToModel()),
                    CustomId = Optional.FromNullable(button.CustomId),
                    Url = Optional.FromNullable(button.Url),
                    Disabled = Optional.Conditional(button.IsDisabled, true)
                },
                _ => throw new InvalidOperationException("Unknown local component type.")
            };
        }
    }
}
