using Disqord.Entities.Local;

namespace Disqord.Gateway
{
    public class LocalActivity : ILocalEntity
    {
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
