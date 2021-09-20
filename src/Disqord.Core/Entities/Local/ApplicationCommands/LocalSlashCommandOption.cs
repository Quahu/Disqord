using System;
using System.Collections.Generic;
using System.Linq;

namespace Disqord
{
    public class LocalSlashCommandOption : ILocalConstruct
    {
        public const int MaxNameLength = 32;

        public const int MaxDescriptionLength = 100;

        public const int MaxOptionsAmount = 25;

        public const int MaxChoicesAmount = 25;

        public SlashCommandOptionType Type { get; set; }

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException(nameof(value), "The command's name must not be empty or whitespace.");

                if (value.Length > MaxNameLength)
                    throw new ArgumentOutOfRangeException(nameof(value), $"The command's name must not be longer than {MaxNameLength} characters.");

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

        public IList<LocalSlashCommandOptionChoice> Choices
        {
            get => _choices;
            set => this.WithChoices(value);
        }
        internal readonly List<LocalSlashCommandOptionChoice> _choices;

        public IList<LocalSlashCommandOption> Options
        {
            get => _options;
            set => this.WithOptions(value);
        }
        internal readonly List<LocalSlashCommandOption> _options;

        public IList<ChannelType> ChannelTypes
        {
            get => _channelTypes;
            set => this.WithChannelTypes(value);
        }
        internal readonly List<ChannelType> _channelTypes;

        public LocalSlashCommandOption(SlashCommandOptionType type, string name, string description)
        {
            Type = type;
            Name = name;
            Description = description;

            _choices = new List<LocalSlashCommandOptionChoice>();
            _options = new List<LocalSlashCommandOption>();
            _channelTypes = new List<ChannelType>();
        }

        private LocalSlashCommandOption(LocalSlashCommandOption other)
        {
            Type = other.Type;
            Name = other.Name;
            Description = other.Description;

            _choices = other._choices.Select(x => x.Clone()).ToList();
            _options = other._options.Select(x => x.Clone()).ToList();
            _channelTypes = other._channelTypes.ToList();
        }

        public virtual LocalSlashCommandOption Clone()
            => new(this);

        object ICloneable.Clone()
            => Clone();

        public void Validate()
        {
            if (_choices.Count > MaxChoicesAmount)
                throw new InvalidOperationException($"The command option must not contain more than {MaxChoicesAmount} choices.");

            if (_options.Count > MaxOptionsAmount)
                throw new InvalidOperationException($"The command option must not contain more than {MaxOptionsAmount} options.");

            for (var i = 0; i < _choices.Count; i++)
                _choices[i].Validate();

            for (var i = 0; i < _options.Count; i++)
                _options[i].Validate();
        }
    }
}
