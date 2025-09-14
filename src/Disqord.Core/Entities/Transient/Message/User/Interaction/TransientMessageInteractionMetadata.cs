using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Disqord.Models;
using Qommon;

namespace Disqord;

public class TransientMessageInteractionMetadata(IClient client, MessageInteractionMetadataJsonModel model)
    : TransientClientEntity<MessageInteractionMetadataJsonModel>(client, model), IMessageInteractionMetadata
{
    /// <inheritdoc/>
    public Snowflake Id => Model.Id;

    /// <inheritdoc/>
    public InteractionType Type => Model.Type;

    /// <inheritdoc/>
    [field: MaybeNull]
    public IUser Author => field ??= new TransientUser(Client, Model.User);

    /// <inheritdoc/>
    public IReadOnlyDictionary<ApplicationIntegrationType, Snowflake> AuthorizingIntegrationOwnerIds => Model.AuthorizingIntegrationOwners;

    /// <inheritdoc/>
    public Snowflake? OriginalResponseMessageId => Model.OriginalResponseMessageId.GetValueOrNullable();

    public static IMessageInteractionMetadata Create(IClient client, MessageInteractionMetadataJsonModel model)
    {
        return model.Type switch
        {
            InteractionType.ApplicationCommand => new TransientMessageApplicationCommandInteractionMetadata(client, Guard.IsAssignableToType<MessageApplicationCommandInteractionMetadataJsonModel>(model)),
            InteractionType.MessageComponent => new TransientMessageComponentInteractionMetadata(client, Guard.IsAssignableToType<MessageComponentInteractionMetadataJsonModel>(model)),
            InteractionType.ModalSubmit => new TransientMessageModalSubmitInteractionMetadata(client, Guard.IsAssignableToType<MessageModalSubmitInteractionMetadataJsonModel>(model)),
            _ => new TransientMessageInteractionMetadata(client, model)
        };
    }
}
