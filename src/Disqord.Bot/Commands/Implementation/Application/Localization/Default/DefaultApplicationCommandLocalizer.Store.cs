using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using Qommon;

namespace Disqord.Bot.Commands.Application.Default;

public partial class DefaultApplicationCommandLocalizer
{
    protected class StoreInformation
    {
        public DefaultApplicationCommandLocalizer Localizer { get; }

        public CultureInfo Locale { get; }

        public bool IsDefaultLocale => Locale.Equals(Localizer.DefaultCulture);

        public string LocaleFilePath { get; }

        public bool LocaleFileExists { get; }

        public MemoryStream MemoryStream { get; }

        public LocalizationStoreJsonModel Model { get; }

        public StoreInformation(DefaultApplicationCommandLocalizer localizer, CultureInfo locale, string localeFilePath, bool localeFileExists, MemoryStream memoryStream, LocalizationStoreJsonModel model)
        {
            Localizer = localizer;
            Locale = locale;
            LocaleFilePath = localeFilePath;
            LocaleFileExists = localeFileExists;
            MemoryStream = memoryStream;
            Model = model;
        }

        public virtual void Migrate()
        {
            var model = Model;
            if (model.SchemaVersion == 0)
            {
                model.SchemaVersion = SchemaVersion;
                return;
            }

            if (model.SchemaVersion >= SchemaVersion)
                return;

            do
            {
                switch (model.SchemaVersion)
                {
                    case 0:
                    {
                        // Uninitialized model - set current schema version.
                        model.SchemaVersion = SchemaVersion;
                        return;
                    }
                    case SchemaVersion:
                    {
                        // Current schema version.
                        break;
                    }
                    default:
                    {
                        throw new InvalidOperationException($"Unsupported localization schema version '{model.SchemaVersion}'. Current schema version is {SchemaVersion}");
                    }
                }

                model.SchemaVersion++;
            }
            while (model.SchemaVersion < SchemaVersion);
        }

        protected virtual void ReadNode(LocalizationNodeJsonModel localizationNode, IEnumerable<LocalApplicationCommand> commands)
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static string GetLocaleDefault(bool isDefaultLocale, string? value)
            {
                if (!isDefaultLocale || string.IsNullOrWhiteSpace(value))
                    return "";

                return value;
            }

