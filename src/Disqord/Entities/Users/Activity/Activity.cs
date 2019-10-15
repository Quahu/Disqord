using Disqord.Models;

namespace Disqord
{
    public class Activity
    {
        public virtual string Name { get; }

        public ActivityType Type { get; }

        internal Activity(ActivityModel model)
        {
            Name = model.Name;
            Type = model.Type;
        }

        internal static Activity Create(ActivityModel model)
        {
            switch (model.Type)
            {
                case ActivityType.Playing when model.ApplicationId != null || model.SessionId != null:
                case ActivityType.Streaming when model.Url == null:
                    return new RichActivity(model);

                case ActivityType.Playing:
                    return new Activity(model);

                case ActivityType.Listening when model.SyncId != null && model.SessionId != null:
                    return new SpotifyActivity(model);

                default:
                    return new RichActivity(model);
            }
        }
    }
}
