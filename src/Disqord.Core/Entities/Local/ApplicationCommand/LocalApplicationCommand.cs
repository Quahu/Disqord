using System;
using Qommon;

namespace Disqord
{
    public abstract class LocalApplicationCommand : ILocalConstruct
    {
        /// <summary>
        ///     Gets or sets the name of this command.
        ///     This property is required.
        /// </summary>
        public Optional<string> Name { get; set; }

        /// <summary>
        ///     Gets or sets whether this command is enabled by default or not.
        ///     If not set, defaults to <see langword="true"/>.
        /// </summary>
        public Optional<bool> IsEnabledByDefault { get; set; }

        protected LocalApplicationCommand()
        { }

        protected LocalApplicationCommand(LocalApplicationCommand other)
        {
            Guard.IsNotNull(other);

            Name = other.Name;
            IsEnabledByDefault = other.IsEnabledByDefault;
        }

        public abstract LocalApplicationCommand Clone();

        object ICloneable.Clone()
            => Clone();

        public void Validate()
        { }
    }
}
