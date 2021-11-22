using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using Disqord.Http;

namespace Disqord.Rest.Api
{
    public sealed partial class Route
    {
        public static class AuditLog
        {
            public static readonly Route GetAuditLogs = Get("guilds/{0:guild_id}/audit-logs");
        }

        public static class Channel
        {
            public static readonly Route GetChannel = Get("channels/{0:channel_id}");

            public static readonly Route ModifyChannel = Patch("channels/{0:channel_id}");

            public static readonly Route DeleteChannel = Delete("channels/{0:channel_id}");

            public static readonly Route GetMessages = Get("channels/{0:channel_id}/messages");

            public static readonly Route GetMessage = Get("channels/{0:channel_id}/messages/{1:message_id}");

            public static readonly Route CreateMessage = Post("channels/{0:channel_id}/messages");

            public static readonly Route CrosspostMessage = Post("channels/{0:channel_id}/messages/{1:message_id}/crosspost");

            public static readonly Route CreateReaction = Put("channels/{0:channel_id}/messages/{1:message_id}/reactions/{2:emoji}/@me");

            public static readonly Route DeleteOwnReaction = Delete("channels/{0:channel_id}/messages/{1:message_id}/reactions/{2:emoji}/@me");

            public static readonly Route DeleteUserReaction = Delete("channels/{0:channel_id}/messages/{1:message_id}/reactions/{2:emoji}/{3:user_id}");

            public static readonly Route GetReactions = Get("channels/{0:channel_id}/messages/{1:message_id}/reactions/{2:emoji}");

            public static readonly Route ClearReactions = Delete("channels/{0:channel_id}/messages/{1:message_id}/reactions");

            public static readonly Route ClearEmojiReactions = Delete("channels/{0:channel_id}/messages/{1:message_id}/reactions/{2:emoji}");

            public static readonly Route ModifyMessage = Patch("channels/{0:channel_id}/messages/{1:message_id}");

            public static readonly Route DeleteMessage = Delete("channels/{0:channel_id}/messages/{1:message_id}");

            public static readonly Route DeleteMessages = Post("channels/{0:channel_id}/messages/bulk-delete");

            public static readonly Route SetOverwrite = Put("channels/{0:channel_id}/permissions/{1:overwrite_id}");

            public static readonly Route DeleteOverwrite = Delete("channels/{0:channel_id}/permissions/{1:overwrite_id}");

            public static readonly Route FollowNewsChannel = Post("channels/{0:channel_id}/followers");

            public static readonly Route GetInvites = Get("channels/{0:channel_id}/invites");

            public static readonly Route CreateInvite = Post("channels/{0:channel_id}/invites");

            public static readonly Route TriggerTyping = Post("channels/{0:channel_id}/typing");

            public static readonly Route GetPinnedMessages = Get("channels/{0:channel_id}/pins");

            public static readonly Route PinMessage = Put("channels/{0:channel_id}/pins/{1:message_id}");

            public static readonly Route UnpinMessage = Delete("channels/{0:channel_id}/pins/{1:message_id}");

            public static readonly Route StartThreadWithMessage = Post("channels/{0:channel_id}/messages/{1:message_id}/threads");

            public static readonly Route StartThread = Post("channels/{0:channel_id}/threads");

            public static readonly Route JoinThread = Put("channels/{0:channel_id}/thread-members/@me");

            public static readonly Route AddThreadMember = Put("channels/{0:channel_id}/thread-members/{1:user_id}");

            public static readonly Route LeaveThread = Delete("channels/{0:channel_id}/thread-members/@me");

            public static readonly Route RemoveThreadMember = Delete("channels/{0:channel_id}/thread-members/{1:user_id}");

            public static readonly Route GetThreadMember = Get("channels/{0:channel_id}/thread-members/{1:user_id}");

            public static readonly Route ListThreadMembers = Get("channels/{0:channel_id}/thread-members");

            public static readonly Route ListPublicArchivedThreads = Get("channels/{0:channel_id}/threads/archived/public");

            public static readonly Route ListPrivateArchivedThreads = Get("channels/{0:channel_id}/threads/archived/private");

