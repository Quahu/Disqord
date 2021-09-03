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

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsEnabledByDefault { get; set; }

        public IList<LocalApplicationCommandOption> Options
        {
            get => _options;
            set => WithOptions(value);
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

        public LocalApplicationCommand WithOptions(IEnumerable<LocalApplicationCommandOption> options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            _options.Clear();
            _options.AddRange(options);
            return this;
        }

        object ICloneable.Clone()
            => Clone();

        public LocalApplicationCommand Clone()
            => new(this);

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Name) && Name.Length > MaxNameLength)
                throw new InvalidOperationException($"Name must not be null and can only consist of 1-{MaxNameLength} characters.");

            if (string.IsNullOrWhiteSpace(Description) && Description.Length > MaxDescriptionLength)
                throw new InvalidOperationException($"Description cannot be null or whitespace, and must be between 1-{MaxDescriptionLength} characters");

            for (var i = 0; i < _options.Count; i++)
                _options[i].Validate();
        }
    }
}
