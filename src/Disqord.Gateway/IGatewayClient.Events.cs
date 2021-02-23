using Disqord.Events;

namespace Disqord.Gateway
{
    public partial interface IGatewayClient : IClient
    {
        event AsynchronousEventHandler<ReadyEventArgs> Ready;

        event AsynchronousEventHandler<ChannelCreatedEventArgs> ChannelCreated;

        event AsynchronousEventHandler<ChannelUpdatedEventArgs> ChannelUpdated;

        event AsynchronousEventHandler<ChannelDeletedEventArgs> ChannelDeleted;

        event AsynchronousEventHandler<ChannelPinsUpdatedEventArgs> ChannelPinsUpdated;

        event AsynchronousEventHandler<GuildAvailableEventArgs> GuildAvailable;

        event AsynchronousEventHandler<JoinedGuildEventArgs> JoinedGuild;

        event AsynchronousEventHandler<GuildUnavailableEventArgs> GuildUnavailable;

        event AsynchronousEventHandler<LeftGuildEventArgs> LeftGuild;

        event AsynchronousEventHandler<BanCreatedEventArgs> BanCreated;

        event AsynchronousEventHandler<BanDeletedEventArgs> BanDeleted;

        event AsynchronousEventHandler<MessageReceivedEventArgs> MessageReceived;

        event AsynchronousEventHandler<MessageDeletedEventArgs> MessageDeleted;

        event AsynchronousEventHandler<TypingStartedEventArgs> TypingStarted;

        event AsynchronousEventHandler<VoiceStateUpdatedEventArgs> VoiceStateUpdated;

        event AsynchronousEventHandler<VoiceServerUpdatedEventArgs> VoiceServerUpdated;
    }
}
