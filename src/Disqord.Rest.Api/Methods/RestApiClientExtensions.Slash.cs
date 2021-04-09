using System.Threading.Tasks;
using Disqord.Models.Slash;

namespace Disqord.Rest.Api
{
    public static partial class RestApiClientExtensions
    {
        public static Task<SlashCommandJsonModel[]> FetchGlobalSlashCommandsAsync(this IRestApiClient client, Snowflake applicationId, IRestRequestOptions options = null)
        {
            var route = Format(Route.Slash.GetGlobalCommands, applicationId);
            return client.ExecuteAsync<SlashCommandJsonModel[]>(route, null, options);
        }

        public static Task<SlashCommandJsonModel> CreateGlobalSlashCommandAsync(this IRestApiClient client, Snowflake applicationId, CreateSlashCommandJsonRestRequestContent content, IRestRequestOptions options = null)
        {
            var route = Format(Route.Slash.CreateGlobalCommand, applicationId);
            return client.ExecuteAsync<SlashCommandJsonModel>(route, content, options);
        }

        public static Task<SlashCommandJsonModel> ModifyGlobalSlashCommandAsync(this IRestApiClient client, Snowflake applicationId, Snowflake commandId, ModifySlashCommandJsonRestRequestContent content, IRestRequestOptions options = null)
        {
            var route = Format(Route.Slash.ModifyGlobalCommand, applicationId, commandId);
            return client.ExecuteAsync<SlashCommandJsonModel>(route, content, options);
        }

        public static Task DeleteGlobalSlashCommandAsync(this IRestApiClient client, Snowflake applicationId, Snowflake commandId, IRestRequestOptions options = null)
        {
            var route = Format(Route.Slash.DeleteGuildCommand, applicationId, commandId);
            return client.ExecuteAsync(route, null, options);
        }

        public static Task<SlashCommandJsonModel[]> FetchGuildSlashCommandsAsync(this IRestApiClient client, Snowflake applicationId, Snowflake guildId, IRestRequestOptions options = null)
        {
            var route = Format(Route.Slash.GetGuildCommands, applicationId, guildId);
            return client.ExecuteAsync<SlashCommandJsonModel[]>(route, null, options);
        }

        public static Task<SlashCommandJsonModel> CreateGuildSlashCommandAsync(this IRestApiClient client, Snowflake applicationId, Snowflake guildId, CreateSlashCommandJsonRestRequestContent content, IRestRequestOptions options = null)
        {
            var route = Format(Route.Slash.CreateGuildCommand, applicationId, guildId);
            return client.ExecuteAsync<SlashCommandJsonModel>(route, content, options);
        }

        public static Task<SlashCommandJsonModel> ModifyGuildSlashCommandAsync(this IRestApiClient client, Snowflake applicationId, Snowflake guildId, Snowflake commandId, ModifySlashCommandJsonRestRequestContent content, IRestRequestOptions options = null)
        {
            var route = Format(Route.Slash.ModifyGuildCommand, applicationId, guildId, commandId);
            return client.ExecuteAsync<SlashCommandJsonModel>(route, content, options);
        }

        public static Task DeleteGuildSlashCommandAsync(this IRestApiClient client, Snowflake applicationId, Snowflake guildId, Snowflake commandId, IRestRequestOptions options = null)
        {
            var route = Format(Route.Slash.DeleteGuildCommand, applicationId, guildId, commandId);
            return client.ExecuteAsync(route, null, options);
        }
    }
}
