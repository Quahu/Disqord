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

        public static readonly Type[] AllowedTypes = { typeof(string), typeof(int), typeof(double) };

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new InvalidOperationException($"The name cannot be null or whitespace.");

                if(value.Length > MaxNameLength)
                    throw new InvalidOperationException($"The name length must be between 1-{MaxNameLength} characters.");

                _name = value;
            }
        }
        private string _name;

        public object Value
        {
            get => _value;
            set
            {
                if (value == null)
                    throw new ArgumentNullException("Value cannot be null");

                if (!AllowedTypes.Contains(value.GetType()))
                    throw new ArgumentException($"Value type must be a string, integer, or double.");

                if (value is string str && str.Length > MaxValueStringLength)
                    throw new ArgumentOutOfRangeException($"Value length must be between 1-{MaxValueStringLength} characters.");

                _value = value;
            }
        }
        private object _value;

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
        { }
    }
}
