using System;
using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class TypingStartHandler : Handler<TypingStartJsonModel, TypingStartedEventArgs>
    {
        public override async Task<TypingStartedEventArgs> HandleDispatchAsync(IGatewayApiClient shard, TypingStartJsonModel model)
        {
            CachedMember member = null;
            if (model.GuildId.HasValue)
            {
                member = Dispatcher.GetOrAddMember(model.GuildId.Value, model.Member.Value);
            }

            return new TypingStartedEventArgs(model.GuildId.GetValueOrNullable(), model.ChannelId, model.UserId, DateTimeOffset.FromUnixTimeSeconds(model.Timestamp), member);
        }
    }
}
