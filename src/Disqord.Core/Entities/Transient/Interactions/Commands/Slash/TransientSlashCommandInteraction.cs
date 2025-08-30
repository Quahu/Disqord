using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Disqord.Models;
using Qommon.Collections.ReadOnly;

namespace Disqord;

public class TransientSlashCommandInteraction(IClient client, long receivedAt, InteractionJsonModel model)
    : TransientApplicationCommandInteraction(client, receivedAt, model), ISlashCommandInteraction
{
    /// <inheritdoc/>
    [field: MaybeNull]
    public IReadOnlyDictionary<string, ISlashCommandInteractionOption> Options
    {
        get
        {
            if (!Data.Options.HasValue)
                return ReadOnlyDictionary<string, ISlashCommandInteractionOption>.Empty;

            return field ??= Data.Options.Value.ToReadOnlyDictionary(Client,
                (model, _) => model.Name,
                (model, client) => new TransientSlashCommandInteractionOption(client, model) as ISlashCommandInteractionOption, StringComparer.OrdinalIgnoreCase);
        }
    }
}
