using System;
using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway
{
    public class TransientActivity : TransientClientEntity<ActivityJsonModel>, IActivity
    {
        /// <inheritdoc/>
        public virtual string Name => Model.Name;

        /// <inheritdoc/>
        public ActivityType Type => Model.Type;

        /// <inheritdoc/>
        public DateTimeOffset CreatedAt => DateTimeOffset.FromUnixTimeMilliseconds(Model.CreatedAt);

        public TransientActivity(IClient client, ActivityJsonModel model)
            : base(client, model)
        { }

        public override string ToString()
            => $"{Type} {Name}";

        public static IActivity Create(IClient client, ActivityJsonModel model)
            => model.Type switch
            {
                ActivityType.Playing when model.ApplicationId.HasValue || model.SessionId.HasValue => new TransientRichActivity(client, model),
                ActivityType.Playing => new TransientActivity(client, model),
                ActivityType.Streaming when model.Url == null => new TransientRichActivity(client, model),
                ActivityType.Streaming => new TransientStreamingActivity(client, model),
                ActivityType.Listening when model.Id != null && model.Id.StartsWith("spotify:") => new TransientSpotifyActivity(client, model),
                ActivityType.Custom => new TransientCustomActivity(client, model),
                _ => new TransientRichActivity(client, model)
            };
    }
}
