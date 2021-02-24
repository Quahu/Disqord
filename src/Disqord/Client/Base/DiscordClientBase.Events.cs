using Disqord.Events;
using Disqord.Gateway;
using Disqord.Rest;

namespace Disqord
{
    public abstract partial class DiscordClientBase : IRestClient, IGatewayClient
    {
        public event AsynchronousEventHandler<ReadyEventArgs> Ready
        {
            add => GatewayClient.Ready += value;
            remove => GatewayClient.Ready -= value;
        }

        public event AsynchronousEventHandler<ChannelCreatedEventArgs> ChannelCreated
        {
            add => GatewayClient.ChannelCreated += value;
            remove => GatewayClient.ChannelCreated -= value;
        }

        public event AsynchronousEventHandler<ChannelUpdatedEventArgs> ChannelUpdated
        {
            add => GatewayClient.ChannelUpdated += value;
            remove => GatewayClient.ChannelUpdated -= value;
        }

        public event AsynchronousEventHandler<ChannelDeletedEventArgs> ChannelDeleted
        {
            add => GatewayClient.ChannelDeleted += value;
            remove => GatewayClient.ChannelDeleted -= value;
        }

        public event AsynchronousEventHandler<ChannelPinsUpdatedEventArgs> ChannelPinsUpdated
        {
            add => GatewayClient.ChannelPinsUpdated += value;
            remove => GatewayClient.ChannelPinsUpdated -= value;
        }

        public event AsynchronousEventHandler<GuildAvailableEventArgs> GuildAvailable
        {
            add => GatewayClient.GuildAvailable += value;
            remove => GatewayClient.GuildAvailable -= value;
        }

        public event AsynchronousEventHandler<JoinedGuildEventArgs> JoinedGuild
        {
            add => GatewayClient.JoinedGuild += value;
            remove => GatewayClient.JoinedGuild -= value;
        }

        public event AsynchronousEventHandler<GuildUnavailableEventArgs> GuildUnavailable
        {
            add => GatewayClient.GuildUnavailable += value;
            remove => GatewayClient.GuildUnavailable -= value;
        }

        public event AsynchronousEventHandler<LeftGuildEventArgs> LeftGuild
        {
            add => GatewayClient.LeftGuild += value;
            remove => GatewayClient.LeftGuild -= value;
        }

        public event AsynchronousEventHandler<BanCreatedEventArgs> BanCreated
        {
            add => GatewayClient.BanCreated += value;
            remove => GatewayClient.BanCreated -= value;
        }

        public event AsynchronousEventHandler<BanDeletedEventArgs> BanDeleted
        {
            add => GatewayClient.BanDeleted += value;
            remove => GatewayClient.BanDeleted -= value;
        }

        public event AsynchronousEventHandler<MessageReceivedEventArgs> MessageReceived
        {
            add => GatewayClient.MessageReceived += value;
            remove => GatewayClient.MessageReceived -= value;
        }

        public event AsynchronousEventHandler<MessageUpdatedEventArgs> MessageUpdated
        {
            add => GatewayClient.MessageUpdated += value;
            remove => GatewayClient.MessageUpdated -= value;
        }

        public event AsynchronousEventHandler<MessageDeletedEventArgs> MessageDeleted
        {
            add => GatewayClient.MessageDeleted += value;
            remove => GatewayClient.MessageDeleted -= value;
        }

        public event AsynchronousEventHandler<TypingStartedEventArgs> TypingStarted
        {
            add => GatewayClient.TypingStarted += value;
            remove => GatewayClient.TypingStarted -= value;
        }

        public event AsynchronousEventHandler<VoiceStateUpdatedEventArgs> VoiceStateUpdated
        {
            add => GatewayClient.VoiceStateUpdated += value;
            remove => GatewayClient.VoiceStateUpdated -= value;
        }

        public event AsynchronousEventHandler<VoiceServerUpdatedEventArgs> VoiceServerUpdated
        {
            add => GatewayClient.VoiceServerUpdated += value;
            remove => GatewayClient.VoiceServerUpdated -= value;
        }
    }
}
