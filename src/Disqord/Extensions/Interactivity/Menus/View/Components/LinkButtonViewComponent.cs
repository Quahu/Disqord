using System;
using Qommon;

namespace Disqord.Extensions.Interactivity.Menus;

public class LinkButtonViewComponent : ViewComponent
{
    public string Url
    {
        get => _url;
        set
        {
            _url = value;
            ReportChanges();
        }
    }
    private string _url;

    public string? Label
    {
        get => _label;
        set
        {
            _label = value;
            ReportChanges();
        }
    }
    private string? _label;

    public LocalEmoji? Emoji
    {
        get => _emoji;
        set
        {
            _emoji = value;
            ReportChanges();
        }
    }
    private LocalEmoji? _emoji;

    public bool IsDisabled
    {
        get => _isDisabled;
        set
        {
            _isDisabled = value;
            ReportChanges();
        }
    }
    private bool _isDisabled;

    public override int Width => 1;

    public LinkButtonViewComponent(string url)
    {
        _url = url;
    }

    internal LinkButtonViewComponent(LinkButtonAttribute attribute, string url)
        : base(attribute)
    {
        _url = url;
        _label = attribute.Label;
        _emoji = attribute.Emoji is string emojiString
            ? LocalEmoji.FromString(emojiString)
            : attribute.Emoji != null
                ? LocalEmoji.Custom(Convert.ToUInt64(attribute.Emoji))
                : null;

        _isDisabled = attribute.IsDisabled;
    }

    protected internal override LocalComponent ToLocalComponent()
    {
        var button = new LocalLinkButtonComponent
        {
            Url = _url,
            IsDisabled = _isDisabled,
            Label = Optional.FromNullable(_label),
            Emoji = Optional.FromNullable(_emoji)
        };

        return button;
    }
}