            public static readonly Route ListJoinedPrivateArchivedThreads = Get("channels/{0:channel_id}/users/@me/threads/archived/private");
        }

        public static class Emoji
        {
            public static readonly Route GetGuildEmojis = Get("guilds/{0:guild_id}/emojis");

            public static readonly Route GetGuildEmoji = Get("guilds/{0:guild_id}/emojis/{1:emoji_id}");

            public static readonly Route CreateGuildEmoji = Post("guilds/{0:guild_id}/emojis");

            public static readonly Route ModifyGuildEmoji = Patch("guilds/{0:guild_id}/emojis/{1:emoji_id}");

            public static readonly Route DeleteGuildEmoji = Delete("guilds/{0:guild_id}/emojis/{1:emoji_id}");
        }

        public static class Sticker
        {
            public static readonly Route GetSticker = Get("stickers/{0:sticker_id}");

            public static readonly Route GetStickerPacks = Get("sticker-packs");

            public static readonly Route GetGuildStickers = Get("guilds/{0:guild_id}/stickers");

            public static readonly Route GetGuildSticker = Get("guilds/{0:guild_id}/stickers/{1:sticker_id}");

            public static readonly Route CreateGuildSticker = Post("guilds/{0:guild_id}/stickers");

            public static readonly Route ModifyGuildSticker = Patch("guilds/{0:guild_id}/stickers/{1:sticker_id}");

            public static readonly Route DeleteGuildSticker = Delete("guilds/{0:guild_id}/stickers/{1:sticker_id}");
        }

        public static class Guild
        {
            public static readonly Route CreateGuild = Post("guilds");

            public static readonly Route GetGuild = Get("guilds/{0:guild_id}");

            public static readonly Route GetGuildPreview = Post("guilds/{0:guild_id}/preview");

            public static readonly Route ModifyGuild = Patch("guilds/{0:guild_id}");

            public static readonly Route DeleteGuild = Delete("guilds/{0:guild_id}");

            public static readonly Route GetChannels = Get("guilds/{0:guild_id}/channels");

            public static readonly Route CreateChannel = Post("guilds/{0:guild_id}/channels");

            public static readonly Route ReorderChannels = Patch("guilds/{0:guild_id}/channels");

            public static readonly Route ListActiveThreads = Get("guilds/{0:guild_id}/threads/active");

            public static readonly Route GetMember = Get("guilds/{0:guild_id}/members/{1:user_id}");

            public static readonly Route GetMembers = Get("guilds/{0:guild_id}/members");

            public static readonly Route SearchMembers = Get("guilds/{0:guild_id}/members/search");

            public static readonly Route AddMember = Put("guilds/{0:guild_id}/members/{1:user_id}");

            public static readonly Route ModifyMember = Patch("guilds/{0:guild_id}/members/{1:user_id}");

            public static readonly Route ModifyCurrentMember = Patch("guilds/{0:guild_id}/members/@me");

            public static readonly Route SetOwnNick = Patch("guilds/{0:guild_id}/members/@me/nick");

            public static readonly Route GrantRole = Put("guilds/{0:guild_id}/members/{1:user_id}/roles/{2:role_id}");

            public static readonly Route RevokeRole = Delete("guilds/{0:guild_id}/members/{1:user_id}/roles/{2:role_id}");

            public static readonly Route KickMember = Delete("guilds/{0:guild_id}/members/{1:user_id}");

            public static readonly Route GetBans = Get("guilds/{0:guild_id}/bans");

            public static readonly Route GetBan = Get("guilds/{0:guild_id}/bans/{1:user_id}");

            public static readonly Route CreateBan = Put("guilds/{0:guild_id}/bans/{1:user_id}");

            public static readonly Route DeleteBan = Delete("guilds/{0:guild_id}/bans/{1:user_id}");

            public static readonly Route GetRoles = Get("guilds/{0:guild_id}/roles");

            public static readonly Route CreateRole = Post("guilds/{0:guild_id}/roles");

            public static readonly Route ReorderRoles = Patch("guilds/{0:guild_id}/roles");

