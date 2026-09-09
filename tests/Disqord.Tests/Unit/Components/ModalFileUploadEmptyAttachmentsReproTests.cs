using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Bot;
using Disqord.Bot.Commands;
using Disqord.Bot.Commands.Components;
using Disqord.Bot.Commands.Interaction;
using Disqord.Models;
using Qmmands;
using Qmmands.Default;
using Qommon;

namespace Disqord.Tests.Unit.Components;

public sealed class ModalFileUploadEmptyAttachmentsReproTests
{
    [Test]
    public void TransientModalFileUploadComponent_ZeroFilesSubmitted_AttachmentIdsIsEmpty()
    {
        // Arrange
        var model = new ModalFileUploadComponentJsonModel
        {
            CustomId = "file_upload",
            Values = Array.Empty<string>(),
        };
        var component = new TransientModalFileUploadComponent(model);

        // Act
        var attachmentIds = component.AttachmentIds;

        // Assert
        Assert.That(attachmentIds, Is.Empty);
    }

    [Test]
    public void BindArgumentFromModalComponent_OptionalAttachmentWithZeroFiles_DoesNotThrowAndLeavesArgumentUnset()
    {
        // Arrange
        var (bindValues, parameter, context) = CreateFixture(isOptional: true);
        var fileUploadComponent = CreateFileUploadComponent();

        // Act
        bindValues.Invoke(context, parameter, fileUploadComponent);

        // Assert
        Assert.That(context.Arguments!.ContainsKey(parameter), Is.False);
    }

    [Test]
    public async Task BindArgumentFromModalComponent_OptionalAttachmentWithExplicitDefaultAndZeroFiles_QmmandsArgumentBinderSucceeds()
    {
        // Arrange
        var (bindValues, parameter, context) = CreateFixture(isOptional: true);
        var fileUploadComponent = CreateFileUploadComponent();

        // Act
        bindValues.Invoke(context, parameter, fileUploadComponent);
        var bindResult = await new DefaultArgumentBinder().BindAsync(context);

        // Assert
        Assert.That(bindResult.IsSuccessful, Is.True);
    }

