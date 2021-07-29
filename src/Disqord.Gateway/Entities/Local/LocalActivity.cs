namespace Disqord.Gateway
{
    public class LocalActivity
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

        public string Name { get; }

        public string Url { get; }

        public ActivityType Type { get; }

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
    }
}
