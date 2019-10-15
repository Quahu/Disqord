using System;
using Disqord.Models;

namespace Disqord.Rest
{
    public sealed class RestGatewayBotResponse : RestDiscordEntity
    {
        public string Url { get; private set; }

        public int ShardAmount { get; private set; }

        public int MaxSessionAmount { get; private set; }

        public int RemainingSessionAmount { get; private set; }

        public TimeSpan ResetAfter { get; private set; }

        internal RestGatewayBotResponse(RestDiscordClient client, GatewayBotModel model) : base(client)
        {
            Update(model);
        }

        internal void Update(GatewayBotModel model)
        {
            Url = model.Url;
            ShardAmount = model.Shards;
            MaxSessionAmount = model.SessionStartLimit.Total;
            RemainingSessionAmount = model.SessionStartLimit.Remaining;
            ResetAfter = TimeSpan.FromMilliseconds(model.SessionStartLimit.ResetAfter);
        }
    }
}