            public static readonly Route ModifyRole = Patch("guilds/{0:guild_id}/roles/{1:role_id}");

            public static readonly Route DeleteRole = Delete("guilds/{0:guild_id}/roles/{1:role_id}");

            public static readonly Route GetPruneCount = Get("guilds/{0:guild_id}/prune");

            public static readonly Route BeginPrune = Post("guilds/{0:guild_id}/prune");

            public static readonly Route GetVoiceRegions = Get("guilds/{0:guild_id}/regions");

            public static readonly Route GetInvites = Get("guilds/{0:guild_id}/invites");

            public static readonly Route GetIntegrations = Get("guilds/{0:guild_id}/integrations");

            public static readonly Route CreateIntegration = Post("guilds/{0:guild_id}/integrations");

            public static readonly Route ModifyIntegration = Patch("guilds/{0:guild_id}/integrations/{1:integration_id}");

            public static readonly Route DeleteIntegration = Delete("guilds/{0:guild_id}/integrations/{1:integration_id}");

            public static readonly Route SyncIntegration = Post("guilds/{0:guild_id}/integrations/{1:integration_id}/sync");

            public static readonly Route GetWidgetSettings = Get("guilds/{0:guild_id}/widget");

            public static readonly Route ModifyWidget = Patch("guilds/{0:guild_id}/widget");

            public static readonly Route GetWidget = Get("guilds/{0:guild_id}/widget.json");

            public static readonly Route GetVanityUrl = Get("guilds/{0:guild_id}/vanity-url");

            public static readonly Route GetWidgetImage = Get("guilds/{0:guild_id}/widget.png");

            public static readonly Route GetWelcomeScreen = Get("guilds/{0:guild_id}/welcome-screen");

            public static readonly Route ModifyWelcomeScreen = Patch("guilds/{0:guild_id}/welcome-screen");

            public static readonly Route ModifyCurrentMemberVoiceState = Patch("guilds/{0:guild_id}/voice-states/@me");

            public static readonly Route ModifyMemberVoiceState = Patch("guilds/{0:guild_id}/voice-states/{1:user_id}");
        }

        public static class GuildEvents
        {
            public static readonly Route GetEvents = Get("guilds/{0:guild_id}/scheduled-events");

            public static readonly Route CreateEvent = Post("guilds/{0:guild_id}/scheduled-events");

            public static readonly Route GetEvent = Get("guilds/{0:guild_id}/scheduled-events/{1:event_id}");

            public static readonly Route ModifyEvent = Patch("guilds/{0:guild_id}/scheduled-events/{1:event_id}");

            public static readonly Route DeleteEvent = Delete("guilds/{0:guild_id}/scheduled-events/{1:event_id}");

            public static readonly Route GetEventUsers = Get("guilds/{0:guild_id}/scheduled-events/{1:event_id}/users");
        }

        public static class Invite
        {
            public static readonly Route GetInvite = Get("invites/{0:invite_code}");

            public static readonly Route DeleteInvite = Delete("invites/{0:invite_code}");
        }

        public static class Stages
        {
            public static readonly Route CreateStage = Post("stage-instances");

            public static readonly Route FetchStage = Get("stage-instances/{0:channel_id}");

            public static readonly Route ModifyStage = Patch("stage-instances/{0:channel_id}");

            public static readonly Route DeleteStage = Delete("stage-instances/{0:channel_id}");
        }

        public static class Template
        {
            public static readonly Route GetTemplate = Get("guilds/templates/{0:template_code}");

            public static readonly Route CreateGuild = Post("guilds/templates/{0:template_code}");

            public static readonly Route GetTemplates = Get("guilds/{0:guild_id}/templates");

            public static readonly Route CreateTemplate = Post("guilds/{0:guild_id}/templates");

            public static readonly Route SyncTemplate = Put("guilds/{0:guild_id}/templates/{1:template_code}");

            public static readonly Route ModifyTemplate = Patch("guilds/{0:guild_id}/templates/{1:template_code}");

            public static readonly Route DeleteTemplate = Delete("guilds/{0:guild_id}/templates/{1:template_code}");
        }

