using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Rest.Pagination;

namespace Disqord.Rest;

public static partial class RestEntityExtensions
{
    public static Task<ITextChannel> ModifyAsync(this ITextChannel channel,
        Action<ModifyTextChannelActionProperties> action,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = channel.GetRestClient();
        return client.ModifyTextChannelAsync(channel.Id, action, options, cancellationToken);
    }

    public static Task<IVoiceChannel> ModifyAsync(this IVoiceChannel channel,
        Action<ModifyVoiceChannelActionProperties> action,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = channel.GetRestClient();
        return client.ModifyVoiceChannelAsync(channel.Id, action, options, cancellationToken);
    }

    public static Task<IStageChannel> ModifyAsync(this IStageChannel channel,
        Action<ModifyStageChannelActionProperties> action,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = channel.GetRestClient();
        return client.ModifyStageChannelAsync(channel.Id, action, options, cancellationToken);
    }

    public static Task<IForumChannel> ModifyAsync(this IForumChannel channel,
        Action<ModifyForumChannelActionProperties> action,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = channel.GetRestClient();
        return client.ModifyForumChannelAsync(channel.Id, action, options, cancellationToken);
    }

    public static Task<ICategoryChannel> ModifyAsync(this ICategoryChannel channel,
        Action<ModifyCategoryChannelActionProperties> action,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = channel.GetRestClient();
        return client.ModifyCategoryChannelAsync(channel.Id, action, options, cancellationToken);
    }

    public static Task<IThreadChannel> ModifyAsync(this IThreadChannel thread,
        Action<ModifyThreadChannelActionProperties> action,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = thread.GetRestClient();
        return client.ModifyThreadChannelAsync(thread.Id, action, options, cancellationToken);
    }

    public static Task DeleteAsync(this IChannel channel,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = channel.GetRestClient();
        return client.DeleteChannelAsync(channel.Id, options, cancellationToken);
    }

    public static IPagedEnumerable<IMessage> EnumerateMessages(this IMessageChannel channel,
        int limit, FetchDirection direction = FetchDirection.Before, Snowflake? startFromId = null,
        IRestRequestOptions? options = null)
    {
        var client = channel.GetRestClient();
        return client.EnumerateMessages(channel.Id, limit, direction, startFromId, options);
    }

    public static Task<IReadOnlyList<IMessage>> FetchMessagesAsync(this IMessageChannel channel,
        int limit = Discord.Limits.Rest.FetchMessagesPageSize, FetchDirection direction = FetchDirection.Before, Snowflake? startFromId = null,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = channel.GetRestClient();
        return client.FetchMessagesAsync(channel.Id, limit, direction, startFromId, options, cancellationToken);
    }

    public static Task<IMessage?> FetchMessageAsync(this IMessageChannel channel,
        Snowflake messageId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = channel.GetRestClient();
        return client.FetchMessageAsync(channel.Id, messageId, options, cancellationToken);
    }

    public static Task<IUserMessage> SendMessageAsync(this IMessageChannel channel,
        LocalMessage message,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = channel.GetRestClient();
        return client.SendMessageAsync(channel.Id, message, options, cancellationToken);
    }

    public static Task<IUserMessage> CrosspostMessageAsync(this ITextChannel channel,
        Snowflake messageId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        if (!channel.IsNews)
            throw new ArgumentException("This text channel must be a news channel to have messages crossposted from it.", nameof(channel));

        var client = channel.GetRestClient();
        return client.CrosspostMessageAsync(channel.Id, messageId, options, cancellationToken);
    }

    public static Task AddReactionAsync(this IMessageChannel channel,
        Snowflake messageId, LocalEmoji emoji,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = channel.GetRestClient();
        return client.AddReactionAsync(channel.Id, messageId, emoji, options, cancellationToken);
    }

    public static Task RemoveOwnReactionAsync(this IMessageChannel channel,
        Snowflake messageId, LocalEmoji emoji,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = channel.GetRestClient();
        return client.RemoveOwnReactionAsync(channel.Id, messageId, emoji, options, cancellationToken);
    }

    public static Task RemoveReactionAsync(this IMessageChannel channel,
        Snowflake messageId, LocalEmoji emoji, Snowflake userId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = channel.GetRestClient();
        return client.RemoveReactionAsync(channel.Id, messageId, emoji, userId, options, cancellationToken);
    }

    public static IPagedEnumerable<IUser> EnumerateReactions(this IMessageChannel channel,
        Snowflake messageId, LocalEmoji emoji, int limit, Snowflake? startFromId = null,
        IRestRequestOptions? options = null)
    {
        var client = channel.GetRestClient();
        return client.EnumerateReactions(channel.Id, messageId, emoji, limit, startFromId, options);
    }

