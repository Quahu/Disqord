using System;

namespace Disqord.Gateway
{
    public class LocalActivity : ILocalConstruct
    {
        public static LocalActivity Playing(string name)
            => new(name, ActivityType.Playing);

        public static LocalActivity Streaming(string name, string url)
            => new(name, url);

        public static LocalActivity Listening(string name)
            => new(name, ActivityType.Listening);

        public static LocalActivity Watching(string name)
            => new(name, ActivityType.Watching);

        public static LocalActivity Competing(string name)
            => new(name, ActivityType.Competing);

        public string Name { get; init; }

        public string Url { get; init; }

        public ActivityType Type { get; init; }

        public LocalActivity()
        { }

        public LocalActivity(string name, ActivityType type, string url = null)
        {
            Name = name;
            Url = url;
            Type = type;
        }

        public LocalActivity(string name, string url)
        {
            Name = name;
            Url = url;
            Type = ActivityType.Streaming;
        }

        public LocalActivity Clone()
            => MemberwiseClone() as LocalActivity;

        object ICloneable.Clone()
            => Clone();
    }
}
