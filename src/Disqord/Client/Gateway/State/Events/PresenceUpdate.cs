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

            // If the name is set it's a user update.
            // Otherwise it's an actual presence update.
            if (model.User.Username.HasValue)
            {
                CachedUser user;
                if (model.GuildId != null)
                {
                    var guild = GetGuild(model.GuildId.Value);
                    var member = guild.GetMember(model.User.Id);
                    if (member == null)
                    {
                        member = CreateMember(guild, new MemberModel
                        {
                            Nick = model.Nick,
                            Roles = model.Roles
                        }, model.User);
                    }

                    user = member;
                }
                else
                {
                    user = GetOrAddSharedUser(model.User);
                }

                var oldUser = user.Clone();
                // We have to check if any of the properties were changed,
                // so we don't fire the event multiple times for each guild
                // the 'presence' update was dispatched for.
                if (user.Name != model.User.Username ||
                    user.Discriminator != model.User.Discriminator ||
                    user.AvatarHash != model.User.Avatar)
                {
                    user.Update(model.User);
                    return _client._userUpdated.InvokeAsync(new UserUpdatedEventArgs(oldUser, user));
                }

                return Task.CompletedTask;
            }
            else
            {
                CachedUser user;
                if (model.GuildId != null)
                {
                    var guild = GetGuild(model.GuildId.Value);
                    user = guild.GetMember(model.User.Id);
                }
                else
                {
                    user = GetUser(model.User.Id);
                }

                // We discard presence updates for uncached users.
                if (user == null)
                    return Task.CompletedTask;

                var oldPresence = user.Presence;
                user.Update(model);
                return _client._presenceUpdated.InvokeAsync(new PresenceUpdatedEventArgs(user, oldPresence));
            }
        }
    }
}
