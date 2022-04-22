using System;
using Qommon;

namespace Disqord
{
    public class LocalSlashCommandOptionChoice : ILocalConstruct
    {
        public Optional<string> Name { get; set; }

        public Optional<object> Value { get; set; }

        public LocalSlashCommandOptionChoice()
        { }

        protected LocalSlashCommandOptionChoice(LocalSlashCommandOptionChoice other)
        {
            Name = other.Name;
            Value = other.Name;
        }

        public virtual LocalSlashCommandOptionChoice Clone()
            => new(this);

        object ICloneable.Clone()
            => Clone();
    }
}
