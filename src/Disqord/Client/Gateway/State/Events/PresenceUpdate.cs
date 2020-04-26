using System;
using System.Threading.Tasks;
using Disqord.Events;
using Disqord.Logging;
using Disqord.Models;
using Disqord.Models.Dispatches;

namespace Disqord
{
    internal sealed partial class DiscordClientState
    {
        public Task HandlePresenceUpdateAsync(PayloadModel payload)
        {
            PresenceUpdateModel model;
            try
            {
                model = Serializer.ToObject<PresenceUpdateModel>(payload.D);
            }
            catch (Exception ex)
            {
                MemberModel memberModel;
                try
                {
                    memberModel = Serializer.ToObject<MemberModel>(payload.D);
                }
                catch
                {
                    // Just to be safe?
                    Log(LogMessageSeverity.Warning, $"Discarding an invalid presence update for an unknown user.", ex);
                    return Task.CompletedTask;
                }

                Log(LogMessageSeverity.Warning, $"Discarding an invalid presence update for user {memberModel.User.Id}.", ex);
                return Task.CompletedTask;
            }

            // If the name is set it's a user update or
            // the user went from offline to online.
            var hasUserData = model.User.Username.HasValue;
            CachedUser user;
            if (model.GuildId != null)
            {
                var guild = GetGuild(model.GuildId.Value);
                user = hasUserData
                    ? GetOrAddMember(guild, new MemberModel
                    {
                        Nick = model.Nick,
                        Roles = model.Roles
                    }, model.User, true)
                    : guild.GetMember(model.User.Id);
            }
            else
            {
                user = hasUserData
                    ? GetSharedOrUnknownUser(model.User)
                    : GetUser(model.User.Id);
            }

            // Discard updates for uncached users.
            if (user == null)
                return Task.CompletedTask;

            if (hasUserData)
            {
                // We have to check if any of the properties were changed,
                // so we don't fire the event multiple times for each guild
                // the 'presence' update was dispatched for or for offline -> online.
                if (user.Name != model.User.Username.Value ||
                    user.Discriminator != model.User.Discriminator.Value ||
                    user.AvatarHash != model.User.Avatar.Value)
                {
                    var oldUser = user.Clone();
                    user.Update(model.User);
                    return _client._userUpdated.InvokeAsync(new UserUpdatedEventArgs(oldUser, user));
                }
            }

            // Let's just hope with the new gateway intents Discord
            // will finally send presences separately from user updates.
            var oldPresence = user.Presence;
            if ((oldPresence == null || oldPresence.Status == UserStatus.Offline) && model.Status == UserStatus.Offline)
                return Task.CompletedTask;

            if (oldPresence?.Activity != null && model.Game != null
                && oldPresence.Activity.CreatedAt == DateTimeOffset.FromUnixTimeMilliseconds(model.Game.CreatedAt.Value))
                return Task.CompletedTask;

            user.Update(model);
            return _client._presenceUpdated.InvokeAsync(new PresenceUpdatedEventArgs(user, oldPresence));
        }
    }
}
