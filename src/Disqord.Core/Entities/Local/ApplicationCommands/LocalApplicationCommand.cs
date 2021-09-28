using System;

namespace Disqord
{
    public abstract class LocalApplicationCommand : ILocalConstruct
    {
        public const int MaxNameLength = 32;

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

        public bool IsEnabledByDefault { get; set; } = true;

        protected LocalApplicationCommand(string name)
        {
            Name = name;
        }

        protected LocalApplicationCommand(LocalApplicationCommand other)
        {
            Name = other.Name;
            IsEnabledByDefault = other.IsEnabledByDefault;
        }

        public abstract LocalApplicationCommand Clone();

        object ICloneable.Clone()
            => Clone();

        public virtual void Validate()
        { }
    }
}
