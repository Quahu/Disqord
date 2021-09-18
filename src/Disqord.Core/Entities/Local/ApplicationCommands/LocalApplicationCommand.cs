using System;
using System.Collections.Generic;
using System.Linq;

namespace Disqord
{
    public class LocalApplicationCommand : ILocalConstruct
    {
        public const int MaxNameLength = 32;

        public const int MaxDescriptionLength = 100;

        public const int MaxOptionsAmount = 25;

        public ApplicationCommandType Type { get; set; } = ApplicationCommandType.Text;

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
                    throw new ArgumentNullException(nameof(value), "The command's description must not be empty or whitespace.");

                if (value.Length > MaxDescriptionLength)
                    throw new ArgumentOutOfRangeException($"The command's description must not be longer than {MaxDescriptionLength} characters.");

                _description = value;
            }
        }
        private string _description;

        public bool IsEnabledByDefault { get; set; }

        public IList<LocalApplicationCommandOption> Options
        {
            get => _options;
            set => this.WithOptions(value);
        }
        internal readonly List<LocalApplicationCommandOption> _options;

        public LocalApplicationCommand(string name, string description)
        {
            Name = name;
            Description = description;
            _options = new List<LocalApplicationCommandOption>();
        }

        private LocalApplicationCommand(LocalApplicationCommand other)
        {
            Name = other.Name;
            Description = other.Description;
            IsEnabledByDefault = other.IsEnabledByDefault;

            _options = other.Options.Select(x => x.Clone()).ToList();
        }

        public virtual LocalApplicationCommand Clone()
            => new(this);

        object ICloneable.Clone()
            => Clone();

        public void Validate()
        {
            if (_options.Count > MaxOptionsAmount)
                throw new InvalidOperationException($"The command must not contain more than {MaxOptionsAmount} options.");

            for (var i = 0; i < _options.Count; i++)
                _options[i].Validate();
        }
    }
}
