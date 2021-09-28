using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Models;
using Disqord.Rest.Api;
using Qommon;
using Qommon.Collections;

namespace Disqord.Rest
{
    public static partial class RestClientExtensions
    {
        public static async Task<IReadOnlyList<IApplicationCommand>> FetchGlobalApplicationCommandsAsync(this IRestClient client,
            Snowflake applicationId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var models = await client.ApiClient.FetchGlobalApplicationCommandsAsync(applicationId, options, cancellationToken).ConfigureAwait(false);
            return models.ToReadOnlyList(client, (x, client) => TransientApplicationCommand.Create(client, x));
        }

        public static async Task<IApplicationCommand> CreateGlobalApplicationCommandAsync(this IRestClient client,
            Snowflake applicationId, LocalApplicationCommand command,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            Guard.IsNotNull(command);

            command.Validate();
            var content = new CreateApplicationCommandJsonRestRequestContent
            {
                Name = command.Name,
                DefaultPermission = command.IsEnabledByDefault
            };

            if (command is LocalSlashCommand slashCommand)
            {
                content.Type = ApplicationCommandType.Slash;
                content.Description = slashCommand.Description;
                content.Options = Optional.Conditional(slashCommand.Options.Count != 0, x => x.Select(x => x.ToModel(client.ApiClient.Serializer)).ToArray(), slashCommand.Options);
            }
            else if (command is LocalUserContextMenuCommand)
            {
                content.Type = ApplicationCommandType.User;
            }
            else if (command is LocalMessageContextMenuCommand)
            {
                content.Type = ApplicationCommandType.Message;
            }

            var model = await client.ApiClient.CreateGlobalApplicationCommandAsync(applicationId, content, options, cancellationToken).ConfigureAwait(false);
            return TransientApplicationCommand.Create(client, model);
        }

        public static async Task<IApplicationCommand> FetchGlobalApplicationCommandAsync(this IRestClient client,
            Snowflake applicationId, Snowflake commandId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var model = await client.ApiClient.FetchGlobalApplicationCommandAsync(applicationId, commandId, options, cancellationToken).ConfigureAwait(false);
            return TransientApplicationCommand.Create(client, model);
        }

        public static async Task<IApplicationCommand> ModifyGlobalApplicationCommandAsync(this IRestClient client,
            Snowflake applicationId, Snowflake commandId, Action<ModifyApplicationCommandActionProperties> action,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            Guard.IsNotNull(action);

            var properties = new ModifyApplicationCommandActionProperties();
            action(properties);

            var content = new ModifyApplicationCommandJsonRestRequestContent
            {
                Name = properties.Name,
                Description = properties.Description,
                DefaultPermission = properties.IsEnabledByDefault
            };

            if (properties.Options.HasValue)
                content.Options = properties.Options.Value.Select(x => x.ToModel(client.ApiClient.Serializer)).ToArray();

            var model = await client.ApiClient.ModifyGlobalApplicationCommandAsync(applicationId, commandId, content, options, cancellationToken).ConfigureAwait(false);
            return TransientApplicationCommand.Create(client, model);
        }

