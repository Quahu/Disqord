using System;

namespace Disqord
{
    public sealed class LocalEmbedFieldBuilder
    {
        public const int MAX_FIELD_NAME_LENGTH = 256;

        public const int MAX_FIELD_VALUE_LENGTH = 1024;

        public string Name
        {
            get => _name;
            set
            {
                if (value != null && value.Length > MAX_FIELD_NAME_LENGTH)
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
                if (value != null && value.Length > MAX_FIELD_VALUE_LENGTH)
                    throw new ArgumentOutOfRangeException(nameof(value), $"The value of the embed field must not be longer than {MAX_FIELD_VALUE_LENGTH} characters.");

                _value = value;
            }
        }
        private string _value;

        public bool IsInline { get; set; }

        public LocalEmbedFieldBuilder()
        { }

        public LocalEmbedFieldBuilder(LocalEmbedFieldBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            Name = builder.Name;
            Value = builder.Value;
            IsInline = builder.IsInline;
        }

        public LocalEmbedFieldBuilder WithName(string name)
        {
            Name = name;
            return this;
        }

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

        public LocalEmbedFieldBuilder WithIsInline(bool isInline)
        {
            IsInline = isInline;
            return this;
        }

        internal LocalEmbedField Build()
            => new LocalEmbedField(this);
    }
}
