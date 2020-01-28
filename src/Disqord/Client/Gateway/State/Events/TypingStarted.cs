using System;
using System.Threading.Tasks;
using Disqord.Events;
using Disqord.Models;
using Disqord.Models.Dispatches;
using Disqord.Rest;

namespace Disqord
{
    internal sealed partial class DiscordClientState
    {
        public Task HandleTypingStartedAsync(PayloadModel payload)
        {
            var model = Serializer.ToObject<TypingStartModel>(payload.D);
            CachedGuild guild = null;
            ICachedMessageChannel channel;
            CachedUser user;
            if (model.GuildId != null)
            {
                guild = GetGuild(model.GuildId.Value);
                channel = guild.GetTextChannel(model.ChannelId);
                user = guild.GetMember(model.UserId);
            }
            else
            {
                channel = GetPrivateChannel(model.ChannelId);
                user = GetUser(model.UserId);
            }

            var userOptional = FetchableSnowflakeOptional.Create<CachedUser, RestUser, IUser>(
                model.UserId, channel is CachedTextChannel textChannel
                    ? textChannel.Guild.GetMember(model.UserId) ?? GetUser(model.UserId)
                    : GetUser(model.UserId),
                RestFetchable.Create((this, model), async (tuple, options) =>
                {
                    var (@this, model) = tuple;
                    return model.GuildId != null
                        ? await @this._client.GetMemberAsync(model.GuildId.Value, model.UserId, options).ConfigureAwait(false)
                            ?? await @this._client.GetUserAsync(model.UserId, options).ConfigureAwait(false)
                        : await @this._client.GetUserAsync(model.UserId, options).ConfigureAwait(false);
                }));
            return _client._typingStarted.InvokeAsync(new TypingStartedEventArgs(_client,
                new SnowflakeOptional<ICachedMessageChannel>(channel, model.ChannelId),
                userOptional,
                DateTimeOffset.FromUnixTimeSeconds(model.Timestamp)));
        }
    }
}
