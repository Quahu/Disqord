using Disqord.Gateway.Api.Models;
using Disqord.Models;
using Qommon;

namespace Disqord.Gateway;

public class LocalActivity : ILocalConstruct<LocalActivity>, IJsonConvertible<ActivityJsonModel>
{
    public static LocalActivity Playing(string name)
    {
        return new(name, ActivityType.Playing);
    }

    public static LocalActivity Streaming(string name, string url)
    {
        return new(name, url);
    }

    public static LocalActivity Listening(string name)
    {
        return new(name, ActivityType.Listening);
    }

    public static LocalActivity Watching(string name)
    {
        return new(name, ActivityType.Watching);
    }

    public static LocalActivity Custom(string text)
    {
        return new("Custom Status", ActivityType.Custom, text: text);
    }

    public static LocalActivity Competing(string name)
    {
        return new(name, ActivityType.Competing);
    }

    public Optional<string> Name { get; set; }

    public Optional<string> Url { get; set; }

    public Optional<ActivityType> Type { get; set; }

    public Optional<string> Text { get; set; }

    public LocalActivity()
    { }

    protected LocalActivity(LocalActivity other)
    {
        Name = other.Name;
        Url = other.Url;
        Type = other.Type;
        Text = other.Text;
    }

    public LocalActivity(string name, ActivityType type, string? url = null, string? text = null)
    {
        Name = name;
        Url = Optional.FromNullable(url);
        Type = type;
        Text = Optional.FromNullable(text);
    }

    public LocalActivity(string name, string url)
    {
        Name = name;
        Url = url;
        Type = ActivityType.Streaming;
    }

    public LocalActivity Clone()
    {
        return new(this);
    }

    /// <inheritdoc />
    public virtual ActivityJsonModel ToModel()
    {
        OptionalGuard.HasValue(Name);
        OptionalGuard.HasValue(Type);

        return new ActivityJsonModel
        {
            Name = Name.Value,
            Type = Type.Value,
            Url = Url,
            State = Text
        };
    }
}
