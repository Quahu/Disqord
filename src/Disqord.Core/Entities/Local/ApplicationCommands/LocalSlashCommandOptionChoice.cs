using System;
using System.Collections.Generic;
using System.Linq;
using Qommon.Collections;

namespace Disqord
{
    public class LocalSlashCommandOptionChoice : ILocalConstruct
    {
        public const int MaxNameLength = 100;

        public const int MaxStringValueLength = 100;

        public static readonly IReadOnlyList<Type> AllowedTypes = new [] { typeof(string), typeof(int), typeof(double) }.ReadOnly();

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException(nameof(value), "The command option choice's name must not be empty or whitespace.");

                if (value.Length > MaxNameLength)
                    throw new ArgumentOutOfRangeException(nameof(value), $"The command option choice's name must not be longer than {MaxNameLength} characters.");

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
                    throw new ArgumentNullException(nameof(value), "The command option choice's value must not be null");

                if (!AllowedTypes.Contains(value.GetType()))
                    throw new ArgumentException("The command option choice's value must be of type: string, int, or double.");

                if (value is string str && str.Length > MaxStringValueLength)
                    throw new ArgumentOutOfRangeException(nameof(value), $"The command option choice's value must not be longer than {MaxStringValueLength} characters.");

                _value = value;
            }
        }
        private object _value;

        public LocalSlashCommandOptionChoice(string name, object value)
        {
            Name = name;
            Value = value;
        }

        private LocalSlashCommandOptionChoice(LocalSlashCommandOptionChoice other)
        {
            Name = other.Name;
            Value = other.Name;
        }

        public virtual LocalSlashCommandOptionChoice Clone()
            => new(this);

        object ICloneable.Clone()
            => Clone();

        public void Validate()
        { }
    }
}
