using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Disqord
{
    public class LocalApplicationCommandOptionChoice : ILocalConstruct
    {
        public const int MaxNameLength = 100;

        public const int MaxValueStringLength = 100;

        public Type[] AllowedTypes = { typeof(string), typeof(int), typeof(double) };

        public string Name { get; set; }

        public object Value { get; set; }

        public LocalApplicationCommandOptionChoice(string name, object value)
        {
            Name = name;
            Value = value;
        }

        public LocalApplicationCommandOptionChoice(LocalApplicationCommandOptionChoice other)
        {
            Name = other.Name;
            Value = other.Name;
        }

        public LocalApplicationCommandOptionChoice WithName(string name)
        {
            Name = name;
            return this;
        }

        public LocalApplicationCommandOptionChoice WithValue(object value)
        {
            Value = value;
            return this;
        }

        object ICloneable.Clone()
            => Clone();

        public LocalApplicationCommandOptionChoice Clone()
            => new(this);

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Name) && Name.Length > MaxNameLength)
                throw new InvalidOperationException($"Name cannot be null or whitespace, and must be between 1-{MaxNameLength} characters");

            if (!AllowedTypes.Contains(Value.GetType()))
                throw new InvalidOperationException($"Value type must be a string, integer, or double");

            if (Value is string str && str.Length > MaxValueStringLength)
                throw new InvalidOperationException($"Value length cannot exceed {MaxValueStringLength} characters as a string");
        }
    }
}