    [Test]
    public void BindArgumentFromModalComponent_RequiredAttachmentWithZeroFiles_ThrowsInvalidOperationException()
    {
        // Arrange
        var (bindValues, parameter, context) = CreateFixture(isOptional: false);
        var fileUploadComponent = CreateFileUploadComponent();

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => bindValues.Invoke(context, parameter, fileUploadComponent));
        Assert.That(exception!.Message, Does.Contain(parameter.Name));
    }

    [Test]
    public void BindArgumentFromModalComponent_SingleAttachmentParameterWithMultipleFiles_ThrowsInvalidOperationException()
    {
        // Arrange
        var (bindValues, parameter, context) = CreateFixture(isOptional: false);
        var firstAttachmentId = Snowflake.Parse("111111111111111111");
        var secondAttachmentId = Snowflake.Parse("222222222222222222");
        context.Attachments[firstAttachmentId] = new FakeAttachment();
        context.Attachments[secondAttachmentId] = new FakeAttachment();
        var fileUploadComponent = CreateFileUploadComponent(firstAttachmentId, secondAttachmentId);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => bindValues.Invoke(context, parameter, fileUploadComponent));
        Assert.That(exception!.Message, Does.Contain(parameter.Name));
    }

    [Test]
    public void BindArgumentFromModalComponent_SingleAttachmentParameterWithOneFile_BindsTheAttachment()
    {
        // Arrange
        var (bindValues, parameter, context) = CreateFixture(isOptional: false);
        var attachmentId = Snowflake.Parse("111111111111111111");
        var attachment = new FakeAttachment();
        context.Attachments[attachmentId] = attachment;
        var fileUploadComponent = CreateFileUploadComponent(attachmentId);

        // Act
        bindValues.Invoke(context, parameter, fileUploadComponent);

        // Assert
        Assert.That(context.Arguments![parameter], Is.SameAs(attachment));
    }

    [Test]
    public void BindArgumentFromModalComponent_RequiredCollectionAttachmentParameterWithZeroFiles_BindsEmptyArrayInsteadOfThrowing()
    {
        // Arrange
        var (bindValues, parameter, context) = CreateCollectionFixture();
        var fileUploadComponent = CreateFileUploadComponent();

        // Act
        bindValues.Invoke(context, parameter, fileUploadComponent);

        // Assert
        Assert.That(context.Arguments!.ContainsKey(parameter), Is.True);
        Assert.That((IAttachment[]) context.Arguments[parameter]!, Is.Empty);
    }

    private static (ExposedBindValues BindValues, IParameter Parameter, FakeComponentCommandContext Context) CreateCollectionFixture()
    {
        var moduleBuilder = new ComponentModuleBuilder();
        var module = moduleBuilder.Build();
        var commandBuilder = new ComponentCommandBuilder(moduleBuilder, new FakeCallback())
        {
            Pattern = "upload",
            Type = ComponentCommandType.Modal,
        };
        var parameterBuilder = new ComponentParameterBuilder(commandBuilder, typeof(IAttachment[]))
        {
            Name = "files",
        };

        commandBuilder.Parameters.Add(parameterBuilder);
        var command = commandBuilder.Build(module);
        var parameter = command.Parameters[0];

        var context = new FakeComponentCommandContext
        {
            Command = command,
            RawArguments = new Dictionary<IParameter, MultiString>(),
            Arguments = new Dictionary<IParameter, object?>(),
        };
        context.Interaction = new FakeEntityInteraction(context.Attachments);

        return (new ExposedBindValues(), parameter, context);
    }

    private static (ExposedBindValues BindValues, IParameter Parameter, FakeComponentCommandContext Context) CreateFixture(bool isOptional)
    {
        var moduleBuilder = new ComponentModuleBuilder();
        var module = moduleBuilder.Build();
        var commandBuilder = new ComponentCommandBuilder(moduleBuilder, new FakeCallback())
        {
            Pattern = "upload",
            Type = ComponentCommandType.Modal,
        };
        var parameterBuilder = new ComponentParameterBuilder(commandBuilder, typeof(IAttachment))
        {
            Name = "file",
        };
        if (isOptional)
        {
            parameterBuilder.DefaultValue = null;
        }

        commandBuilder.Parameters.Add(parameterBuilder);
        var command = commandBuilder.Build(module);
        var parameter = command.Parameters[0];

        var context = new FakeComponentCommandContext
        {
            Command = command,
            RawArguments = new Dictionary<IParameter, MultiString>(),
            Arguments = new Dictionary<IParameter, object?>(),
        };
        context.Interaction = new FakeEntityInteraction(context.Attachments);

        return (new ExposedBindValues(), parameter, context);
    }

    private static TransientModalFileUploadComponent CreateFileUploadComponent(params Snowflake[] attachmentIds)
    {
        var model = new ModalFileUploadComponentJsonModel
        {
            CustomId = "file",
            Values = Array.ConvertAll(attachmentIds, static id => id.ToString()),
        };

        return new TransientModalFileUploadComponent(model);
    }

    private sealed class ExposedBindValues : DefaultComponentExecutionSteps.BindValues
    {
        public void Invoke(IDiscordComponentCommandContext context, IParameter parameter, IModalComponent modalComponent)
        {
            var method = typeof(DefaultComponentExecutionSteps.BindValues).GetMethod(
                "BindArgumentFromModalComponent",
                BindingFlags.NonPublic | BindingFlags.Instance);

            try
            {
                method!.Invoke(this, new object[] { context, parameter, modalComponent });
            }
            catch (TargetInvocationException exception) when (exception.InnerException != null)
            {
                throw exception.InnerException;
            }
        }
    }

    private sealed class FakeCallback : ICommandCallback
    {
        public ValueTask<IModuleBase?> CreateModuleBase(ICommandContext context)
        {
            throw new NotSupportedException();
        }

        public ValueTask<IResult?> ExecuteAsync(ICommandContext context)
        {
            throw new NotSupportedException();
        }
    }

    private sealed class FakeComponentCommandContext : IDiscordComponentCommandContext
    {
        public Dictionary<Snowflake, IAttachment> Attachments { get; } = new();

        public IServiceProvider Services => throw new NotSupportedException();

        public CancellationToken CancellationToken => CancellationToken.None;

        public CultureInfo Locale => CultureInfo.InvariantCulture;

        public ICommandExecutionStep? ExecutionStep { get; set; }

        public ICommand? Command { get; set; }

        public IDictionary<IParameter, MultiString>? RawArguments { get; set; }

        public IDictionary<IParameter, object?>? Arguments { get; set; }

        public IModuleBase? ModuleBase { get; set; }

        public DiscordBotBase Bot => throw new NotSupportedException();

        public CultureInfo? GuildLocale => null;

        public Snowflake? GuildId => null;

        public Snowflake ChannelId => default;

        public IUser Author => throw new NotSupportedException();

        public IUserInteraction Interaction { get; set; } = null!;

        public ValueTask ResetAsync()
        {
            return default;
        }
    }

    private sealed class FakeEntityInteraction : IEntityInteraction
    {
        public IInteractionEntities Entities { get; }

        public FakeEntityInteraction(IReadOnlyDictionary<Snowflake, IAttachment> attachments)
        {
            Entities = new FakeInteractionEntities(attachments);
        }

        public long __ReceivedAt => throw new NotSupportedException();

        public Snowflake ApplicationId => throw new NotSupportedException();

        public int Version => throw new NotSupportedException();

        public InteractionType Type => throw new NotSupportedException();

        public string Token => throw new NotSupportedException();

        public Snowflake Id => throw new NotSupportedException();

        public IClient Client => throw new NotSupportedException();

        public Snowflake ChannelId => throw new NotSupportedException();

        public Snowflake? GuildId => null;

        public IInteractionChannel? Channel => throw new NotSupportedException();

        public IUser Author => throw new NotSupportedException();

        public Permissions AuthorPermissions => throw new NotSupportedException();

        public Permissions ApplicationPermissions => throw new NotSupportedException();

        public CultureInfo Locale => throw new NotSupportedException();

        public CultureInfo? GuildLocale => null;

        public IReadOnlyList<IEntitlement> Entitlements => throw new NotSupportedException();

        public IReadOnlyDictionary<ApplicationIntegrationType, Snowflake> AuthorizingIntegrationOwnerIds => throw new NotSupportedException();

        public InteractionContextType? ContextType => throw new NotSupportedException();

        public int? AttachmentSizeLimit => throw new NotSupportedException();

        public void Update(InteractionJsonModel model)
        {
            throw new NotSupportedException();
        }
    }

    private sealed class FakeInteractionEntities : IInteractionEntities
    {
        public IReadOnlyDictionary<Snowflake, IAttachment> Attachments { get; }

        public FakeInteractionEntities(IReadOnlyDictionary<Snowflake, IAttachment> attachments)
        {
            Attachments = attachments;
        }

        public IReadOnlyDictionary<Snowflake, IUser> Users => throw new NotSupportedException();

        public IReadOnlyDictionary<Snowflake, IRole> Roles => throw new NotSupportedException();

        public IReadOnlyDictionary<Snowflake, IInteractionChannel> Channels => throw new NotSupportedException();

        public IReadOnlyDictionary<Snowflake, IMessage> Messages => throw new NotSupportedException();
    }

    private sealed class FakeAttachment : IAttachment
    {
        public Snowflake Id => default;

        public IClient Client => throw new NotSupportedException();

        public string FileName => throw new NotSupportedException();

        public string? Description => throw new NotSupportedException();

        public string? ContentType => throw new NotSupportedException();

        public int FileSize => throw new NotSupportedException();

        public string Url => throw new NotSupportedException();

        public string ProxyUrl => throw new NotSupportedException();

        public int? Width => throw new NotSupportedException();

        public int? Height => throw new NotSupportedException();

        public bool IsEphemeral => throw new NotSupportedException();

        public AttachmentFlags Flags => throw new NotSupportedException();

        public TimeSpan? Duration => throw new NotSupportedException();

        public string? WaveformBase64 => throw new NotSupportedException();
    }
}
