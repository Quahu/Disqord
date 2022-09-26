using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Bot.Commands;
using Disqord.Bot.Commands.Application;
using Disqord.Gateway;
using Disqord.Rest;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Qmmands;
using Qommon;
using Qommon.Collections;

namespace Disqord.Bot;

public abstract partial class DiscordBotBase
{
    /// <summary>
    ///     Gets or sets the slash command name regex used to validate name transformations.
    /// </summary>
    protected Regex SlashCommandNameRegex { get; set; } = new(@"^[-_\p{L}\p{N}\p{Sc}]{1,32}$");

    /// <summary>
    ///     Transforms the given command name to the slash command compatible equivalent.
    /// </summary>
    /// <remarks>
    ///     By default the value is transformed to lower-case.
    /// </remarks>
    /// <param name="value"> The value to transform. </param>
    /// <returns>
    ///     The transformed name.
    /// </returns>
    protected virtual string TransformSlashCommandName(string value)
    {
        return value.ToLowerInvariant();
    }

    /// <summary>
    ///     Gets a slash command name for the given command name.
    /// </summary>
    /// <remarks>
    ///     By default the value is transformed using <see cref="TransformSlashCommandName"/>
    ///     and then validated using <see cref="SlashCommandNameRegex"/>.
    /// </remarks>
    /// <param name="value"> The value to get the name for. </param>
    /// <returns>
    ///     The slash command name.
    /// </returns>
    protected virtual string GetSlashCommandName(string value)
    {
        var transformedValue = TransformSlashCommandName(value);
        if (!SlashCommandNameRegex.IsMatch(transformedValue))
            Throw.InvalidOperationException($"The transformed slash command name '{transformedValue}' does not match the regex '{SlashCommandNameRegex}'.");

        return transformedValue;
    }