    public static Task<IReadOnlyList<IUser>> FetchReactionsAsync(this IMessageChannel channel,
        Snowflake messageId, LocalEmoji emoji, int limit = Discord.Limits.Rest.FetchReactionsPageSize, Snowflake? startFromId = null,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = channel.GetRestClient();
        return client.FetchReactionsAsync(channel.Id, messageId, emoji, limit, startFromId, options, cancellationToken);
    }

    public static Task ClearReactionsAsync(this IMessageChannel channel,
        Snowflake messageId, LocalEmoji? emoji = null,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = channel.GetRestClient();
        return client.ClearReactionsAsync(channel.Id, messageId, emoji, options, cancellationToken);
    }

    public static Task<IUserMessage> ModifyMessageAsync(this IMessageChannel channel,
        Snowflake messageId, Action<ModifyMessageActionProperties> action,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = channel.GetRestClient();
        return client.ModifyMessageAsync(channel.Id, messageId, action, options, cancellationToken);
    }

    public static Task DeleteMessageAsync(this IMessageChannel channel,
        Snowflake messageId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = channel.GetRestClient();
        return client.DeleteMessageAsync(channel.Id, messageId, options, cancellationToken);
    }

    public static IPagedEnumerable<Snowflake> EnumerateMessageDeletion(this IMessageGuildChannel channel,
        IEnumerable<Snowflake> messageIds,
        IRestRequestOptions? options = null)
    {
        var client = channel.GetRestClient();
        return client.EnumerateMessageDeletion(channel.Id, messageIds, options);
    }

    public static Task DeleteMessagesAsync(this IMessageGuildChannel channel,
        IEnumerable<Snowflake> messageIds,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = channel.GetRestClient();
        return client.DeleteMessagesAsync(channel.Id, messageIds, options, cancellationToken);
    }

    public static Task SetOverwriteAsync(this IGuildChannel channel,
        LocalOverwrite overwrite,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = channel.GetRestClient();
        return client.SetOverwriteAsync(channel.Id, overwrite, options, cancellationToken);
    }

    public static Task DeleteOverwriteAsync(this IGuildChannel channel,
        Snowflake targetId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = channel.GetRestClient();
        return client.DeleteOverwriteAsync(channel.Id, targetId, options, cancellationToken);
    }

    public static Task<IReadOnlyList<IInvite>> FetchInvitesAsync(this IGuildChannel channel,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = channel.GetRestClient();
        return client.FetchChannelInvitesAsync(channel.Id, options, cancellationToken);
    }

    public static Task<IInvite> CreateInviteAsync(this IGuildChannel channel,
        TimeSpan maxAge = default, int maxUses = 0, bool isTemporaryMembership = false, bool isUnique = false,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = channel.GetRestClient();
        return client.CreateInviteAsync(channel.Id, maxAge, maxUses, isTemporaryMembership, isUnique, options, cancellationToken);
    }

    public static Task<IFollowedChannel> FollowAsync(this ITextChannel channel,
        Snowflake targetChannelId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        if (!channel.IsNews)
            throw new ArgumentException("This text channel must be a news channel to follow it.", nameof(channel));

        var client = channel.GetRestClient();
        return client.FollowNewsChannelAsync(channel.Id, targetChannelId, options, cancellationToken);
    }

    public static Task TriggerTypingAsync(this IMessageChannel channel,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = channel.GetRestClient();
        return client.TriggerTypingAsync(channel.Id, options, cancellationToken);
    }

    public static IDisposable BeginTyping(this IMessageChannel channel,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = channel.GetRestClient();
        return client.BeginTyping(channel.Id, options, cancellationToken);
    }

    public static Task<IReadOnlyList<IUserMessage>> FetchPinnedMessagesAsync(this IMessageChannel channel,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = channel.GetRestClient();
        return client.FetchPinnedMessagesAsync(channel.Id, options, cancellationToken);
    }

    public static Task PinMessageAsync(this IMessageChannel channel,
        Snowflake messageId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = channel.GetRestClient();
        return client.PinMessageAsync(channel.Id, messageId, options, cancellationToken);
    }

    public static Task UnpinMessageAsync(this IMessageChannel channel,
        Snowflake messageId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = channel.GetRestClient();
        return client.UnpinMessageAsync(channel.Id, messageId, options, cancellationToken);
    }

    /*
     * Webhooks
     */
    public static Task<IWebhook> CreateWebhookAsync(this IMessageChannel channel,
        string name, Action<CreateWebhookActionProperties>? action = null,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = channel.GetRestClient();
        return client.CreateWebhookAsync(channel.Id, name, action, options, cancellationToken);
    }

    public static Task<IReadOnlyList<IWebhook>> FetchWebhooksAsync(this IMessageChannel channel,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = channel.GetRestClient();
        return client.FetchChannelWebhooksAsync(channel.Id, options, cancellationToken);
    }

    /*
    * Threads
    */
    public static Task<IThreadChannel> CreatePublicThreadAsync(this ITextChannel channel,
        string name, Snowflake? messageId = null, Action<CreateThreadChannelActionProperties>? action = null,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = channel.GetRestClient();
        return client.CreatePublicThreadAsync(channel.Id, name, messageId, action, options, cancellationToken);
    }

