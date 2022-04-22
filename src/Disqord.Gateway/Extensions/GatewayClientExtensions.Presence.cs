using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;
using Disqord.Gateway.Models;
using Qommon;

namespace Disqord.Gateway
{
    public static partial class GatewayClientExtensions
    {
        public static Task SetPresenceAsync(this IGatewayClient client, UserStatus status, CancellationToken cancellationToken = default)
            => client.InternalSetPresenceAsync(status, default, cancellationToken);

        public static Task SetPresenceAsync(this IGatewayClient client, LocalActivity activity, CancellationToken cancellationToken = default)
            => client.InternalSetPresenceAsync(default, activity != null ? new[] { activity } : null, cancellationToken);

        public static Task SetPresenceAsync(this IGatewayClient client, UserStatus status, IEnumerable<LocalActivity> activities, CancellationToken cancellationToken = default)
            => client.InternalSetPresenceAsync(status, Optional.Create(activities), cancellationToken);

        public static Task SetPresenceAsync(this IGatewayClient client, UserStatus status, LocalActivity activity, CancellationToken cancellationToken = default)
            => client.InternalSetPresenceAsync(status, activity != null ? new[] { activity } : null, cancellationToken);

        private static Task InternalSetPresenceAsync(this IGatewayClient client, Optional<UserStatus> status, Optional<IEnumerable<LocalActivity>> activities, CancellationToken cancellationToken)
            => Task.WhenAll(client.Shards.Values.Select(x => x.InternalSetPresenceAsync(status, activities, cancellationToken)));

        public static Task SetPresenceAsync(this IGatewayApiClient shard, UserStatus status, CancellationToken cancellationToken = default)
            => shard.InternalSetPresenceAsync(status, default, cancellationToken);

        public static Task SetPresenceAsync(this IGatewayApiClient shard, LocalActivity activity, CancellationToken cancellationToken = default)
            => shard.InternalSetPresenceAsync(default, activity != null ? new[] { activity } : null, cancellationToken);

        public static Task SetPresenceAsync(this IGatewayApiClient shard, UserStatus status, IEnumerable<LocalActivity> activities, CancellationToken cancellationToken = default)
            => shard.InternalSetPresenceAsync(status, Optional.Create(activities), cancellationToken);

        public static Task SetPresenceAsync(this IGatewayApiClient shard, UserStatus status, LocalActivity activity, CancellationToken cancellationToken = default)
            => shard.InternalSetPresenceAsync(status, activity != null ? new[] { activity } : null, cancellationToken);

        private static Task InternalSetPresenceAsync(this IGatewayApiClient shard, Optional<UserStatus> status, Optional<IEnumerable<LocalActivity>> activities, CancellationToken cancellationToken)
        {
            var presence = shard.Presence;
            if (presence == null)
            {
                shard.Presence = new UpdatePresenceJsonModel
                {
                    Status = UserStatus.Online,
                    Activities = Array.Empty<ActivityJsonModel>()
                };
            }

            if (status.HasValue)
                shard.Presence.Status = status.Value;

            if (activities.HasValue)
                shard.Presence.Activities = activities.Value?.Select(x => x.ToModel()).ToArray() ?? Array.Empty<ActivityJsonModel>();

            var payload = new GatewayPayloadJsonModel
            {
                Op = GatewayPayloadOperation.UpdatePresence,
                D = new UpdatePresenceJsonModel
                {
                    Status = shard.Presence.Status,
                    Activities = shard.Presence.Activities
                }
            };
            return shard.SendAsync(payload, cancellationToken);
        }
    }
}
