using Disqord.Events;

namespace Disqord.Gateway.Default
{
    public partial class DefaultGatewayClient : IGatewayClient
    {
        public event AsynchronousEventHandler<ReadyEventArgs> Ready
        {
            add => Dispatcher.ReadyEvent.Hook(value);
            remove => Dispatcher.ReadyEvent.Unhook(value);
        }

        public event AsynchronousEventHandler<ChannelCreatedEventArgs> ChannelCreated
        {
            add => Dispatcher.ChannelCreatedEvent.Hook(value);
            remove => Dispatcher.ChannelCreatedEvent.Unhook(value);
        }

        public event AsynchronousEventHandler<ChannelUpdatedEventArgs> ChannelUpdated
        {
            add => Dispatcher.ChannelUpdatedEvent.Hook(value);
            remove => Dispatcher.ChannelUpdatedEvent.Unhook(value);
        }

        public event AsynchronousEventHandler<ChannelDeletedEventArgs> ChannelDeleted
        {
            add => Dispatcher.ChannelDeletedEvent.Hook(value);
            remove => Dispatcher.ChannelDeletedEvent.Unhook(value);
        }

        public event AsynchronousEventHandler<ChannelPinsUpdatedEventArgs> ChannelPinsUpdated
        {
            add => Dispatcher.ChannelPinsUpdatedEvent.Hook(value);
            remove => Dispatcher.ChannelPinsUpdatedEvent.Unhook(value);
        }

        public event AsynchronousEventHandler<GuildAvailableEventArgs> GuildAvailable
        {
            add => Dispatcher.GuildAvailableEvent.Hook(value);
            remove => Dispatcher.GuildAvailableEvent.Unhook(value);
        }

        public event AsynchronousEventHandler<JoinedGuildEventArgs> JoinedGuild
        {
            add => Dispatcher.JoinedGuildEvent.Hook(value);
            remove => Dispatcher.JoinedGuildEvent.Unhook(value);
        }

        public event AsynchronousEventHandler<GuildUnavailableEventArgs> GuildUnavailable
        {
            add => Dispatcher.GuildUnavailableEvent.Hook(value);
            remove => Dispatcher.GuildUnavailableEvent.Unhook(value);
        }

        public event AsynchronousEventHandler<LeftGuildEventArgs> LeftGuild
        {
            add => Dispatcher.LeftGuildEvent.Hook(value);
            remove => Dispatcher.LeftGuildEvent.Unhook(value);
        }

        public event AsynchronousEventHandler<BanCreatedEventArgs> BanCreated
        {
            add => Dispatcher.BanCreatedEvent.Hook(value);
            remove => Dispatcher.BanCreatedEvent.Unhook(value);
        }

        public event AsynchronousEventHandler<BanDeletedEventArgs> BanDeleted
        {
            add => Dispatcher.BanDeletedEvent.Hook(value);
            remove => Dispatcher.BanDeletedEvent.Unhook(value);
        }

        public event AsynchronousEventHandler<MessageReceivedEventArgs> MessageReceived
        {
            add => Dispatcher.MessageReceivedEvent.Hook(value);
            remove => Dispatcher.MessageReceivedEvent.Unhook(value);
        }

        public event AsynchronousEventHandler<TypingStartedEventArgs> TypingStarted
        {
            add => Dispatcher.TypingStartedEvent.Hook(value);
            remove => Dispatcher.TypingStartedEvent.Unhook(value);
        }

        public event AsynchronousEventHandler<VoiceStateUpdatedEventArgs> VoiceStateUpdated
        {
            add => Dispatcher.VoiceStateUpdatedEvent.Hook(value);
            remove => Dispatcher.VoiceStateUpdatedEvent.Unhook(value);
        }

        public event AsynchronousEventHandler<VoiceServerUpdatedEventArgs> VoiceServerUpdated
        {
            add => Dispatcher.VoiceServerUpdatedEvent.Hook(value);
            remove => Dispatcher.VoiceServerUpdatedEvent.Unhook(value);
        }
    }
}
