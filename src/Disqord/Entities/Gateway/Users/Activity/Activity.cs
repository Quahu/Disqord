using System;
using Disqord.Models;

namespace Disqord
{
    /// <summary>
    ///     Represents a user's activity.
    /// </summary>
    public class Activity
    {
        public virtual string Name { get; }

        public ActivityType Type { get; }

        public DateTimeOffset CreatedAt { get; }

        internal Activity(ActivityModel model)
        {
            Name = model.Name;
            Type = model.Type;
            CreatedAt = DateTimeOffset.FromUnixTimeMilliseconds(model.CreatedAt.Value);
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

                case ActivityType.Streaming:
                    return new StreamingActivity(model);

                case ActivityType.Listening when model.SyncId != null && model.SessionId != null:
                    return new SpotifyActivity(model);

                case ActivityType.Custom:
                    return new CustomActivity(model);

                default:
                    return new RichActivity(model);
            }
        }
    }
}
