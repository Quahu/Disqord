﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Disqord
{
    public class LocalApplicationCommandOption : ILocalConstruct
    {
        public const int MaxChoiceSize = 25;

        public const int MaxNameLength = 32;

        public const int MaxDescriptionLength = 100;

        public const string NameRegex = "^[\\w -]{1, 32}$";

        public ApplicationCommandOptionType Type { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool Required { get; set; }

        public IList<LocalApplicationCommandOptionChoice> Choices
        {
            get => _choices;
            set => WithChoices(value);
        }
        internal readonly List<LocalApplicationCommandOptionChoice> _choices;

        public IList<LocalApplicationCommandOption> Options
        {
            get => _options;
            set => WithOptions(value);
        }
        internal readonly List<LocalApplicationCommandOption> _options;

        public LocalApplicationCommandOption(ApplicationCommandOptionType type, string name, string description)
        {
            Type = type;
            Name = name;
            Description = description;

            _choices = new List<LocalApplicationCommandOptionChoice>(MaxChoiceSize);
            _options = new List<LocalApplicationCommandOption>();
        }

        public LocalApplicationCommandOption(LocalApplicationCommandOption other)
        {
            Type = other.Type;
            Name = other.Name;
            Description = other.Description;

            _choices = other._choices.Select(x => x.Clone()).ToList();
            _options = other._options.Select(x => x.Clone()).ToList();
        }

        public LocalApplicationCommandOption WithType(ApplicationCommandOptionType type)
        {
            Type = type;
            return this;
        }

        public LocalApplicationCommandOption WithName(string name)
        {
            Name = name;
            return this;
        }

        public LocalApplicationCommandOption WithRequired(bool required)
        {
            Required = required;
            return this;
        }

        public LocalApplicationCommandOption WithChoices(IEnumerable<LocalApplicationCommandOptionChoice> choices)
        {
            if (choices == null)
                throw new ArgumentNullException(nameof(choices));

            _choices.Clear();
            _choices.AddRange(choices);
            return this;
        }

        public LocalApplicationCommandOption WithOptions(IEnumerable<LocalApplicationCommandOption> options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            _options.Clear();
            _options.AddRange(options);
            return this;
        }

        object ICloneable.Clone()
            => Clone();

        public LocalApplicationCommandOption Clone()
            => new(this);

        public void Validate()
        {
            if (!Regex.IsMatch(Name, NameRegex))
                throw new InvalidOperationException($"Name must be lowercase and consist of 1-{MaxNameLength} characters.");

            if (string.IsNullOrWhiteSpace(Description) && Description.Length > MaxDescriptionLength)
                throw new InvalidOperationException($"Description cannot be null or whitespace, and must be between 1-{MaxDescriptionLength} characters");

            for (var i = 0; i < _choices.Count; i++)
                _choices[i].Validate();

            for (var i = 0; i < _options.Count; i++)
                _options[i].Validate();
        }
    }
}