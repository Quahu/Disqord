using System;
using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;
using Qommon;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class TypingStartHandler : Handler<TypingStartJsonModel, TypingStartedEventArgs>
    {
        public override ValueTask<TypingStartedEventArgs> HandleDispatchAsync(IGatewayApiClient shard, TypingStartJsonModel model)
        {
            IMember member = null;
            if (model.GuildId.HasValue)
            {
                member = Dispatcher.GetOrAddMemberTransient(model.GuildId.Value, model.Member.Value);
            }

            var e = new TypingStartedEventArgs(model.GuildId.GetValueOrNullable(), model.ChannelId, model.UserId, DateTimeOffset.FromUnixTimeSeconds(model.Timestamp), member);
            return new(e);
        }
    }
}
