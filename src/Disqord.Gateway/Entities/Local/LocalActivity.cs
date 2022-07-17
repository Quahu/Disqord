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

    public static LocalActivity Competing(string name)
    {
        return new(name, ActivityType.Competing);
    }

    public Optional<string> Name { get; set; }

    public Optional<string> Url { get; set; }

    public Optional<ActivityType> Type { get; set; }

    public LocalActivity()
    { }

    protected LocalActivity(LocalActivity other)
    {
        Name = other.Name;
        Url = other.Url;
        Type = other.Type;
    }

    public LocalActivity(string name, ActivityType type, string? url = null)
    {
        Name = name;
        Url = Optional.FromNullable(url);
        Type = type;
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
            Url = Url
        };
    }
}
