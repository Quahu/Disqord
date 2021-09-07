using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Disqord
{
    public class LocalApplicationCommand : ILocalConstruct
    {
        public const int MaxNameLength = 32;

        public const int MaxDescriptionLength = 100;

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException($"The name cannot be null or whitespace, and must be between 1-{MaxNameLength} characters.");

                if (value.Length > MaxNameLength)
                    throw new ArgumentOutOfRangeException($"The name length must be between 1-{MaxNameLength} characters.");

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
                    throw new InvalidOperationException($"The description cannot be null or whitespace.");

                if (value.Length > MaxDescriptionLength)
                    throw new ArgumentOutOfRangeException($"The description must be between 1-{MaxDescriptionLength} characters.");

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

        public LocalApplicationCommand(LocalApplicationCommand other)
        {
            Name = other.Name;
            Description = other.Description;
            IsEnabledByDefault = other.IsEnabledByDefault;

            _options = other.Options.Select(x => x.Clone()).ToList();
        }

        object ICloneable.Clone()
            => Clone();

        public LocalApplicationCommand Clone()
            => new(this);

        public void Validate()
        {
            for (var i = 0; i < _options.Count; i++)
                _options[i].Validate();
        }
    }
}
