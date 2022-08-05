using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Bot.Commands;
using Disqord.Bot.Commands.Application;
using Disqord.Bot.Commands.Parsers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Qmmands;
using Qmmands.Default;
using Qmmands.Text;

namespace Disqord.Bot;

public abstract partial class DiscordBotBase
{
    protected virtual IEnumerable<Assembly> GetModuleAssemblies()
    {
        return new[] { Assembly.GetEntryAssembly()! };
    }

    protected virtual bool CheckModuleType(TypeInfo typeInfo)
    {
        return true;
    }

    protected virtual void MutateTopLevelModule(IModuleBuilder module)
    {
        try
        {
            MutateModule(module);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An exception occurred while mutating the module '{0}'.", module.TypeInfo?.Name ?? module.Name ?? module.GetType().Name);
        }
    }

    protected virtual void InvokeMutateModule(IModuleBuilder module)
    {
        var typeInfo = module.TypeInfo;
        if (typeInfo == null)
            return;

        var methods = typeInfo.GetMethods(BindingFlags.Static | BindingFlags.Public);
        foreach (var method in methods)
        {
            var attributes = method.GetCustomAttributes();
            var isCallback = false;
            foreach (var attribute in attributes)
            {
                if (attribute is MutateModuleAttribute)
                {
                    isCallback = true;
                    break;
                }
            }

            if (isCallback)
            {
                method.Invoke(null, new object[] { this, module });
                return;
            }
        }
    }

    protected virtual void OnMutateModule(IModuleBuilder module)
    {
        InvokeMutateModule(module);

        if (module is ApplicationModuleBuilder applicationModule && applicationModule.Alias != null)
        {
            applicationModule.Alias = GetSlashCommandName(applicationModule.Alias);
        }
    }

    protected virtual void MutateModule(IModuleBuilder module)
    {
        OnMutateModule(module);

        var submodules = module.Submodules;
        var submoduleCount = submodules.Count;
        for (var i = 0; i < submoduleCount; i++)
        {
            var submodule = submodules[i];
            MutateModule(submodule);
        }

        var commands = module.Commands;
        var commandCount = commands.Count;
        for (var i = 0; i < commandCount; i++)
        {
            var command = commands[i];
            MutateCommand(command);
        }
    }

    protected virtual void OnMutateCommand(ICommandBuilder command)
    {
        if (command is ApplicationCommandBuilder applicationCommand && applicationCommand.Type is ApplicationCommandType.Slash && applicationCommand.Alias != null)
        {
            applicationCommand.Alias = GetSlashCommandName(applicationCommand.Alias);
        }
    }

    protected virtual void MutateCommand(ICommandBuilder command)
    {
        OnMutateCommand(command);

        var parameters = command.Parameters;
        var parameterCount = parameters.Count;
        for (var i = 0; i < parameterCount; i++)
        {
            var parameter = parameters[i];
            MutateParameter(parameter);
        }
    }

    protected virtual void OnMutateParameter(IParameterBuilder parameter)
    {
        if (parameter is ApplicationParameterBuilder applicationParameter && applicationParameter.Name != null)
        {
            applicationParameter.Name = GetSlashCommandName(applicationParameter.Name);
        }
    }

    protected virtual void MutateParameter(IParameterBuilder parameter)
    {
        OnMutateParameter(parameter);
    }

    protected virtual ValueTask AddTypeParsers(DefaultTypeParserProvider typeParserProvider, CancellationToken cancellationToken)
    {
        typeParserProvider.AddParser(new SnowflakeTypeParser());
        typeParserProvider.AddParser(new ColorTypeParser());
        typeParserProvider.AddParser(new CustomEmojiTypeParser());
        typeParserProvider.AddParser(new GuildEmojiTypeParser());
        typeParserProvider.AddParser(new GuildChannelTypeParser<IGuildChannel>());
        typeParserProvider.AddParser(new GuildChannelTypeParser<ICategorizableGuildChannel>());
        typeParserProvider.AddParser(new GuildChannelTypeParser<IMessageGuildChannel>());
        typeParserProvider.AddParser(new GuildChannelTypeParser<IVocalGuildChannel>());
        typeParserProvider.AddParser(new GuildChannelTypeParser<ITextChannel>());
        typeParserProvider.AddParser(new GuildChannelTypeParser<IVoiceChannel>());
        typeParserProvider.AddParser(new GuildChannelTypeParser<ICategoryChannel>());
        typeParserProvider.AddParser(new GuildChannelTypeParser<IStageChannel>());
        typeParserProvider.AddParser(new GuildChannelTypeParser<IThreadChannel>());
        typeParserProvider.AddParser(new GuildChannelTypeParser<IForumChannel>());
        typeParserProvider.AddParser(new MemberTypeParser());
        typeParserProvider.AddParser(new RoleTypeParser());
        return default;
    }

