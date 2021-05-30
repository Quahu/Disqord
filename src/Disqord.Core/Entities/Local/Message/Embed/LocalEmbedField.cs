using System;

namespace Disqord
{
    public class LocalEmbedField : ILocalConstruct
    {
        public const int MAX_FIELD_NAME_LENGTH = 256;

        public const int MAX_FIELD_VALUE_LENGTH = 1024;

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException(nameof(value), "The embed field's name must not be null or whitespace.");

                if (value.Length > MAX_FIELD_NAME_LENGTH)
                    throw new ArgumentOutOfRangeException(nameof(value), $"The name of the embed field must not be longer than {MAX_FIELD_NAME_LENGTH} characters.");

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

                if (value.Length > MAX_FIELD_VALUE_LENGTH)
                    throw new ArgumentOutOfRangeException(nameof(value), $"The value of the embed field must not be longer than {MAX_FIELD_VALUE_LENGTH} characters.");

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

        public LocalEmbedField WithName(string name)
        {
            Name = name;
            return this;
        }

        public LocalEmbedField WithBlankName()
            => WithName("\u200b");

        public LocalEmbedField WithValue(string value)
        {
            Value = value;
            return this;
        }

        public LocalEmbedField WithValue(object value)
        {
            Value = value?.ToString();
            return this;
        }

        public LocalEmbedField WithBlankValue()
            => WithValue("\u200b");

        public LocalEmbedField WithIsInline(bool isInline)
        {
            IsInline = isInline;
            return this;
        }

        public LocalEmbedField Clone()
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
