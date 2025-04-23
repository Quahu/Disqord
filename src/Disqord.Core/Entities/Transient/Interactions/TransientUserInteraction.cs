using System.Collections.Generic;
using System.Globalization;
using Disqord.Models;
using Qommon;
using Qommon.Collections.ReadOnly;

namespace Disqord;

public class TransientUserInteraction : TransientInteraction, IUserInteraction
{
    /// <inheritdoc/>
    public Snowflake? GuildId => Model.GuildId.GetValueOrNullable();

    /// <inheritdoc/>
    public Snowflake ChannelId
    {
        get
        {
            if (Model.Channel.HasValue)
            {
                return Model.Channel.Value.Id;
            }

            return Model.ChannelId.Value;
        }
    }

    /// <inheritdoc/>
    public IInteractionChannel? Channel
    {
        get
        {
            if (!Model.Channel.HasValue)
                return null;

            return _channel ??= new TransientInteractionChannel(Client, Model.Channel.Value);
        }
    }
    private IInteractionChannel? _channel;

    /// <inheritdoc/>
    public IUser Author
    {
        get
        {
            return _author ??= Model.Member.HasValue
                ? new TransientMember(Client, GuildId!.Value, Model.Member.Value)
                : new TransientUser(Client, Model.User.Value);
        }
    }
    private IUser? _author;

    /// <inheritdoc/>
    public Permissions AuthorPermissions
    {
        get
        {
            if (!Model.Member.HasValue || !Model.Member.Value.Permissions.HasValue)
                return Permissions.None;

            return Model.Member.Value.Permissions.Value;
        }
    }

    /// <inheritdoc/>
    public Permissions ApplicationPermissions => Model.AppPermissions.GetValueOrDefault();

    /// <inheritdoc/>
    public CultureInfo Locale => Discord.Internal.GetLocale(Model.Locale.Value);

    /// <inheritdoc/>
    public CultureInfo? GuildLocale => Optional.ConvertOrDefault(Model.GuildLocale, Discord.Internal.GetLocale);

    /// <inheritdoc/>
    public IReadOnlyList<IEntitlement> Entitlements => _entitlements ??= Model.Entitlements.ToReadOnlyList(Client, (model, client) => new TransientEntitlement(client, model));

    private IReadOnlyList<IEntitlement>? _entitlements;

    /// <inheritdoc/>
    public int? AttachmentSizeLimit => Model.AttachmentSizeLimit.GetValueOrNullable();

    public TransientUserInteraction(IClient client, long __receivedAt, InteractionJsonModel model)
        : base(client, __receivedAt, model)
    { }

    public new static IUserInteraction Create(IClient client, long __receivedAt, InteractionJsonModel model)
    {
        return model.Type switch
        {
            InteractionType.ApplicationCommand => model.Data.Value.Type.Value switch
            {
                ApplicationCommandType.Slash => new TransientSlashCommandInteraction(client, __receivedAt, model),
                ApplicationCommandType.User or ApplicationCommandType.Message => new TransientContextMenuInteraction(client, __receivedAt, model),
                _ => new TransientApplicationCommandInteraction(client, __receivedAt, model)
            },
            InteractionType.MessageComponent => TransientComponentInteraction.Create(client, __receivedAt, model),
            InteractionType.ApplicationCommandAutoComplete => new TransientAutoCompleteInteraction(client, __receivedAt, model),
            InteractionType.ModalSubmit => new TransientModalSubmitInteraction(client, __receivedAt, model),
            _ => new TransientUserInteraction(client, __receivedAt, model)
        };
    }
}
