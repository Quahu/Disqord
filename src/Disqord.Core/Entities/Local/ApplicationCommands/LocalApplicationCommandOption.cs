using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Disqord
{
    public class LocalApplicationCommandOption : ILocalConstruct
    {
        public const int MaxNameLength = 32;

        public const int MaxDescriptionLength = 100;

        public const int MaxOptionsAmount = 25;

        public const int MaxChoicesAmount = 25;

        public const string NameRegex = "^[\\w -]{1, 32}$";

        public ApplicationCommandOptionType Type { get; set; }

        public string Name
        {
            get => _name;
            set
            {
                if (!Regex.IsMatch(Name, NameRegex))
                    throw new ArgumentException($"The command option's name must not be empty or whitespace, must be lowercase, and must be between of 1-{MaxNameLength} characters.");

                _name = value;
            }
        }
        private string _name;

        public string Description
        {
            get => _description;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException(nameof(value), "The command option's description must not be empty or whitespace.");

                if (value.Length > MaxDescriptionLength)
                    throw new ArgumentOutOfRangeException($"The command option's description must not be longer than {MaxDescriptionLength} characters.");

                _description = value;
            }
        }
        private string _description;

        public bool IsRequired { get; set; }

        public IList<LocalApplicationCommandOptionChoice> Choices
        {
            get => _choices;
            set => this.WithChoices(value);
        }
        internal readonly List<LocalApplicationCommandOptionChoice> _choices;

        public IList<LocalApplicationCommandOption> Options
        {
            get => _options;
            set => this.WithOptions(value);
        }
        internal readonly List<LocalApplicationCommandOption> _options;

        public IList<ChannelType> ChannelTypes
        {
            get => _channelTypes;
            set => this.WithChannelTypes(value);
        }
        internal readonly List<ChannelType> _channelTypes;

        public LocalApplicationCommandOption(ApplicationCommandOptionType type, string name, string description)
        {
            Type = type;
            Name = name;
            Description = description;

            _choices = new List<LocalApplicationCommandOptionChoice>();
            _options = new List<LocalApplicationCommandOption>();
            _channelTypes = new List<ChannelType>();
        }

        public LocalApplicationCommandOption(LocalApplicationCommandOption other)
        {
            Type = other.Type;
            Name = other.Name;
            Description = other.Description;

            _choices = other._choices.Select(x => x.Clone()).ToList();
            _options = other._options.Select(x => x.Clone()).ToList();
            _channelTypes = other._channelTypes.ToList();
        }

        object ICloneable.Clone()
            => Clone();

        public LocalApplicationCommandOption Clone()
            => new(this);

        public void Validate()
        {
            if (_options.Count > MaxOptionsAmount)
                throw new InvalidOperationException($"The command option must not contain more than {MaxOptionsAmount} options.");

            if (_choices.Count > MaxChoicesAmount)
                throw new InvalidOperationException($"The command option must not contain more than {MaxChoicesAmount} choices.");

            for (var i = 0; i < _choices.Count; i++)
                _choices[i].Validate();

            for (var i = 0; i < _options.Count; i++)
                _options[i].Validate();
        }
    }
}
