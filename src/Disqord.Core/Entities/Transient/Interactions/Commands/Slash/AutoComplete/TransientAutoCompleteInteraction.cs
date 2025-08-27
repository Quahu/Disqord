using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Disqord.Models;
using Qommon;
using Qommon.Collections.ReadOnly;

namespace Disqord;

public class TransientAutoCompleteInteraction(IClient client, long receivedAt, InteractionJsonModel model)
    : TransientApplicationCommandInteraction(client, receivedAt, model), IAutoCompleteInteraction
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
                static (model, _) => model.Name,
                static (model, client) => new TransientSlashCommandInteractionOption(client, model) as ISlashCommandInteractionOption, StringComparer.OrdinalIgnoreCase);
        }
    }

    public ISlashCommandInteractionOption FocusedOption
    {
        get
        {
            var names = new List<string>();
            FindOptionName(names, Data.Options.Value);
            var nameCount = names.Count;
            ISlashCommandInteractionOption? option = null;
            for (var i = 0; i < nameCount; i++)
            {
                var name = names[i];
                option = (option?.Options ?? Options)[name];
            }

            return option!;

            static void FindOptionName(List<string> names, ApplicationCommandInteractionDataOptionJsonModel[] options)
            {
                foreach (var option in options)
                {
                    if (option.Options.TryGetValue(out var suboptions))
                    {
                        names.Add(option.Name);
                        FindOptionName(names, suboptions);
                        return;
                    }

                    if (option.Focused.GetValueOrDefault())
                    {
                        names.Add(option.Name);
                        return;
                    }
                }
            }
        }
    }
}
