using System;

namespace Disqord
{
    public sealed class LocalEmbedFieldBuilder : ICloneable
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

        public LocalEmbedFieldBuilder()
        { }

        internal LocalEmbedFieldBuilder(LocalEmbedFieldBuilder builder)
        {
            _name = builder.Name;
            _value = builder.Value;
            IsInline = builder.IsInline;
        }

        public LocalEmbedFieldBuilder WithName(string name)
        {
            Name = name;
            return this;
        }

        public LocalEmbedFieldBuilder WithBlankName()
            => WithName("\u200b");

        public LocalEmbedFieldBuilder WithValue(string value)
        {
            Value = value;
            return this;
        }

        public LocalEmbedFieldBuilder WithValue(object value)
        {
            Value = value?.ToString();
            return this;
        }

        public LocalEmbedFieldBuilder WithBlankValue()
            => WithValue("\u200b");

        public LocalEmbedFieldBuilder WithIsInline(bool isInline)
        {
            IsInline = isInline;
            return this;
        }

        public LocalEmbedFieldBuilder Clone()
            => new LocalEmbedFieldBuilder(this);

        object ICloneable.Clone()
            => Clone();

        internal LocalEmbedField Build()
        {
            if (_name == null)
                throw new InvalidOperationException("The embed field's name must be set.");

            if (_value == null)
                throw new InvalidOperationException("The embed field's value must be set.");

            return new LocalEmbedField(this);
        }
    }
}
