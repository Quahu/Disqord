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

            return _client._typingStarted.InvokeAsync(new TypingStartedEventArgs(_client,
                new OptionalSnowflakeEntity<ICachedMessageChannel>(channel, model.ChannelId),
                new DownloadableOptionalSnowflakeEntity<CachedUser, RestUser>(user, model.UserId,
                async options => guild != null
                    ? await guild.GetMemberAsync(model.UserId, options).ConfigureAwait(false)
                    : await _client.GetUserAsync(model.UserId, options).ConfigureAwait(false)),
                DateTimeOffset.FromUnixTimeSeconds(model.Timestamp)));
        }
    }
}
