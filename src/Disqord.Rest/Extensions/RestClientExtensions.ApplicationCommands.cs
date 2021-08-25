using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Disqord.Rest.Api;
using Disqord.Collections;

namespace Disqord.Rest
{
    public static partial class RestClientExtensions
    {
        public static async Task<IReadOnlyList<IApplicationCommand>> FetchGlobalApplicationCommandsAsync(this IRestClient client, Snowflake applicationId, IRestRequestOptions options = null)
        {
            var models = await client.ApiClient.FetchGlobalApplicationCommandsAsync(applicationId, options).ConfigureAwait(false);
            return models.ToReadOnlyList(client, (x, client) => new TransientApplicationCommand(client, x));
        }

        public static Task<IApplicationCommand> CreateGlobalApplicationCommandAsync(this IRestClient client, Snowflake applicationId, CreateApplicationCommandJsonRestRequestContent content, IRestRequestOptions options = null)
        {

        }

        public static async Task<IApplicationCommand> FetchGlobalApplicationCommandAsync(this IRestClient client, Snowflake applicationId, Snowflake commandId, IRestRequestOptions options = null)
        {
            var model = await client.ApiClient.FetchGlobalApplicationCommandAsync(applicationId, commandId, options).ConfigureAwait(false);
            return new TransientApplicationCommand(client, model);
        }

        public static async Task<IApplicationCommand> ModifyGlobalApplicationCommandAsync(this IRestClient client, Snowflake applicationId, Snowflake commandId, Action<ModifyApplicationCommandActionProperties> action, IRestRequestOptions options = null)
        {
            var properties = new ModifyApplicationCommandActionProperties();
            action(properties);

            var content = new ModifyApplicationCommandJsonRestRequestContent
            {
                Name = properties.Name,
                Description = properties.Description,
                // Options = ...
                DefaultPermission = properties.EnabledByDefault
            };

            var model = await client.ApiClient.ModifyGlobalApplicationCommandAsync(applicationId, commandId, content, options).ConfigureAwait(false);
            return new TransientApplicationCommand(client, model);
        }

        public static Task DeleteGlobalApplicationCommandAsync(this IRestClient client, Snowflake applicationId, Snowflake commandId, IRestRequestOptions options = null)
            => client.ApiClient.DeleteGlobalApplicationCommandAsync(applicationId, commandId, options);

        public static async Task<IReadOnlyList<IApplicationCommand>> FetchGuildApplicationCommandsAsync(this IRestClient client, Snowflake applicationId, Snowflake guildId, IRestRequestOptions options = null)
        {
            var models = await client.ApiClient.FetchGuildApplicationCommandsAsync(applicationId, guildId, options).ConfigureAwait(false);
            return models.ToReadOnlyList(client, (x, client) => new TransientApplicationCommand(client, x));
        }

        public static Task<IReadOnlyList<IApplicationCommand>> SetGlobalApplicationCommandsAsync(this IRestClient client, Snowflake applicationId, JsonObjectRestRequestContent<ModifyApplicationCommandJsonRestRequestContent[]> content, IRestRequestOptions options = null)
        {
            
        }

        public static Task<IApplicationCommand> CreateGuildApplicationCommandAsync(this IRestClient client, Snowflake applicationId, Snowflake guildId, Action model, IRestRequestOptions options = null)
        {
            
        }

        public static async Task<IApplicationCommand> FetchGuildApplicationCommandAsync(this IRestClient client, Snowflake applicationId, Snowflake guildId, Snowflake commandId, IRestRequestOptions options = null)
        {
            var model = await client.ApiClient.FetchGuildApplicationCommandAsync(applicationId, guildId, commandId, options).ConfigureAwait(false);
            return new TransientApplicationCommand(client, model);
        }

        public static async Task<IApplicationCommand> ModifyGuildApplicationCommandAsync(this IRestClient client, Snowflake applicationId, Snowflake guildId, Snowflake commandId, Action<ModifyApplicationCommandActionProperties> action, IRestRequestOptions options = null)
        {
            var properties = new ModifyApplicationCommandActionProperties();
            action(properties);

            var content = new ModifyApplicationCommandJsonRestRequestContent
            {
                Name = properties.Name,
                Description = properties.Description,
                // Options = ...
                DefaultPermission = properties.EnabledByDefault
            };

            var model = await client.ApiClient.ModifyGuildApplicationCommandAsync(applicationId, guildId, commandId, content, options).ConfigureAwait(false);
            return new TransientApplicationCommand(client, model);
        }

        public static Task DeleteGuildApplicationCommandAsync(this IRestClient client, Snowflake applicationId, Snowflake guildId, Snowflake commandId, IRestRequestOptions options = null)
            => client.ApiClient.DeleteGuildApplicationCommandAsync(applicationId, guildId, commandId, options);

        public static Task<IReadOnlyList<IApplicationCommand>> SetGuildApplicationCommandsAsync(this IRestClient client, Snowflake applicationId, Snowflake guildId, JsonObjectRestRequestContent<ModifyApplicationCommandJsonRestRequestContent[]> content, IRestRequestOptions options = null)
        {

        }
    }
}
