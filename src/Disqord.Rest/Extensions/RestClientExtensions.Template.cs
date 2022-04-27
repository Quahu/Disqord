using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Rest.Api;
using Qommon;
using Qommon.Collections;
using Qommon.Collections.ReadOnly;

namespace Disqord.Rest
{
    public partial class RestClientExtensions
    {
        public static async Task<IGuildTemplate> FetchTemplateAsync(this IRestClient client,
            string templateCode,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var model = await client.ApiClient.FetchGuildTemplateAsync(templateCode, options, cancellationToken).ConfigureAwait(false);
            return new TransientGuildTemplate(client, model);
        }

        public static async Task<IGuild> CreateGuildFromTemplateAsync(this IRestClient client,
            string templateCode, string name, Action<CreateGuildFromTemplateActionProperties> action = null,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var properties = new CreateGuildFromTemplateActionProperties();
            action?.Invoke(properties);

            var content = new CreateGuildFromTemplateJsonRestRequestContent(name)
            {
                Icon = properties.Icon
            };

            var model = await client.ApiClient.CreateGuildFromTemplateAsync(templateCode, content, options, cancellationToken).ConfigureAwait(false);
            return new TransientGuild(client, model);
        }

        public static async Task<IReadOnlyList<IGuildTemplate>> FetchTemplatesAsync(this IRestClient client,
            Snowflake guildId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var model = await client.ApiClient.FetchGuildTemplatesAsync(guildId, options, cancellationToken).ConfigureAwait(false);
            return model.ToReadOnlyList(client, static (x, client) => new TransientGuildTemplate(client, x));
        }

        public static async Task<IGuildTemplate> CreateTemplateAsync(this IRestClient client,
            Snowflake guildId, string name, Action<CreateTemplateActionProperties> action = null,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var properties = new CreateTemplateActionProperties();
            action?.Invoke(properties);

            var content = new CreateGuildTemplateJsonRestRequestContent(name)
            {
                Description = properties.Description
            };

            var model = await client.ApiClient.CreateGuildTemplateAsync(guildId, content, options, cancellationToken).ConfigureAwait(false);
            return new TransientGuildTemplate(client, model);
        }

        public static async Task<IGuildTemplate> SynchronizeTemplateAsync(this IRestClient client,
            Snowflake guildId, string templateCode,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var model = await client.ApiClient.SynchronizeGuildTemplateAsync(guildId, templateCode, options, cancellationToken).ConfigureAwait(false);
            return new TransientGuildTemplate(client, model);
        }

        public static async Task<IGuildTemplate> ModifyTemplateAsync(this IRestClient client,
            Snowflake guildId, string templateCode, Action<ModifyTemplateActionProperties> action,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            Guard.IsNotNull(action);

            var properties = new ModifyTemplateActionProperties();
            action?.Invoke(properties);

            var content = new ModifyGuildTemplateJsonRestRequestContent
            {
                Name = properties.Name,
                Description = properties.Description
            };

            var model = await client.ApiClient.ModifyGuildTemplateAsync(guildId, templateCode, content, options, cancellationToken).ConfigureAwait(false);
            return new TransientGuildTemplate(client, model);
        }

        public static async Task<IGuildTemplate> DeleteTemplateAsync(this IRestClient client,
            Snowflake guildId, string templateCode,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var model = await client.ApiClient.DeleteGuildTemplateAsync(guildId, templateCode, options, cancellationToken).ConfigureAwait(false);
            return new TransientGuildTemplate(client, model);
        }
    }
}
