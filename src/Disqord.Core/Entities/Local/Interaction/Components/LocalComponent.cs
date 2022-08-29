using System;
using System.Linq;
using Disqord.Models;
using Qommon;

namespace Disqord;

public abstract class LocalComponent : ILocalConstruct<LocalComponent>, IJsonConvertible<ComponentJsonModel>
{
    public static LocalRowComponent Row(params LocalComponent[] components)
    {
        return new LocalRowComponent
        {
            Components = components
        };
    }

    public static LocalButtonComponent Button(string customId, string label)
    {
        return new LocalButtonComponent
        {
            CustomId = customId,
            Label = label
        };
    }

    public static LocalButtonComponent Button(string customId, LocalEmoji emoji)
    {
        return new LocalButtonComponent
        {
            CustomId = customId,
            Emoji = emoji
        };
    }

    public static LocalLinkButtonComponent LinkButton(string url, string label)
    {
        return new LocalLinkButtonComponent
        {
            Url = url,
            Label = label
        };
    }

    public static LocalLinkButtonComponent LinkButton(string url, LocalEmoji emoji)
    {
        return new LocalLinkButtonComponent
        {
            Url = url,
            Emoji = emoji
        };
    }

    public static LocalSelectionComponent Selection(string customId, params LocalSelectionComponentOption[] options)
    {
        return new LocalSelectionComponent
        {
            CustomId = customId,
            Options = options
        };
    }

    public static LocalTextInputComponent TextInput(string customId, string label, TextInputComponentStyle style)
    {
        return new LocalTextInputComponent
        {
            Style = style,
            CustomId = customId,
            Label = label
        };
    }

    /// <inheritdoc/>
    public abstract LocalComponent Clone();

    /// <inheritdoc/>
    public virtual ComponentJsonModel ToModel()
    {
        // TODO: maybe split this via inheritance
        var model = new ComponentJsonModel();

        if (this is ILocalCustomIdentifiableEntity customIdentifiableEntity)
            model.CustomId = customIdentifiableEntity.CustomId;

        if (this is LocalRowComponent rowComponent)
        {
            model.Type = ComponentType.Row;
            model.Components = Optional.Convert(rowComponent.Components, components => components.Select(component => component.ToModel()).ToArray());
        }
        else if (this is LocalButtonComponentBase buttonComponentBase)
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
        else if (this is LocalSelectionComponent selectionComponent)
        {
            model.Type = ComponentType.Selection;
            model.Placeholder = selectionComponent.Placeholder;
            model.MinValues = selectionComponent.MinimumSelectedOptions;
            model.MaxValues = selectionComponent.MaximumSelectedOptions;
            model.Disabled = selectionComponent.IsDisabled;
            model.Options = Optional.Convert(selectionComponent.Options, options => options.Select(option => option.ToModel()).ToArray());
        }
        else if (this is LocalTextInputComponent textInputComponent)
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

    /// <summary>
    ///     Converts the specified component to a <see cref="LocalComponent"/>.
    /// </summary>
    /// <param name="component"> The component to convert. </param>
    /// <returns>
    ///     The output <see cref="LocalComponent"/>.
    /// </returns>
    public static LocalComponent CreateFrom(IComponent component)
    {
        return component switch
        {
            IRowComponent rowComponent => LocalRowComponent.CreateFrom(rowComponent),
            IButtonComponent buttonComponent => LocalButtonComponentBase.CreateFrom(buttonComponent),
            ISelectionComponent selectionComponent => LocalSelectionComponent.CreateFrom(selectionComponent),
            ITextInputComponent textInputComponent => LocalTextInputComponent.CreateFrom(textInputComponent),
            _ => throw new ArgumentException("Unsupported component type.", nameof(component))
        };
    }
}
