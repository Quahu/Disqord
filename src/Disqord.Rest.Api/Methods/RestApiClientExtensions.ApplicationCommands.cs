using System.Threading.Tasks;
using Disqord.Models;

namespace Disqord.Rest.Api
{
    public static partial class RestApiClientExtensions
    {
        public static Task<ApplicationCommandJsonModel[]> FetchGlobalApplicationCommandsAsync(this IRestApiClient client, Snowflake applicationId, IRestRequestOptions options = null)
        {
            var route = Format(Route.Interactions.GetGlobalCommands, applicationId);
            return client.ExecuteAsync<ApplicationCommandJsonModel[]>(route, null, options);
        }
        
        public static Task<ApplicationCommandJsonModel> CreateGlobalApplicationCommandAsync(this IRestApiClient client, Snowflake applicationId, CreateApplicationCommandJsonRestRequestContent content, IRestRequestOptions options = null)
        {
            var route = Format(Route.Interactions.CreateGlobalCommand, applicationId);
            return client.ExecuteAsync<ApplicationCommandJsonModel>(route, content, options);
        }
        
        public static Task<ApplicationCommandJsonModel> FetchGlobalApplicationCommandAsync(this IRestApiClient client, Snowflake applicationId, Snowflake commandId, IRestRequestOptions options = null)
        {
            var route = Format(Route.Interactions.GetGlobalCommand, applicationId, commandId);
            return client.ExecuteAsync<ApplicationCommandJsonModel>(route, null, options);
        }
        
        public static Task<ApplicationCommandJsonModel> ModifyGlobalApplicationCommandAsync(this IRestApiClient client, Snowflake applicationId, Snowflake commandId, ModifyApplicationCommandJsonRestRequestContent content, IRestRequestOptions options = null)
        {
            var route = Format(Route.Interactions.ModifyGlobalCommand, applicationId, commandId);
            return client.ExecuteAsync<ApplicationCommandJsonModel>(route, content, options);
        }
        
        public static Task DeleteGlobalApplicationCommandAsync(this IRestApiClient client, Snowflake applicationId, Snowflake commandId, IRestRequestOptions options = null)
        {
            var route = Format(Route.Interactions.DeleteGlobalCommand, applicationId, commandId);
            return client.ExecuteAsync(route, null, options);
        }
        
        public static Task<ApplicationCommandJsonModel[]> FetchGuildApplicationCommandsAsync(this IRestApiClient client, Snowflake applicationId, Snowflake guildId, IRestRequestOptions options = null)
        {
            var route = Format(Route.Interactions.GetGuildCommands, applicationId, guildId);
            return client.ExecuteAsync<ApplicationCommandJsonModel[]>(route, null, options);
        }

        public static Task<ApplicationCommandJsonModel[]> ModifyGlobalApplicationCommandsAsync(this IRestApiClient client, Snowflake applicationId, JsonObjectRestRequestContent<ModifyApplicationCommandJsonRestRequestContent[]> content, IRestRequestOptions options = null)
        {
            var route = Format(Route.Interactions.ModifyGlobalCommands, applicationId);
            return client.ExecuteAsync<ApplicationCommandJsonModel[]>(route, content, options);
        }
        
        public static Task<ApplicationCommandJsonModel> CreateGuildApplicationCommandAsync(this IRestApiClient client, Snowflake applicationId, Snowflake guildId, CreateApplicationCommandJsonRestRequestContent content, IRestRequestOptions options = null)
        {
            var route = Format(Route.Interactions.CreateGuildCommand, applicationId, guildId);
            return client.ExecuteAsync<ApplicationCommandJsonModel>(route, content, options);
        }
        
        public static Task<ApplicationCommandJsonModel> FetchGuildApplicationCommandAsync(this IRestApiClient client, Snowflake applicationId, Snowflake guildId, Snowflake commandId, IRestRequestOptions options = null)
        {
            var route = Format(Route.Interactions.GetGuildCommand, applicationId, guildId, commandId);
            return client.ExecuteAsync<ApplicationCommandJsonModel>(route, null, options);
        }
        
        public static Task<ApplicationCommandJsonModel> ModifyGuildApplicationCommandAsync(this IRestApiClient client, Snowflake applicationId, Snowflake guildId, Snowflake commandId, ModifyApplicationCommandJsonRestRequestContent content, IRestRequestOptions options = null)
        {
            var route = Format(Route.Interactions.ModifyGuildCommand, applicationId, guildId, commandId);
            return client.ExecuteAsync<ApplicationCommandJsonModel>(route, content, options);
        }
        
        public static Task DeleteGuildApplicationCommandAsync(this IRestApiClient client, Snowflake applicationId, Snowflake guildId, Snowflake commandId, IRestRequestOptions options = null)
        {
            var route = Format(Route.Interactions.DeleteGuildCommand, applicationId, guildId, commandId);
            return client.ExecuteAsync(route, null, options);
        }
        
        public static Task<ApplicationCommandJsonModel[]> ModifyGuildApplicationCommandsAsync(this IRestApiClient client, Snowflake applicationId, Snowflake guildId, JsonObjectRestRequestContent<ModifyApplicationCommandJsonRestRequestContent[]> content, IRestRequestOptions options = null)
        {
            var route = Format(Route.Interactions.ModifyGuildCommands, applicationId, guildId);
            return client.ExecuteAsync<ApplicationCommandJsonModel[]>(route, content, options);
        }

        public static Task<GuildApplicationCommandPermissionsJsonModel[]> FetchApplicationCommandPermissionsAsync(this IRestApiClient client, Snowflake applicationId, Snowflake guildId, IRestRequestOptions options = null)
        {
            var route = Format(Route.Interactions.GetAllCommandPermissions, applicationId, guildId);
            return client.ExecuteAsync<GuildApplicationCommandPermissionsJsonModel[]>(route, null, options);
        }
        
        public static Task<GuildApplicationCommandPermissionsJsonModel> FetchApplicationCommandPermissionsAsync(this IRestApiClient client, Snowflake applicationId, Snowflake guildId, Snowflake commandId, IRestRequestOptions options = null)
        {
            var route = Format(Route.Interactions.GetCommandPermissions, applicationId, guildId, commandId);
            return client.ExecuteAsync<GuildApplicationCommandPermissionsJsonModel>(route, null, options);
        }

        public static Task<GuildApplicationCommandPermissionsJsonModel> ModifyApplicationCommandPermissionsAsync(this IRestApiClient client, Snowflake applicationId, Snowflake guildId, Snowflake commandId, ModifyApplicationCommandPermissionsJsonRestRequestContent content, IRestRequestOptions options = null)
        {
            var route = Format(Route.Interactions.ModifyCommandPermissions, applicationId, guildId, commandId);
            return client.ExecuteAsync<GuildApplicationCommandPermissionsJsonModel>(route, content, options);
        }

        public static Task<GuildApplicationCommandPermissionsJsonModel[]> ModifyApplicationCommandPermissionsAsync(this IRestApiClient client, Snowflake applicationId, Snowflake guildId, JsonObjectRestRequestContent<ModifyApplicationCommandPermissionsJsonRestRequestContent[]> content, IRestRequestOptions options = null)
        {
            var route = Format(Route.Interactions.ModifyAllCommandPermissions, applicationId, guildId);
            return client.ExecuteAsync<GuildApplicationCommandPermissionsJsonModel[]>(route, content, options);
        }
    }
}
