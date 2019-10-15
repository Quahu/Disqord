using System;

namespace Disqord
{
    public sealed class EmbedFieldBuilder
    {
        public const int MAX_FIELD_NAME_LENGTH = 256;

        public const int MAX_FIELD_VALUE_LENGTH = 1024;

        public string Name
        {
            get => _name;
            set
            {
                if (value?.Length > MAX_FIELD_NAME_LENGTH)
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
                if (value?.Length > MAX_FIELD_VALUE_LENGTH)
                    throw new ArgumentOutOfRangeException(nameof(value), $"The value of the embed field must not be longer than {MAX_FIELD_VALUE_LENGTH} characters.");

                _value = value;
            }
        }
        private string _value;

        public bool IsInline { get; set; }

        public EmbedFieldBuilder()
        { }

        public EmbedFieldBuilder(EmbedFieldBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            Name = builder.Name;
            Value = builder.Value;
            IsInline = builder.IsInline;
        }

        public EmbedFieldBuilder WithName(string name)
        {
            Name = name;
            return this;
        }

        public EmbedFieldBuilder WithValue(object value)
        {
            Value = value?.ToString();
            return this;
        }

        public EmbedFieldBuilder WithIsInline(bool isInline)
        {
            IsInline = isInline;
            return this;
        }

        public EmbedField Build()
            => new EmbedField(this);
    }
}
