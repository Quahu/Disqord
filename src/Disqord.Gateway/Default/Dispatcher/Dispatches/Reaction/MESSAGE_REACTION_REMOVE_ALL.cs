﻿using System.Threading.Tasks;
using Disqord.Gateway.Api;

namespace Disqord.Gateway.Default
{
    public partial class DefaultGatewayDispatcher
    {
        private Task MessageReactionRemoveAllAsync(GatewayDispatchReceivedEventArgs e)
        {
            //return _messageDeletedEvent.InvokeAsync(this, new MessageDeletedEventArgs());
            return Task.CompletedTask;
        }
    }
}