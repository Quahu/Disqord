using System;
using System.Linq;
using Qommon;

namespace Disqord.Models
{
    public static partial class LocalEntityExtensions
    {
        public static ComponentJsonModel ToModel(this LocalComponent component)
        {
            if (component == null)
                return null;

            var model = new ComponentJsonModel();

            if (component is ILocalCustomIdentifiableEntity customIdentifiableEntity)
                model.CustomId = customIdentifiableEntity.CustomId;

            if (component is LocalRowComponent rowComponent)
            {
                model.Type = ComponentType.Row;
                model.Components = rowComponent.Components.Select(x => x.ToModel()).ToArray();
            }
            else if (component is LocalButtonComponentBase buttonComponentBase)
            {
                model.Type = ComponentType.Button;
                model.Label = buttonComponentBase.Label;
                model.Emoji = Optional.Convert(buttonComponentBase.Emoji, emoji => emoji.ToModel());
                model.Disabled = buttonComponentBase.IsDisabled;

                if (buttonComponentBase is LocalButtonComponent buttonComponent)
                {
                    model.Style = Optional.Convert(buttonComponent.Style, style => (byte) style);
                }
                else if (buttonComponentBase is LocalLinkButtonComponent linkButtonComponent)
                {
                    model.Style = (byte) ButtonComponentStyle.Link;
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
                model.Placeholder = selectionComponent.Placeholder;
                model.MinValues = selectionComponent.MinimumSelectedOptions;
                model.MaxValues = selectionComponent.MaximumSelectedOptions;
                model.Disabled = selectionComponent.IsDisabled;
                model.Options = selectionComponent.Options.Select(x => x.ToModel()).ToArray();
            }
            else if (component is LocalTextInputComponent textInputComponent)
            {
                model.Type = ComponentType.TextInput;
                model.Style = Optional.Convert(textInputComponent.Style, style => (byte) style);
                model.CustomId = textInputComponent.CustomId;
                model.Label = textInputComponent.Label;
                model.MinLength = textInputComponent.MinimumInputLength;
                model.MaxLength = textInputComponent.MaximumInputLength;
                model.Required = textInputComponent.IsRequired;
                model.Value = textInputComponent.PrefilledValue;
                model.Placeholder = textInputComponent.Placeholder;
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
                Description = option.Description,
                Emoji = Optional.Convert(option.Emoji, emoji => emoji.ToModel()),
                Default = option.IsDefault
            };
    }
}