        public static class User
        {
            public static readonly Route GetCurrentUser = Get("users/@me");

            public static readonly Route GetUser = Get("users/{0:user_id}");

            public static readonly Route ModifyCurrentUser = Patch("users/@me");

            public static readonly Route GetGuilds = Get("users/@me/guilds");

            public static readonly Route GetCurrentGuildMember = Get("users/@me/guilds/{0:guild_id}/member");

            public static readonly Route LeaveGuild = Delete("users/@me/guilds/{0:guild_id}");

            public static readonly Route CreateDirectChannel = Post("users/@me/channels");

            public static readonly Route GetConnections = Get("users/@me/connections");
        }

        public static class Voice
        {
            public static readonly Route GetVoiceRegions = Get("voice/regions");
        }

        public static class Webhook
        {
            public static readonly Route CreateWebhook = Post("channels/{0:channel_id}/webhooks");

            public static readonly Route GetChannelWebhooks = Get("channels/{0:channel_id}/webhooks");

            public static readonly Route GetGuildWebhooks = Get("guilds/{0:guild_id}/webhooks");

            public static readonly Route GetWebhook = Get("webhooks/{0:webhook_id}");

            public static readonly Route GetWebhookWithToken = Get("webhooks/{0:webhook_id}/{1:webhook_token}");

            public static readonly Route ModifyWebhook = Patch("webhooks/{0:webhook_id}");

            public static readonly Route ModifyWebhookWithToken = Patch("webhooks/{0:webhook_id}/{1:webhook_token}");

            public static readonly Route DeleteWebhook = Delete("webhooks/{0:webhook_id}");

            public static readonly Route DeleteWebhookWithToken = Delete("webhooks/{0:webhook_id}/{1:webhook_token}");

            public static readonly Route ExecuteWebhook = Post("webhooks/{0:webhook_id}/{1:webhook_token}");

            public static readonly Route ExecuteSlackWebhook = Post("webhooks/{0:webhook_id}/{1:webhook_token}/slack");

            public static readonly Route ExecuteGithubWebhook = Post("webhooks/{0:webhook_id}/{1:webhook_token}/github");

            public static readonly Route GetWebhookMessage = Get("webhooks/{0:webhook_id}/{1:webhook_token}/messages/{2:message_id}");

            public static readonly Route ModifyWebhookMessage = Patch("webhooks/{0:webhook_id}/{1:webhook_token}/messages/{2:message_id}");

            public static readonly Route DeleteWebhookMessage = Delete("webhooks/{0:webhook_id}/{1:webhook_token}/messages/{2:message_id}");
        }

        public static class Gateway
        {
            public static readonly Route GetGateway = Get("gateway");

            public static readonly Route GetBotGateway = Get("gateway/bot");
        }

        public static class OAuth2
        {
            public static readonly Route GetCurrentApplication = Get("oauth2/applications/@me");

            public static readonly Route GetCurrentAuthorization = Get("oauth2/@me");
        }

        public static class Interactions
        {
            public static readonly Route GetGlobalCommands = Get("applications/{0:application_id}/commands");

            public static readonly Route CreateGlobalCommand = Post("applications/{0:application_id}/commands");

            public static readonly Route GetGlobalCommand = Get("applications/{0:application_id}/commands/{1:command_id}");

            public static readonly Route ModifyGlobalCommand = Patch("applications/{0:application_id}/commands/{1:command_id}");

            public static readonly Route DeleteGlobalCommand = Delete("applications/{0:application_id}/commands/{1:command_id}");

            public static readonly Route GetGuildCommands = Get("applications/{0:application_id}/guilds/{1:guild_id}/commands");

            public static readonly Route SetGlobalCommands = Put("applications/{0:application_id}/commands");

            public static readonly Route CreateGuildCommand = Post("applications/{0:application_id}/guilds/{1:guild_id}/commands");

            public static readonly Route GetGuildCommand = Get("applications/{0:application_id}/guilds/{1:guild_id}/commands/{2:command_id}");

            public static readonly Route ModifyGuildCommand = Patch("applications/{0:application_id}/guilds/{1:guild_id}/commands/{2:command_id}");