    /// <summary>
    ///     Localizes the given global and guild application commands.
    /// </summary>
    /// <remarks>
    ///     By default attempts to use the <see cref="IApplicationCommandLocalizer"/> service.
    /// </remarks>
    /// <param name="globalCommands"> The global commands to localize. </param>
    /// <param name="guildCommands"> The guild commands to localize. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    protected virtual async ValueTask LocalizeApplicationCommands(IEnumerable<LocalApplicationCommand> globalCommands,
        IReadOnlyDictionary<Snowflake, IEnumerable<LocalApplicationCommand>> guildCommands, CancellationToken cancellationToken)
    {
        var localizer = Services.GetService<IApplicationCommandLocalizer>();
        if (localizer == null)
            return;

        try
        {
            await localizer.LocalizeAsync(globalCommands, guildCommands, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An exception occurred during the localization of application commands.");
        }
    }

    /// <summary>
    ///     Converts the application command map's structure to global and guild application commands.
    /// </summary>
    /// <param name="map"> The map to convert. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A tuple of global and guild commands.
    /// </returns>
    /// <exception cref="InvalidOperationException"> Thrown when the conversion fails. </exception>
    protected virtual (IEnumerable<LocalApplicationCommand> GlobalCommands, IReadOnlyDictionary<Snowflake, IEnumerable<LocalApplicationCommand>> GuildCommands)
        ConvertApplicationCommandMap(ApplicationCommandMap map, CancellationToken cancellationToken)
    {
        var globalCommands = new FastList<LocalApplicationCommand>();
        var guildCommands = new Dictionary<Snowflake, IEnumerable<LocalApplicationCommand>>();

        // var commandCache = new Dictionary<ApplicationCommand, LocalApplicationCommand>();

        static void GetCommands( /*Dictionary<ApplicationCommand, LocalApplicationCommand> commandCache,*/
            ApplicationCommandMap.TopLevelNode topLevelNode, FastList<LocalApplicationCommand> localCommands)
        {
            static void AddCommand( /*Dictionary<ApplicationCommand, LocalApplicationCommand> commandCache,*/
                FastList<LocalApplicationCommand> localCommands, ApplicationCommand command)
            {
                static void GetPermissions(IReadOnlyList<ICheck> checks, ref Optional<Permissions> requiredMemberPermissions, ref Optional<bool> isEnabledInPrivateChannels)
                {
                    if (checks.Count == 0)
                        return;

                    foreach (var group in checks.GroupBy(x => x.Group))
                    {
                        if (group.Key == null)
                        {
                            foreach (var check in group)
                            {
                                if (check is RequireAuthorPermissionsAttribute requireAuthorPermissionsAttribute)
                                {
                                    requiredMemberPermissions = requiredMemberPermissions.GetValueOrDefault() | requireAuthorPermissionsAttribute.Permissions;
                                    continue;
                                }

                                if (check is RequireGuildAttribute)
                                {
                                    if (isEnabledInPrivateChannels.HasValue && isEnabledInPrivateChannels.Value)
                                        throw new InvalidOperationException($"Cannot apply {nameof(RequireGuildAttribute)} if other checks contain {nameof(RequirePrivateAttribute)}.");

                                    isEnabledInPrivateChannels = false;
                                    continue;
                                }

                                if (check is RequirePrivateAttribute)
                                {
                                    if (isEnabledInPrivateChannels.HasValue && !isEnabledInPrivateChannels.Value)
                                        throw new InvalidOperationException($"Cannot apply {nameof(RequirePrivateAttribute)} if other checks contain {nameof(RequireGuildAttribute)}.");

                                    isEnabledInPrivateChannels = true;
                                    continue;
                                }
                            }
                        }
                        else
                        {
                            var groupIsEnabledInPrivateChannels = Optional<bool>.Empty;
                            foreach (var check in group)
                            {
                                if (check is RequireGuildAttribute)
                                {
                                    groupIsEnabledInPrivateChannels = groupIsEnabledInPrivateChannels.HasValue && groupIsEnabledInPrivateChannels.Value
                                        ? Optional<bool>.Empty
                                        : false;

                                    continue;
                                }

                                if (check is RequirePrivateAttribute)
                                {
                                    groupIsEnabledInPrivateChannels = groupIsEnabledInPrivateChannels.HasValue && !groupIsEnabledInPrivateChannels.Value
                                        ? Optional<bool>.Empty
                                        : true;
                                }
                            }

                            if (groupIsEnabledInPrivateChannels.HasValue && isEnabledInPrivateChannels.HasValue)
                            {
                                if (groupIsEnabledInPrivateChannels.Value != isEnabledInPrivateChannels.Value)
                                    throw new InvalidOperationException($"Cannot apply {(groupIsEnabledInPrivateChannels.Value ? nameof(RequirePrivateAttribute) : nameof(RequireGuildAttribute))} if other checks contain {(groupIsEnabledInPrivateChannels.Value ? nameof(RequireGuildAttribute) : nameof(RequirePrivateAttribute))}.");

                                isEnabledInPrivateChannels = groupIsEnabledInPrivateChannels;
                            }
                        }
                    }
                }

                var modules = new FastList<ApplicationModule>();
                var module = command.Module;
                do
                {
                    modules.Add(module);
                    module = module.Parent;
                }
                while (module != null);

                var requiredMemberPermissions = Optional<Permissions>.Empty;
                var isEnabledInPrivateChannels = Optional<bool>.Empty;
                var hasModuleAlias = false;
                for (var i = 0; i < modules.Count; i++)
                {
                    module = modules[i];
                    GetPermissions(module.Checks, ref requiredMemberPermissions, ref isEnabledInPrivateChannels);

                    if (module.Alias != null)
                    {
                        hasModuleAlias = true;
                        break;
                    }
                }

                if (!hasModuleAlias)
                    GetPermissions(command.Checks, ref requiredMemberPermissions, ref isEnabledInPrivateChannels);

                // TODO: use the cache for slash commands
                if (command.Type is ApplicationCommandType.User or ApplicationCommandType.Message)
                {
                    /*if (commandCache.TryGetValue(command, out var cachedLocalContextMenuCommand))
                    {
                        localCommands.Add(cachedLocalContextMenuCommand.Clone());
                        return;
                    }
                    */

                    LocalApplicationCommand contextMenuCommand = command.Type is ApplicationCommandType.User
                        ? new LocalUserContextMenuCommand()
                        : new LocalMessageContextMenuCommand();

                    contextMenuCommand.Name = command.Alias;
                    contextMenuCommand.IsEnabledInPrivateChannels = isEnabledInPrivateChannels;
                    contextMenuCommand.DefaultRequiredMemberPermissions = requiredMemberPermissions;
                    localCommands.Add(contextMenuCommand);

                    // commandCache.Add(command, contextMenuCommand);
                    return;
                }

                var names = new FastList<(string Alias, string? Description)>();
                module = command.Module;
                names.Add((command.Alias, command.Description));
                do
                {
                    if (module.Alias != null)
                        names.Add((module.Alias, module.Description));

                    module = module.Parent;
                }
                while (module != null);

                var nameCount = names.Count;

                // var isCached = commandCache.TryGetValue(command, out var cachedLocalApplicationCommand);
                LocalSlashCommand? localSlashCommand = null; /*= cachedLocalApplicationCommand as LocalSlashCommand;*/
                var commandCount = localCommands.Count;
                for (var i = 0; i < commandCount; i++)
                {
                    var existingLocalCommand = localCommands[i];
                    if (existingLocalCommand is not LocalSlashCommand existingLocalSlashCommand)
                        continue;

                    var alias = names[^1].Alias;
                    if (existingLocalSlashCommand.Name != alias)
                        continue;

                    if (nameCount == 1)
                        throw new InvalidOperationException($"Duplicate top-level application command alias '{alias}' encountered.");

                    localSlashCommand = existingLocalSlashCommand;
                    break;
                }

                if (localSlashCommand == null)
                {
                    localSlashCommand = new LocalSlashCommand();
                    localSlashCommand.DefaultRequiredMemberPermissions = requiredMemberPermissions;
                    localSlashCommand.IsEnabledInPrivateChannels = isEnabledInPrivateChannels;
                    localCommands.Add(localSlashCommand);

                    // commandCache.Add(command, localSlashCommand);
                }

                LocalSlashCommandOption? lastAliasOption = null;
                for (var i = nameCount - 1; i >= 0; i--)
                {
                    var name = names[i];
                    if (!localSlashCommand.Name.HasValue)
                    {
                        localSlashCommand.Name = name.Alias!;
                        localSlashCommand.Description = name.Description ?? "No description.";
                        continue;
                    }

                    if (i == nameCount - 1)
                        continue;

                    IList<LocalSlashCommandOption> options;
                    if (lastAliasOption == null)
                    {
                        if (!localSlashCommand.Options.TryGetValue(out options!))
                            localSlashCommand.Options = new(options = new List<LocalSlashCommandOption>());
                    }
                    else
                    {
                        if (!lastAliasOption.Options.TryGetValue(out options!))
                            lastAliasOption.Options = new(options = new List<LocalSlashCommandOption>());
                    }

                    lastAliasOption = null;
                    var optionCount = options.Count;
                    for (var j = 0; j < optionCount; j++)
                    {
                        var existingOption = options[j];
                        if (existingOption.Name != name.Alias)
                            continue;

                        if ( /*!isCached && */i != nameCount - 1 && existingOption.Type != SlashCommandOptionType.SubcommandGroup)
                            throw new InvalidOperationException($"Duplicate application subcommand group alias '{name.Alias}' encountered.");

                        if ( /*!isCached && */i == nameCount - 1)
                            throw new InvalidOperationException($"Duplicate application subcommand alias '{name.Alias}' encountered.");

                        lastAliasOption = existingOption;
                        break;
                    }

                    if (lastAliasOption != null)
                        continue;

                    var localSlashCommandOption = new LocalSlashCommandOption
                    {
                        Name = name.Alias,
                        Description = name.Description ?? "No description.",
                        Type = i != 0
                            ? SlashCommandOptionType.SubcommandGroup
                            : SlashCommandOptionType.Subcommand
                    };

                    options.Add(localSlashCommandOption);
                    lastAliasOption = localSlashCommandOption;
                }

                var parameters = command.Parameters;
                var parameterCount = parameters.Count;
                if ( /*!isCached && */parameterCount != 0)
                {
                    IList<LocalSlashCommandOption> parameterOptions;
                    if (lastAliasOption == null)
                    {
                        if (!localSlashCommand.Options.TryGetValue(out parameterOptions!))
                            localSlashCommand.Options = new(parameterOptions = new List<LocalSlashCommandOption>());
                    }
                    else
                    {
                        if (!lastAliasOption.Options.TryGetValue(out parameterOptions!))
                            lastAliasOption.Options = new(parameterOptions = new List<LocalSlashCommandOption>());
                    }

                    for (var i = 0; i < parameterCount; i++)
                    {
                        static LocalSlashCommandOption GetOption(ApplicationParameter parameter)
                        {
                            var typeInformation = parameter.GetTypeInformation();
                            var option = new LocalSlashCommandOption
                            {
                                Name = parameter.Name,
                                Description = parameter.Description ?? "No description.",
                                IsRequired = !typeInformation.IsOptional
                            };

                            if (typeInformation.IsStringLike)
                            {
                                option.Type = SlashCommandOptionType.String;
                            }
                            else
                            {
                                var actualType = typeInformation.ActualType;
                                if (actualType.IsEnum)
                                {
                                    actualType = actualType.GetCustomAttribute(typeof(FlagsAttribute)) != null
                                        ? typeof(string)
                                        : actualType.GetEnumUnderlyingType();
                                }

                                if (actualType == typeof(bool))
                                {
                                    option.Type = SlashCommandOptionType.Boolean;
                                }
                                else if (actualType == typeof(byte))
                                {
                                    option.Type = SlashCommandOptionType.Integer;
                                    option.MinimumValue = byte.MinValue;
                                    option.MaximumValue = byte.MaxValue;
                                }
                                else if (actualType == typeof(sbyte))
                                {
                                    option.Type = SlashCommandOptionType.Integer;
                                    option.MinimumValue = sbyte.MinValue;
                                    option.MaximumValue = sbyte.MaxValue;
                                }
                                else if (actualType == typeof(short))
                                {
                                    option.Type = SlashCommandOptionType.Integer;
                                    option.MinimumValue = short.MinValue;
                                    option.MaximumValue = short.MaxValue;
                                }
                                else if (actualType == typeof(ushort))
                                {
                                    option.Type = SlashCommandOptionType.Integer;
                                    option.MinimumValue = ushort.MinValue;
                                    option.MaximumValue = ushort.MaxValue;
                                }
                                else if (actualType == typeof(int))
                                {
                                    option.Type = SlashCommandOptionType.Integer;
                                    option.MinimumValue = int.MinValue;
                                    option.MaximumValue = int.MaxValue;
                                }
                                else if (actualType == typeof(uint))
                                {
                                    option.Type = SlashCommandOptionType.Integer;
                                    option.MinimumValue = uint.MinValue;
                                    option.MaximumValue = uint.MaxValue;
                                }
                                else if (actualType == typeof(Half))
                                {
                                    option.Type = SlashCommandOptionType.Number;
                                    option.MinimumValue = (double) Half.MinValue;
                                    option.MaximumValue = (double) Half.MaxValue;
                                }
                                else if (actualType == typeof(float))
                                {
                                    option.Type = SlashCommandOptionType.Number;
                                    option.MinimumValue = float.MinValue;
                                    option.MaximumValue = float.MaxValue;
                                }
                                else if (actualType == typeof(double))
                                {
                                    option.Type = SlashCommandOptionType.Number;
                                }
                                else if (typeof(IUser).IsAssignableFrom(actualType))
                                {
                                    option.Type = SlashCommandOptionType.User;
                                }
                                else if (typeof(IChannel).IsAssignableFrom(actualType))
                                {
                                    option.Type = SlashCommandOptionType.Channel;
                                }
                                else if (typeof(IRole).IsAssignableFrom(actualType))
                                {
                                    option.Type = SlashCommandOptionType.Role;
                                }
                                else if (typeof(ISnowflakeEntity).IsAssignableFrom(actualType))
                                {
                                    option.Type = SlashCommandOptionType.Mentionable;
                                }
                                else if (actualType == typeof(IAttachment))
                                {
                                    option.Type = SlashCommandOptionType.Attachment;
                                }
                                else
                                {
                                    option.Type = SlashCommandOptionType.String;
                                }

                                if (option.Type.Value is SlashCommandOptionType.Channel or SlashCommandOptionType.Mentionable)
                                {
                                    if (option.Type.Value is SlashCommandOptionType.Channel)
                                    {
                                        if (actualType != typeof(IChannel) && !typeof(IInteractionChannel).IsAssignableFrom(actualType))
                                            throw new InvalidOperationException($"Slash command channel parameters must be of type {typeof(IChannel)} or {typeof(IInteractionChannel)}.");
                                    }

                                    var customAttributes = parameter.CustomAttributes;
                                    var customAttributeCount = customAttributes.Count;
                                    for (var i = 0; i < customAttributeCount; i++)
                                    {
                                        var customAttribute = customAttributes[i];
                                        if (customAttribute is not ChannelTypesAttribute requireChannelTypesAttribute)
                                            continue;

                                        option.ChannelTypes = requireChannelTypesAttribute.ChannelTypes;
                                        break;
                                    }
                                }
                            }

                            if (option.Type.Value is SlashCommandOptionType.String or SlashCommandOptionType.Integer or SlashCommandOptionType.Number)
                            {
                                if (typeInformation.ActualType.IsEnum && option.Type.Value != SlashCommandOptionType.String)
                                {
                                    var fields = typeInformation.ActualType.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                                    var names = new string[fields.Length];
                                    var values = new object[fields.Length];
                                    for (var i = 0; i < fields.Length; i++)
                                    {
                                        var field = fields[i];
                                        names[i] = field.GetCustomAttribute<ChoiceNameAttribute>()?.Name ?? field.Name;
                                        values[i] = field.GetRawConstantValue()!;
                                    }

                                    // Source: Type.GetEnumData()
                                    var comparer = Comparer.Default;
                                    for (var i = 1; i < values.Length; i++)
                                    {
                                        var j = i;
                                        var tempName = names[i];
                                        var tempValue = values[i];
                                        var exchanged = false;

                                        while (comparer.Compare(values[j - 1], tempValue) > 0)
                                        {
                                            names[j] = names[j - 1];
                                            values[j] = values[j - 1];
                                            j--;
                                            exchanged = true;
                                            if (j == 0)
                                                break;
                                        }

                                        if (exchanged)
                                        {
                                            names[j] = tempName;
                                            values[j] = tempValue;
                                        }
                                    }

                                    // ---

                                    if (names.Length <= Discord.Limits.ApplicationCommand.Option.MaxChoiceAmount)
                                    {
                                        var choices = new LocalSlashCommandOptionChoice[names.Length];
                                        for (var i = 0; i < names.Length; i++)
                                        {
                                            var name = names[i];
                                            var value = values.GetValue(i)!;
                                            choices[i] = new LocalSlashCommandOptionChoice
                                            {
                                                Name = name,
                                                Value = value
                                            };
                                        }

                                        option.Choices = choices;
                                    }
                                    else
                                    {
                                        option.Type = SlashCommandOptionType.String;
                                    }
                                }
                                else
                                {
                                    var customAttributes = parameter.CustomAttributes;
                                    var customAttributeCount = customAttributes.Count;
                                    for (var i = 0; i < customAttributeCount; i++)
                                    {
                                        var customAttribute = customAttributes[i];
                                        if (customAttribute is not ChoiceAttribute choiceAttribute)
                                            continue;

                                        option.AddChoice(new LocalSlashCommandOptionChoice
                                        {
                                            Name = choiceAttribute.Name,
                                            Value = choiceAttribute.Value
                                        });

                                        if (option.Choices.Value.Count == Discord.Limits.ApplicationCommand.Option.MaxChoiceAmount)
                                            break;
                                    }

                                    if (!option.Choices.HasValue)
                                    {
                                        var checks = parameter.Checks;
                                        var checkCount = checks.Count;
                                        for (var i = 0; i < checkCount; i++)
                                        {
                                            static void ThrowIfTooLarge(IConvertible convertible)
                                            {
                                                if (convertible.GetTypeCode() is TypeCode.Int64 or TypeCode.UInt64 or TypeCode.Decimal)
                                                    Throw.InvalidOperationException($"The value {convertible} cannot be used for slash command parameter range checks as it is too large.");
                                            }

                                            static void SetMinimum(LocalSlashCommandOption option, IConvertible value)
                                            {
                                                ThrowIfTooLarge(value);

                                                if (option.Type.Value == SlashCommandOptionType.String)
                                                {
                                                    option.MinimumLength = value.ToInt32(null);
                                                }
                                                else
                                                {
                                                    option.MinimumValue = value.ToDouble(null);
                                                }
                                            }

                                            static void SetMaximum(LocalSlashCommandOption option, IConvertible value)
                                            {
                                                ThrowIfTooLarge(value);

                                                if (option.Type.Value == SlashCommandOptionType.String)
                                                {
                                                    option.MaximumLength = value.ToInt32(null);
                                                }
                                                else
                                                {
                                                    option.MaximumValue = value.ToDouble(null);
                                                }
                                            }

                                            var check = checks[i];
                                            switch (check)
                                            {
                                                case MinimumAttribute minimumAttribute:
                                                {
                                                    SetMinimum(option, minimumAttribute.Minimum);
                                                    break;
                                                }
                                                case MaximumAttribute maximumAttribute:
                                                {
                                                    SetMaximum(option, maximumAttribute.Maximum);
                                                    break;
                                                }
                                                case RangeAttribute rangeAttribute:
                                                {
                                                    SetMinimum(option, rangeAttribute.Minimum);
                                                    SetMaximum(option, rangeAttribute.Maximum);
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            return option;
                        }

                        var parameter = parameters[i];
                        var option = GetOption(parameter);
                        if (option.Type.Value is SlashCommandOptionType.String or SlashCommandOptionType.Number or SlashCommandOptionType.Integer)
                        {
                            var autoCompleteCommand = command.AutoCompleteCommand;
                            if (autoCompleteCommand != null)
                            {
                                var autoCompleteParameters = autoCompleteCommand.Parameters;
                                var autoCompleteParameterCount = autoCompleteParameters.Count;
                                for (var j = 0; j < autoCompleteParameterCount; j++)
                                {
                                    if (autoCompleteParameters[j].Name == option.Name && typeof(IAutoComplete).IsAssignableFrom(autoCompleteParameters[j].ReflectedType))
                                    {
                                        option.HasAutoComplete = true;
                                        break;
                                    }
                                }
                            }
                        }

                        parameterOptions.Add(option);
                    }
                }
            }

            foreach (var contextMenuCommand in topLevelNode.ContextMenuCommands.Values)
                AddCommand( /*commandCache,*/ localCommands, contextMenuCommand);

            static void GetCommands( /*Dictionary<ApplicationCommand, LocalApplicationCommand> commandCache,*/
                ApplicationCommandMap.Node node, FastList<LocalApplicationCommand> commands)
            {
                foreach (var slashCommand in node.SlashCommands.Values)
                    AddCommand( /*commandCache,*/ commands, slashCommand);

                foreach (var subnode in node.Nodes.Values)
                    GetCommands( /*commandCache,*/ subnode, commands);
            }

            GetCommands( /*commandCache,*/ topLevelNode, localCommands);
        }

        cancellationToken.ThrowIfCancellationRequested();

        GetCommands( /*commandCache,*/ map.GlobalNode, globalCommands);
        foreach (var (guildId, guildNode) in map.GuildNodes)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var list = new FastList<LocalApplicationCommand>();
            guildCommands[guildId] = list;
            GetCommands( /*commandCache,*/ guildNode, list);
        }

        return (globalCommands, guildCommands);
    }

    public class ApplicationCommandSyncException : AggregateException
    {
        public Snowflake? GuildId { get; }

        public IReadOnlyList<IApplicationCommand> SyncedCommands { get; }

        public ApplicationCommandSyncException(Snowflake? guildId, IReadOnlyList<IApplicationCommand> syncedCommands, IEnumerable<Exception> exceptions)
            : base($"Application command sync failed for {(guildId == null ? "global" : $"guild ({guildId})")} commands.", exceptions)
        {
            GuildId = guildId;
            SyncedCommands = syncedCommands;
        }
    }

    /// <summary>
    ///     Syncs the given global and guild commands to the Discord API.
    /// </summary>
    /// <param name="globalCommands"> The global commands to sync. </param>
    /// <param name="guildCommands"> The guild commands to sync. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    protected virtual async ValueTask SyncApplicationCommands(IEnumerable<LocalApplicationCommand> globalCommands,
        IReadOnlyDictionary<Snowflake, IEnumerable<LocalApplicationCommand>> guildCommands, CancellationToken cancellationToken)
    {
        if (!_syncGlobalApplicationCommands && !_syncGuildApplicationCommands)
            return;

        var cacheProvider = Services.GetService<IApplicationCommandCacheProvider>();
        if (cacheProvider == null)
            return;

        static Task<IReadOnlyList<IApplicationCommand>> GetSyncStrategyTask(DiscordBotBase bot, Snowflake applicationId,
            Snowflake? guildId, IEnumerable<LocalApplicationCommand> commands, IApplicationCommandCacheChanges changes,
            DefaultRestRequestOptions options, CancellationToken cancellationToken)
        {
            const int BulkChangeThreshold = 5;

            var unchangedCount = changes.UnchangedCommands.Count;
            var createdCount = changes.CreatedCommands.Count;
            var modifiedCount = changes.ModifiedCommands.Count;
            var deletedCount = changes.DeletedCommandIds.Count;
            if (guildId == null)
                bot.Logger.LogDebug("Global application command states: {0} unchanged, {1} created, {2} modified, {3} deleted.", unchangedCount, createdCount, modifiedCount, deletedCount);
            else
                bot.Logger.LogDebug("Guild ({0}) application command states: {1} unchanged, {2} created, {3} modified, {4} deleted.", guildId, unchangedCount, createdCount, modifiedCount, deletedCount);

            var totalChangeCount = createdCount + modifiedCount + deletedCount;
            if (changes.AreInitial || totalChangeCount > BulkChangeThreshold)
            {
                return guildId == null
                    ? bot.SetGlobalApplicationCommandsAsync(applicationId, commands, options, cancellationToken)
                    : bot.SetGuildApplicationCommandsAsync(applicationId, guildId.Value, commands, options, cancellationToken);
            }

            var index = 0;
            var tasks = new Task[totalChangeCount];
            var createdCommands = changes.CreatedCommands;
            for (var i = 0; i < createdCount; i++)
            {
                var command = createdCommands[i];
                tasks[index++] = guildId == null
                    ? bot.CreateGlobalApplicationCommandAsync(applicationId, command, options, cancellationToken)
                    : bot.CreateGuildApplicationCommandAsync(applicationId, guildId.Value, command, options, cancellationToken);
            }

            var modifiedCommands = changes.ModifiedCommands;
            foreach (var (commandId, command) in modifiedCommands)
            {
                void Action(ModifyApplicationCommandActionProperties properties)
                {
                    properties.Name = command.Name;
                    properties.NameLocalizations = Optional.Convert(command.NameLocalizations, localizations => localizations as IEnumerable<KeyValuePair<CultureInfo, string>>);

                    if (command is LocalSlashCommand slashCommand)
                    {
                        properties.Description = slashCommand.Description;
                        properties.DescriptionLocalizations = Optional.Convert(slashCommand.DescriptionLocalizations, localizations => localizations as IEnumerable<KeyValuePair<CultureInfo, string>>);
                        properties.Options = Optional.Convert(slashCommand.Options, options => options as IEnumerable<LocalSlashCommandOption>);
                    }

                    properties.DefaultRequiredMemberPermissions = command.DefaultRequiredMemberPermissions;
                    properties.IsEnabledInPrivateChannels = command.IsEnabledInPrivateChannels.GetValueOrDefault(true);
                    properties.IsEnabledByDefault = command.IsEnabledByDefault;
                }

                tasks[index++] = guildId == null
                    ? bot.ModifyGlobalApplicationCommandAsync(applicationId, commandId, Action, options, cancellationToken)
                    : bot.ModifyGuildApplicationCommandAsync(applicationId, guildId.Value, commandId, Action, options, cancellationToken);
            }

            var deletedCommandsIds = changes.DeletedCommandIds;
            for (var i = 0; i < deletedCount; i++)
            {
                var commandId = deletedCommandsIds[i];
                tasks[index++] = guildId == null
                    ? bot.DeleteGlobalApplicationCommandAsync(applicationId, commandId, options, cancellationToken)
                    : bot.DeleteGuildApplicationCommandAsync(applicationId, guildId.Value, commandId, options, cancellationToken);
            }

            static async Task<IReadOnlyList<IApplicationCommand>> ExecuteTasks(Snowflake? guildId, int count, Task[] tasks)
            {
                var whenAllTask = Task.WhenAll(tasks);
                var continuation = whenAllTask.ContinueWith(_ => { }, default(CancellationToken));
                await continuation.ConfigureAwait(false);
                var exceptions = new List<Exception>(count);
                var commands = new List<IApplicationCommand>(count);
                foreach (var task in tasks)
                {
                    if (task.Exception != null)
                    {
                        exceptions.Add(task.Exception);
                        continue;
                    }

                    if (task is not Task<IApplicationCommand> resultTask)
                        continue;

                    commands.Add(resultTask.Result);
                }

                if (exceptions.Count > 0)
                    throw new ApplicationCommandSyncException(guildId, commands, exceptions);

                return commands;
            }

            return ExecuteTasks(guildId, totalChangeCount - deletedCount, tasks);
        }

        var cache = await cacheProvider.GetCacheAsync(cancellationToken).ConfigureAwait(false);
        await using (cache.ConfigureAwait(false))
        {
            var requestOptions = new DefaultRestRequestOptions
            {
                MaximumDelayDuration = TimeSpan.Zero
            };

            var applicationId = _applicationId ?? (_currentApplication ??= await this.FetchCurrentApplicationAsync(requestOptions, cancellationToken).ConfigureAwait(false)).Id;
            var commandChanges = new FastList<(Snowflake?, IApplicationCommandCacheChanges)>();
            var tasks = new FastList<Task<IReadOnlyList<IApplicationCommand>>>();
            if (_syncGlobalApplicationCommands)
            {
                var changes = cache.GetChanges(null, globalCommands);
                if (changes.Any)
                {
                    commandChanges.Add((null, changes));
                    tasks.Add(GetSyncStrategyTask(this, applicationId, null, globalCommands, changes, requestOptions, cancellationToken));
                }
                else
                {
                    Logger.LogDebug("Global application commands are up-to-date.");
                }
            }

            if (_syncGuildApplicationCommands)
            {
                foreach (var (guildId, commands) in guildCommands)
                {
                    var changes = cache.GetChanges(guildId, commands);
                    if (changes.Any)
                    {
                        commandChanges.Add((guildId, changes));
                        tasks.Add(GetSyncStrategyTask(this, applicationId, guildId, commands, changes, requestOptions, cancellationToken));
                    }
                    else
                    {
                        Logger.LogDebug("Guild ({0}) application commands are up-to-date.", guildId);
                    }
                }
            }

            var whenAllTask = Task.WhenAll(tasks);
            var continuation = whenAllTask.ContinueWith(_ => { }, default(CancellationToken));
            await continuation.ConfigureAwait(false);

            var count = tasks.Count;
            for (var i = 0; i < count; i++)
            {
                var task = tasks[i];
                if (!task.IsCompletedSuccessfully)
                    continue;

                var (guildId, changes) = commandChanges[i];
                cache.ApplyChanges(guildId, changes, task.Result);
            }

            if (whenAllTask.Exception != null)
                ExceptionDispatchInfo.Capture(whenAllTask.Exception).Throw();
        }
    }

    /// <summary>
    ///     Checks whether application commands should be initialized and synced.
    /// </summary>
    /// <remarks>
    ///     By default, this method ensures the sync is only performed if the bot
    ///     is managing the shard with index <c>0</c>, i.e. the default shard.
    /// </remarks>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A <see cref="ValueTask{TResult}"/> representing the work with the result being a <see cref="bool"/>.
    /// </returns>
    protected virtual async ValueTask<bool> ShouldInitializeApplicationCommands(CancellationToken cancellationToken)
    {
        var shardCoordinator = (this as IGatewayClient).ApiClient.ShardCoordinator;
        var shardSet = await shardCoordinator.GetShardSetAsync(cancellationToken).ConfigureAwait(false);
        return shardSet.ShardIds.Any(shardId => shardId.Index == 0);
    }

    /// <summary>
    ///     Initializes and syncs application commands.
    /// </summary>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    public virtual async ValueTask InitializeApplicationCommands(CancellationToken cancellationToken)
    {
        var stopwatch = new Stopwatch();
        var map = Commands.GetCommandMapProvider().GetRequiredMap<ApplicationCommandMap>();

        stopwatch.Start();

        var (globalCommands, guildCommands) = ConvertApplicationCommandMap(map, cancellationToken);
        Logger.LogDebug("Application command build took {0}ms.", stopwatch.Elapsed.TotalMilliseconds);

        stopwatch.Restart();

        await LocalizeApplicationCommands(globalCommands, guildCommands, cancellationToken).ConfigureAwait(false);
        Logger.LogDebug("Application command localization took {0}ms.", stopwatch.Elapsed.TotalMilliseconds);

        stopwatch.Restart();

        await SyncApplicationCommands(globalCommands, guildCommands, cancellationToken).ConfigureAwait(false);
        Logger.LogDebug("Application command synchronization took {0}ms.", stopwatch.Elapsed.TotalMilliseconds);
    }
}
