using System;

namespace Disqord
{
    public class LocalEmbedField : ILocalConstruct
    {
        public const int MaxFieldNameLength = 256;

        public const int MaxFieldValueLength = 1024;

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException(nameof(value), "The embed field's name must not be null or whitespace.");

                if (value.Length > MaxFieldNameLength)
                    throw new ArgumentOutOfRangeException(nameof(value), $"The name of the embed field must not be longer than {MaxFieldNameLength} characters.");

                _name = value;
            }
        }
        private string _name;

        public string Value
        {
            get => _value;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException(nameof(value), "The embed field's value must not be null or whitespace.");

                if (value.Length > MaxFieldValueLength)
                    throw new ArgumentOutOfRangeException(nameof(value), $"The value of the embed field must not be longer than {MaxFieldValueLength} characters.");

                _value = value;
            }
        }
        private string _value;

        public bool IsInline { get; set; }

        public int Length
        {
            get
            {
                var nameLength = _name?.Length ?? 0;
                var valueLength = _value?.Length ?? 0;
                return nameLength + valueLength;
            }
        }

        public LocalEmbedField()
        { }

        private LocalEmbedField(LocalEmbedField other)
        {
            _name = other.Name;
            _value = other.Value;
            IsInline = other.IsInline;
        }

        public virtual LocalEmbedField Clone()
            => new(this);

        object ICloneable.Clone()
            => Clone();

        public void Validate()
        {
            if (_name == null)
                throw new InvalidOperationException("The embed field's name must be set.");

            if (_value == null)
                throw new InvalidOperationException("The embed field's value must be set.");
        }
    }
}
