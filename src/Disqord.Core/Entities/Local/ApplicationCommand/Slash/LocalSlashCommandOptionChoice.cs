using System;
using System.Collections.Generic;
using System.Globalization;
using Qommon;

namespace Disqord
{
    public class LocalSlashCommandOptionChoice : ILocalConstruct
    {
        /// <summary>
        ///     Gets or sets the name of this choice.
        /// </summary>
        /// <remarks>
        ///     This property is required.
        /// </remarks>
        public Optional<string> Name { get; set; }

        /// <summary>
        ///     Gets or sets the localizations of the name of this choice.
        /// </summary>
        public Optional<IDictionary<CultureInfo, string>> NameLocalizations { get; set; }

        /// <summary>
        ///     Gets or sets the value of this choice.
        /// </summary>
        /// <remarks>
        ///     This property is required.
        /// </remarks>
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