    public static Task<IThreadChannel> CreatePrivateThreadAsync(this ITextChannel channel,
        string name, Action<CreateThreadChannelActionProperties>? action = null,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = channel.GetRestClient();
        return client.CreatePrivateThreadAsync(channel.Id, name, action, options, cancellationToken);
    }

    public static Task<IThreadChannel> CreateThreadAsync(this IForumChannel channel,
        string name, LocalMessage message, Action<CreateThreadChannelActionProperties>? action = null,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = channel.GetRestClient();
        return client.CreateForumThreadAsync(channel.Id, name, message, action, options, cancellationToken);
    }

    public static Task JoinAsync(this IThreadChannel thread,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = thread.GetRestClient();
        return client.JoinThreadAsync(thread.Id, options, cancellationToken);
    }

    public static Task AddMemberAsync(this IThreadChannel thread,
        Snowflake memberId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = thread.GetRestClient();
        return client.AddThreadMemberAsync(thread.Id, memberId, options, cancellationToken);
    }

    public static Task LeaveAsync(this IThreadChannel thread,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = thread.GetRestClient();
        return client.LeaveThreadAsync(thread.Id, options, cancellationToken);
    }

    public static Task RemoveMemberAsync(this IThreadChannel thread,
        Snowflake memberId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = thread.GetRestClient();
        return client.RemoveThreadMemberAsync(thread.Id, memberId, options, cancellationToken);
    }

    public static Task<IThreadMember?> FetchMemberAsync(this IThreadChannel thread,
        Snowflake memberId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = thread.GetRestClient();
        return client.FetchThreadMemberAsync(thread.Id, memberId, options, cancellationToken);
    }

    public static Task<IReadOnlyList<IThreadMember>> FetchMembersAsync(this IThreadChannel thread,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = thread.GetRestClient();
        return client.FetchThreadMembersAsync(thread.Id, options, cancellationToken);
    }

    public static IPagedEnumerable<IThreadChannel> EnumeratePublicArchivedThreads(this ITextChannel channel,
        int limit, DateTimeOffset? startFromDate = null,
        IRestRequestOptions? options = null)
    {
        var client = channel.GetRestClient();
        return client.EnumeratePublicArchivedThreads(channel.Id, limit, startFromDate, options);
    }

    public static Task<IReadOnlyList<IThreadChannel>> FetchPublicArchivedThreadsAsync(this ITextChannel channel,
        int limit = Discord.Limits.Rest.FetchThreadsPageSize, DateTimeOffset? startFromDate = null,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = channel.GetRestClient();
        return client.FetchPublicArchivedThreadsAsync(channel.Id, limit, startFromDate, options, cancellationToken);
    }

    public static IPagedEnumerable<IThreadChannel> EnumeratePrivateArchivedThreads(this ITextChannel channel,
        int limit, DateTimeOffset? startFromDate = null,
        IRestRequestOptions? options = null)
    {
        var client = channel.GetRestClient();
        return client.EnumeratePrivateArchivedThreads(channel.Id, limit, startFromDate, options);
    }

    public static Task<IReadOnlyList<IThreadChannel>> FetchPrivateArchivedThreadsAsync(this ITextChannel channel,
        int limit = Discord.Limits.Rest.FetchThreadsPageSize, DateTimeOffset? startFromDate = null,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = channel.GetRestClient();
        return client.FetchPrivateArchivedThreadsAsync(channel.Id, limit, startFromDate, options, cancellationToken);
    }

    public static IPagedEnumerable<IThreadChannel> EnumerateJoinedPrivateArchivedThreads(this ITextChannel channel,
        int limit, Snowflake? startFromId = null,
        IRestRequestOptions? options = null)
    {
        var client = channel.GetRestClient();
        return client.EnumerateJoinedPrivateArchivedThreads(channel.Id, limit, startFromId, options);
    }

    public static Task<IReadOnlyList<IThreadChannel>> FetchJoinedPrivateArchivedThreadsAsync(this ITextChannel channel,
        int limit = Discord.Limits.Rest.FetchThreadsPageSize, Snowflake? startFromId = null,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = channel.GetRestClient();
        return client.FetchJoinedPrivateArchivedThreadsAsync(channel.Id, limit, startFromId, options, cancellationToken);
    }

    /*
     * Stages
     */
    public static Task<IStage> CreateStageAsync(this IStageChannel channel,
        string topic, Action<CreateStageActionProperties>? action = null,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = channel.GetRestClient();
        return client.CreateStageAsync(channel.Id, topic, action, options, cancellationToken);
    }

    public static Task<IStage?> FetchStageAsync(this IStageChannel channel,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = channel.GetRestClient();
        return client.FetchStageAsync(channel.Id, options, cancellationToken);
    }
}
