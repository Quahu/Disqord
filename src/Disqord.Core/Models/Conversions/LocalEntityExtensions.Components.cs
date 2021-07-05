using System;
using System.Linq;

namespace Disqord.Models
{
    public static partial class LocalEntityExtensions
    {
        public static ComponentJsonModel ToModel(this LocalComponent component)
        {
            if (component == null)
                return null;

            var model = new ComponentJsonModel();
            if (component is LocalRowComponent rowComponent)
            {
                model.Type = ComponentType.Row;
                model.Components = rowComponent.Components.Select(x => x.ToModel()).ToArray();
            }
            else if (component is LocalNestedComponent nestedComponent)
            {
                model.Disabled = Optional.Conditional(nestedComponent.IsDisabled, true);
                if (component is ILocalInteractiveComponent interactiveComponent)
                    model.CustomId = interactiveComponent.CustomId;

                if (component is LocalButtonComponentBase buttonComponentBase)
                {
                    model.Type = ComponentType.Button;
                    model.Label = Optional.FromNullable(buttonComponentBase.Label);
                    model.Emoji = Optional.FromNullable(buttonComponentBase.Emoji.ToModel());

                    if (buttonComponentBase is LocalButtonComponent buttonComponent)
                    {
                        model.Style = (ButtonComponentStyle) buttonComponent.Style;
                    }
                    else if (buttonComponentBase is LocalLinkButtonComponent linkButtonComponent)
                    {
                        model.Style = ButtonComponentStyle.Link;
                        model.Url = linkButtonComponent.Url;
                    }
                    else
                    {
                        throw new InvalidOperationException("Unknown local button component type.");
                    }
                }
                else if (component is LocalSelectionComponent selectionComponent)
                {
                    model.Type = ComponentType.Selection;
                    model.Placeholder = Optional.FromNullable(selectionComponent.Placeholder);
                    model.MinValues = Optional.FromNullable(selectionComponent.MinimumSelectedOptions);
                    model.MaxValues = Optional.FromNullable(selectionComponent.MaximumSelectedOptions);
                    model.Options = selectionComponent.Options.Select(x => x.ToModel()).ToArray();
                }
                else
                {
                    throw new InvalidOperationException("Unknown local nested component type.");
                }
            }
            else
            {
                throw new InvalidOperationException("Unknown local component type.");
            }

            return model;
        }

        public static SelectOptionJsonModel ToModel(this LocalSelectionComponentOption option)
            => new()
            {
                Label = option.Label,
                Value = option.Value,
                Description = Optional.FromNullable(option.Description),
                Emoji = Optional.FromNullable(option.Emoji.ToModel()),
                Default = Optional.Conditional(option.IsDefault, true)
            };
    }
}