        public static Task DeleteGlobalApplicationCommandAsync(this IRestClient client,
            Snowflake applicationId, Snowflake commandId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
            => client.ApiClient.DeleteGlobalApplicationCommandAsync(applicationId, commandId, options, cancellationToken);

        public static async Task<IReadOnlyList<IApplicationCommand>> FetchGuildApplicationCommandsAsync(this IRestClient client,
            Snowflake applicationId, Snowflake guildId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var models = await client.ApiClient.FetchGuildApplicationCommandsAsync(applicationId, guildId, options, cancellationToken).ConfigureAwait(false);
            return models.ToReadOnlyList(client, (x, client) => TransientApplicationCommand.Create(client, x));
        }

        public static async Task<IReadOnlyList<IApplicationCommand>> SetGlobalApplicationCommandsAsync(this IRestClient client,
            Snowflake applicationId, IEnumerable<LocalApplicationCommand> commands,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var contents = commands.Select(x =>
            {
                var content = new CreateApplicationCommandJsonRestRequestContent
                {
                    Name = x.Name,
                    DefaultPermission = x.IsEnabledByDefault
                };

                if (x is LocalSlashCommand slashCommand)
                {
                    content.Type = ApplicationCommandType.Slash;
                    content.Description = slashCommand.Description;
                    content.Options = Optional.Conditional(slashCommand.Options.Count != 0, x => x.Select(x => x.ToModel(client.ApiClient.Serializer)).ToArray(), slashCommand.Options);
                }
                else if (x is LocalUserContextMenuCommand)
                {
                    content.Type = ApplicationCommandType.User;
                }
                else if (x is LocalMessageContextMenuCommand)
                {
                    content.Type = ApplicationCommandType.Message;
                }

                return content;
            }).ToArray();

            var models = await client.ApiClient.SetGlobalApplicationCommandsAsync(applicationId, new JsonObjectRestRequestContent<CreateApplicationCommandJsonRestRequestContent[]>(contents), options, cancellationToken);
            return models.ToReadOnlyList(client, (x, client) => TransientApplicationCommand.Create(client, x));
        }

        public static async Task<IApplicationCommand> CreateGuildApplicationCommandAsync(this IRestClient client,
            Snowflake applicationId, Snowflake guildId, LocalApplicationCommand command,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            Guard.IsNotNull(command);

            command.Validate();
            var content = new CreateApplicationCommandJsonRestRequestContent
            {
                Name = command.Name,
                DefaultPermission = command.IsEnabledByDefault
            };

            if (command is LocalSlashCommand slashCommand)
            {
                content.Type = ApplicationCommandType.Slash;
                content.Description = slashCommand.Description;
                content.Options = Optional.Conditional(slashCommand.Options.Count != 0, x => x.Select(x => x.ToModel(client.ApiClient.Serializer)).ToArray(), slashCommand.Options);
            }
            else if (command is LocalUserContextMenuCommand)
            {
                content.Type = ApplicationCommandType.User;
            }
            else if (command is LocalMessageContextMenuCommand)
            {
                content.Type = ApplicationCommandType.Message;
            }

            var model = await client.ApiClient.CreateGuildApplicationCommandAsync(applicationId, guildId, content, options, cancellationToken).ConfigureAwait(false);
            return TransientApplicationCommand.Create(client, model);
        }

        public static async Task<IApplicationCommand> FetchGuildApplicationCommandAsync(this IRestClient client,
            Snowflake applicationId, Snowflake guildId, Snowflake commandId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var model = await client.ApiClient.FetchGuildApplicationCommandAsync(applicationId, guildId, commandId, options, cancellationToken).ConfigureAwait(false);
            return TransientApplicationCommand.Create(client, model);
        }

        public static async Task<IApplicationCommand> ModifyGuildApplicationCommandAsync(this IRestClient client,
            Snowflake applicationId, Snowflake guildId, Snowflake commandId, Action<ModifyApplicationCommandActionProperties> action,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            Guard.IsNotNull(action);

            var properties = new ModifyApplicationCommandActionProperties();
            action(properties);

            var content = new ModifyApplicationCommandJsonRestRequestContent
            {
                Name = properties.Name,
                Description = properties.Description,
                DefaultPermission = properties.IsEnabledByDefault
            };

            if (properties.Options.HasValue)
                content.Options = properties.Options.Value.Select(x => x.ToModel(client.ApiClient.Serializer)).ToArray();

            var model = await client.ApiClient.ModifyGuildApplicationCommandAsync(applicationId, guildId, commandId, content, options, cancellationToken).ConfigureAwait(false);
            return TransientApplicationCommand.Create(client, model);
        }

        public static Task DeleteGuildApplicationCommandAsync(this IRestClient client,
            Snowflake applicationId, Snowflake guildId, Snowflake commandId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
            => client.ApiClient.DeleteGuildApplicationCommandAsync(applicationId, guildId, commandId, options, cancellationToken);

        public static async Task<IReadOnlyList<IApplicationCommand>> SetGuildApplicationCommandsAsync(this IRestClient client,
            Snowflake applicationId, Snowflake guildId, IEnumerable<LocalApplicationCommand> commands,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var contents = commands.Select(x =>
            {
                var content = new CreateApplicationCommandJsonRestRequestContent
                {
                    Name = x.Name,
                    DefaultPermission = x.IsEnabledByDefault
                };

                if (x is LocalSlashCommand slashCommand)
                {
                    content.Type = ApplicationCommandType.Slash;
                    content.Description = slashCommand.Description;
                    content.Options = Optional.Conditional(slashCommand.Options.Count != 0, x => x.Select(x => x.ToModel(client.ApiClient.Serializer)).ToArray(), slashCommand.Options);
                }
                else if (x is LocalUserContextMenuCommand)
                {
                    content.Type = ApplicationCommandType.User;
                }
                else if (x is LocalMessageContextMenuCommand)
                {
                    content.Type = ApplicationCommandType.Message;
                }

                return content;
            }).ToArray();

            var models = await client.ApiClient.SetGuildApplicationCommandsAsync(applicationId, guildId, new JsonObjectRestRequestContent<CreateApplicationCommandJsonRestRequestContent[]>(contents), options, cancellationToken);
            return models.ToReadOnlyList(client, (x, client) => TransientApplicationCommand.Create(client, x));
        }
    }
}
