﻿using System.Threading.Tasks;
using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class TypingStartHandler : Handler<TypingStartJsonModel, TypingStartedEventArgs>
    {
        public override async Task<TypingStartedEventArgs> HandleDispatchAsync(TypingStartJsonModel model)
        {
            if (model.GuildId.HasValue)
            {

            }

            return new TypingStartedEventArgs(model.ChannelId);
        }
    }
}