    protected virtual ValueTask InitializeTypeParsers(CancellationToken cancellationToken)
    {
        var typeParserProvider = Services.GetService<ITypeParserProvider>() as DefaultTypeParserProvider;
        if (typeParserProvider == null)
            return default;

        return AddTypeParsers(typeParserProvider, cancellationToken);
    }

    protected virtual ValueTask InitializeModules(CancellationToken cancellationToken)
    {
        try
        {
            var modules = new List<IModule>();
            foreach (var assembly in GetModuleAssemblies())
            {
                modules.AddRange(Commands.AddModules(assembly, MutateTopLevelModule, CheckModuleType));
            }

            var textModules = modules.OfType<ITextModule>().ToArray();
            if (textModules.Length != 0)
                Logger.LogInformation("Mapped {0} text command top-level modules with {1} total commands.", textModules.Length, textModules.SelectMany(CommandUtilities.EnumerateAllCommands).Count());

            var applicationModules = modules.OfType<ApplicationModule>().ToArray();
            if (applicationModules.Length != 0)
                Logger.LogInformation("Mapped {0} application command top-level modules with {1} total commands.", applicationModules.Length, applicationModules.SelectMany(CommandUtilities.EnumerateAllCommands).Count());
        }
        catch (TextCommandMappingException ex)
        {
            Logger.LogCritical(ex, "Failed to map text command {0} in module {1}.", ex.Command, ex.Command.Module);
            throw;
        }
        catch (ApplicationCommandMappingException ex)
        {
            if (ex.Module != null)
                Logger.LogCritical(ex, "Failed to map application command module {0}.", ex.Module);
            else
                Logger.LogCritical(ex, "Failed to map application command {0} in module {1}.", ex.Command, ex.Command!.Module);

            throw;
        }

        return default;
    }

    protected virtual object GetRateLimitBucketKey(IDiscordCommandContext context, RateLimitBucketType bucketType)
    {
        return bucketType switch
        {
            RateLimitBucketType.User => context.Author.Id,
            RateLimitBucketType.Member => (context.GuildId, context.Author.Id),
            RateLimitBucketType.Guild => context.GuildId ?? context.Author.Id,
            RateLimitBucketType.Channel => context.ChannelId,
            _ => throw new ArgumentOutOfRangeException(nameof(bucketType))
        };
    }

    protected virtual ValueTask InitializeRateLimiter(CancellationToken cancellationToken)
    {
        if (Services.GetService<ICommandRateLimiter>() is DefaultCommandRateLimiter rateLimiter)
        {
            rateLimiter.BucketKeyGenerator = (_, __) =>
            {
                if (_ is not IDiscordCommandContext context)
                    throw new ArgumentException($"Context must be a {typeof(IDiscordCommandContext)}.", nameof(context));

                if (__ is not RateLimitBucketType bucketType)
                    throw new ArgumentException($"Bucket type must be a {typeof(RateLimitBucketType)}.", nameof(bucketType));

                return context.Bot.GetRateLimitBucketKey(context, bucketType);
            };
        }

        return default;
    }

    protected virtual ValueTask OnInitialize(CancellationToken cancellationToken)
    {
        return default;
    }

    public async ValueTask InitializeAsync(CancellationToken cancellationToken)
    {
        _masterService?.Bind(this);

        DefaultBotCommandsSetup.Initialize(Commands);

        await OnInitialize(cancellationToken).ConfigureAwait(false);

        await InitializeTypeParsers(cancellationToken).ConfigureAwait(false);
        await InitializeModules(cancellationToken).ConfigureAwait(false);
        await InitializeRateLimiter(cancellationToken).ConfigureAwait(false);

        if (await ShouldInitializeApplicationCommands(cancellationToken).ConfigureAwait(false))
        {
            await InitializeApplicationCommands(cancellationToken).ConfigureAwait(false);
        }
        else
        {
            Logger.LogDebug("Skipping application command sync.");
        }
    }
}
