using System;
using System.Collections.Generic;
using System.Globalization;
using Qommon;

namespace Disqord
{
    public abstract class LocalApplicationCommand : ILocalConstruct
    {
        /// <summary>
        ///     Gets or sets the name of this command.
        /// </summary>
        /// <remarks>
        ///     This property is required.
        /// </remarks>
        public Optional<string> Name { get; set; }

        /// <summary>
        ///     Gets or sets the localizations of the name of this command.
        /// </summary>
        public Optional<IDictionary<CultureInfo, string>> NameLocalizations { get; set; }

        /// <summary>
        ///     Gets or sets whether this command is enabled by default.
        /// </summary>
        /// <remarks>
        ///     If not set, defaults to <see langword="true"/>.
        /// </remarks>
        public Optional<bool> IsEnabledByDefault { get; set; }

        /// <summary>
        ///     Gets or sets the default required permissions of members of this command.
        /// </summary>
        public Optional<Permission> DefaultRequiredMemberPermissions { get; set; }

        /// <summary>
        ///     Gets or sets whether this command is enabled in private channels.
        /// </summary>
        public Optional<bool> IsEnabledInPrivateChannels { get; set; }

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
    }
}
