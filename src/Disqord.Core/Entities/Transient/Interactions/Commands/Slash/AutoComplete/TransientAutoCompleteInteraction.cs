using System;
using System.Collections.Generic;
using Disqord.Models;
using Qommon;
using Qommon.Collections;
using Qommon.Collections.ReadOnly;

namespace Disqord;

public class TransientAutoCompleteInteraction : TransientApplicationCommandInteraction, IAutoCompleteInteraction
{
    /// <inheritdoc/>
    public IReadOnlyDictionary<string, ISlashCommandInteractionOption> Options
    {
        get
        {
            if (!Model.Data.Value.Options.HasValue)
                return ReadOnlyDictionary<string, ISlashCommandInteractionOption>.Empty;

            return _options ??= Model.Data.Value.Options.Value.ToReadOnlyDictionary(Client,
                (model, _) => model.Name,
                (model, client) => new TransientSlashCommandInteractionOption(client, model) as ISlashCommandInteractionOption, StringComparer.OrdinalIgnoreCase);
        }
    }
    private IReadOnlyDictionary<string, ISlashCommandInteractionOption>? _options;

    public ISlashCommandInteractionOption FocusedOption
    {
        get
        {
            static void FindOptionName(FastList<string> names, ApplicationCommandInteractionDataOptionJsonModel[] options)
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

            var names = new FastList<string>();
            FindOptionName(names, Model.Data.Value.Options.Value);
            var nameCount = names.Count;
            ISlashCommandInteractionOption? option = null;
            for (var i = 0; i < nameCount; i++)
            {
                var name = names[i];
                option = (option?.Options ?? Options)[name];
            }

            return option!;
        }
    }

    public TransientAutoCompleteInteraction(IClient client, long __receivedAt, InteractionJsonModel model)
        : base(client, __receivedAt, model)
    { }
}