            var isDefaultLocale = IsDefaultLocale;
            foreach (var command in commands)
            {
                OptionalGuard.HasValue(command.Name);
                var commandName = command.Name.Value;
                Guard.IsNotNullOrWhiteSpace(commandName);

                var isInitial = false;
                CommandLocalizationJsonModel? commandLocalization;
                if (command is LocalContextMenuCommand)
                {
                    if (!localizationNode.ContextMenuCommands.TryGetValue(out var contextMenuLocalizations) || contextMenuLocalizations == null)
                    {
                        localizationNode.ContextMenuCommands = contextMenuLocalizations = new Dictionary<string, CommandLocalizationJsonModel?>();
                        isInitial = true;
                    }

                    if (isInitial || !contextMenuLocalizations.TryGetValue(command.Name.Value, out commandLocalization) || commandLocalization == null)
                    {
                        contextMenuLocalizations[command.Name.Value] = commandLocalization = new CommandLocalizationJsonModel();
                        isInitial = true;
                    }
                }
                else
                {
                    if (!localizationNode.SlashCommands.TryGetValue(out var slashCommandLocalizations) || slashCommandLocalizations == null)
                    {
                        localizationNode.SlashCommands = slashCommandLocalizations = new Dictionary<string, CommandLocalizationJsonModel?>();
                        isInitial = true;
                    }

                    if (isInitial || !slashCommandLocalizations.TryGetValue(command.Name.Value, out commandLocalization) || commandLocalization == null)
                    {
                        slashCommandLocalizations[command.Name.Value] = commandLocalization = new CommandLocalizationJsonModel();
                        isInitial = true;
                    }
                }

                if (isInitial)
                {
                    commandLocalization.Name = GetLocaleDefault(isDefaultLocale, commandName);
                }
                else if (!string.IsNullOrWhiteSpace(commandLocalization.Name) && commandName != commandLocalization.Name)
                {
                    command.AddNameLocalization(Locale, commandLocalization.Name);
                }

                if (command is not LocalSlashCommand slashCommand)
                    continue;

                OptionalGuard.HasValue(slashCommand.Description);
                var commandDescription = slashCommand.Description.Value;
                Guard.IsNotNullOrWhiteSpace(commandDescription);

                if (isInitial)
                {
                    commandLocalization.Description = GetLocaleDefault(isDefaultLocale, commandDescription);
                }
                else if (!string.IsNullOrWhiteSpace(commandLocalization.Description.GetValueOrDefault()) && commandDescription != commandLocalization.Description.GetValueOrDefault())
                {
                    slashCommand.AddDescriptionLocalization(Locale, commandLocalization.Description.Value!);
                }

                if (slashCommand.Options.TryGetValue(out var options))
                {
                    if (options == null)
                        return;

                    if (isInitial || !commandLocalization.Options.TryGetValue(out var optionLocalizations) || optionLocalizations == null)
                    {
                        commandLocalization.Options = optionLocalizations = new Dictionary<string, OptionLocalizationJsonModel?>();
                    }

                    static void ReadOptions(CultureInfo locale, bool isDefaultLocale, bool isInitial,
                        Dictionary<string, OptionLocalizationJsonModel?> optionLocalizations, IEnumerable<LocalSlashCommandOption> options)
                    {
                        foreach (var option in options)
                        {
                            OptionalGuard.HasValue(option.Name);
                            var optionName = option.Name.Value;
                            Guard.IsNotNullOrWhiteSpace(optionName);

                            OptionalGuard.HasValue(option.Description);
                            var optionDescription = option.Description.Value;
                            Guard.IsNotNullOrWhiteSpace(optionDescription);

                            if (isInitial || !optionLocalizations.TryGetValue(option.Name.Value, out var optionLocalization) || optionLocalization == null)
                            {
                                optionLocalizations[option.Name.Value] = optionLocalization = new OptionLocalizationJsonModel();
                                isInitial = true;
                            }

                            if (isInitial)
                            {
                                optionLocalization.Name = GetLocaleDefault(isDefaultLocale, optionName);
                                optionLocalization.Description = GetLocaleDefault(isDefaultLocale, optionDescription);
                            }
                            else
                            {
                                if (!string.IsNullOrWhiteSpace(optionLocalization.Name) && optionName != optionLocalization.Name)
                                {
                                    option.AddNameLocalization(locale, optionLocalization.Name);
                                }

                                if (!string.IsNullOrWhiteSpace(optionLocalization.Description) && optionDescription != optionLocalization.Description)
                                {
                                    option.AddDescriptionLocalization(locale, optionLocalization.Description);
                                }
                            }

                            if (option.Options.TryGetValue(out var suboptions) && suboptions != null)
                            {
                                var isInitialOption = isInitial;
                                if (!optionLocalization.Options.TryGetValue(out var suboptionLocalizations) || suboptionLocalizations == null)
                                {
                                    optionLocalization.Options = suboptionLocalizations = new Dictionary<string, OptionLocalizationJsonModel?>();
                                    isInitialOption = true;
                                }

                                ReadOptions(locale, isDefaultLocale, isInitialOption, suboptionLocalizations, suboptions);
                            }

                            if (!option.Choices.TryGetValue(out var choices) || choices == null)
                                continue;

                            if (isInitial || !optionLocalization.Choices.TryGetValue(out var choiceLocalizations) || choiceLocalizations == null)
                            {
                                optionLocalization.Choices = choiceLocalizations = new Dictionary<string, ChoiceLocalizationJsonModel?>();
                            }

                            foreach (var choice in choices)
                            {
                                OptionalGuard.HasValue(choice.Name);
                                var choiceName = choice.Name.Value;

                                if (isInitial || !choiceLocalizations.TryGetValue(choiceName, out var choiceLocalization) || choiceLocalization == null)
                                {
                                    choiceLocalizations[choiceName] = new ChoiceLocalizationJsonModel
                                    {
                                        Name = GetLocaleDefault(isDefaultLocale, choiceName)
                                    };

                                    continue;
                                }

                                if (!string.IsNullOrWhiteSpace(choiceLocalization.Name) && choiceName != choiceLocalization.Name)
                                {
                                    choice.AddNameLocalization(locale, choiceLocalization.Name);
                                }
                            }
                        }
                    }

                    ReadOptions(Locale, isDefaultLocale, isInitial, optionLocalizations, options);
                }
            }
        }

        public void Execute(Snowflake? guildId, IEnumerable<LocalApplicationCommand> commands)
        {
            var model = Model;
            LocalizationNodeJsonModel? localizationNode;
            if (guildId == null)
            {
                localizationNode = model.GlobalLocalizations ??= new();
            }
            else
            {
                model.GuildLocalizations ??= new Dictionary<Snowflake, LocalizationNodeJsonModel>();

                if (!model.GuildLocalizations.TryGetValue(guildId.Value, out localizationNode))
                    model.GuildLocalizations[guildId.Value] = localizationNode = new LocalizationNodeJsonModel();
            }

            ReadNode(localizationNode, commands);
        }
    }
}