            public static readonly Route DeleteGuildCommand = Delete("applications/{0:application_id}/guilds/{1:guild_id}/commands/{2:command_id}");

            public static readonly Route SetGuildCommands = Put("applications/{0:application_id}/guilds/{1:guild_id}/commands");

            public static readonly Route CreateInitialResponse = Post("interactions/{0:interaction_id}/{1:interaction_token}/callback");

            public static readonly Route GetInitialResponse = Get("webhooks/{0:application_id}/{1:interaction_token}/messages/@original");

            public static readonly Route ModifyInitialResponse = Patch("webhooks/{0:application_id}/{1:interaction_token}/messages/@original");

            public static readonly Route DeleteInitialResponse = Delete("webhooks/{0:application_id}/{1:interaction_token}/messages/@original");

            public static readonly Route CreateFollowupResponse = Post("webhooks/{0:application_id}/{1:interaction_token}");

            public static readonly Route GetFollowupResponse = Get("webhooks/{0:application_id}/{1:interaction_token}/messages/{2:message_id}");

            public static readonly Route ModifyFollowupResponse = Patch("webhooks/{0:application_id}/{1:interaction_token}/messages/{2:message_id}");

            public static readonly Route DeleteFollowupResponse = Delete("webhooks/{0:application_id}/{1:interaction_token}/messages/{2:message_id}");

            public static readonly Route GetAllCommandPermissions = Get("applications/{0:application_id}/guilds/{1:guild_id}/commands/permissions");

            public static readonly Route GetCommandPermissions = Get("applications/{0:application_id}/guilds/{1:guild_id}/commands/{2:command_id}/permissions");

            public static readonly Route SetCommandPermissions = Put("applications/{0:application_id}/guilds/{1:guild_id}/commands/{2:command_id}/permissions");

            public static readonly Route SetCommandsPermissions = Put("applications/{0:application_id}/guilds/{1:guild_id}/commands/permissions");
        }

        public static Route Get(string path)
            => new(HttpRequestMethod.Get, path);

        public static Route Post(string path)
            => new(HttpRequestMethod.Post, path);

        public static Route Patch(string path)
            => new(HttpRequestMethod.Patch, path);

        public static Route Put(string path)
            => new(HttpRequestMethod.Put, path);

        public static Route Delete(string path)
            => new(HttpRequestMethod.Delete, path);

#if DEBUG
        [ModuleInitializer]
        internal static void ModuleInitializer()
        {
            // Debug route testing code. Validates argument indices.
            var types = typeof(Route).GetNestedTypes();
            for (var i = 0; i < types.Length; i++)
            {
                var type = types[i];
                var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);
                for (var j = 0; j < fields.Length; j++)
                {
                    var field = fields[j];
                    var route = (Route) field.GetValue(null);
                    try
                    {
                        var pathSpan = route.Path.AsSpan();
                        var argumentIndex = '0';
                        int firstBracketIndex;
                        while ((firstBracketIndex = pathSpan.IndexOf('{')) != -1)
                        {
                            pathSpan = pathSpan.Slice(firstBracketIndex + 1);
                            var secondBracketIndex = pathSpan.IndexOf('}');
                            if (secondBracketIndex == -1)
                                Debug.Fail($"Route {type.Name}.{field.Name} contains an unmatched bracket.");

                            var segment = pathSpan.Slice(0, secondBracketIndex);
                            if (argumentIndex != segment[0])
                                Debug.Fail($"Route {type.Name}.{field.Name} contains duplicate argument index ({argumentIndex}).");

                            if (segment.Length > 1)
                            {
                                var nameSpan = segment[2..];
                            }
                            else
                            {
                                Debug.Write($"Route {type.Name}.{field.Name} contains an unnamed argument ({argumentIndex}).");
                            }

                            argumentIndex++;
                            pathSpan = pathSpan.Slice(secondBracketIndex + 1);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.Fail($"An exception occurred when parsing route {type.Name}.{field.Name}: {ex}");
                    }
                }
            }
        }
#endif
    }
}
