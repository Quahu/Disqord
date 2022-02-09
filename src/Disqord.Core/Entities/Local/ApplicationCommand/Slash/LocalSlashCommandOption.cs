using System;
using System.Collections.Generic;
using System.Linq;
using Qommon;

namespace Disqord
{
    public class LocalSlashCommandOption : ILocalConstruct
    {
        /// <summary>
        ///     Gets or sets the type of this option.
        /// </summary>
        /// <remarks>
        ///     This property is required.
        /// </remarks>
        public Optional<SlashCommandOptionType> Type { get; set; }

        /// <summary>
        ///     Gets or sets the name of this option.
        /// </summary>
        /// <remarks>
        ///     This property is required.
        /// </remarks>
        public Optional<string> Name { get; set; }

        /// <summary>
        ///     Gets or sets the description of this option/
        /// </summary>
        /// <remarks>
        ///     This property is required.
        /// </remarks>
        public Optional<string> Description { get; set; }

        /// <summary>
        ///     Gets or sets whether this option is required.
        /// </summary>
        /// <remarks>
        ///     If not set, Discord defaults it to <see langword="false"/>.
        /// </remarks>
        public Optional<bool> IsRequired { get; set; }

        /// <summary>
        ///     Gets or sets the choices of this option.
        /// </summary>
        public Optional<IList<LocalSlashCommandOptionChoice>> Choices { get; set; }

        /// <summary>
        ///     Gets or sets whether this option supports auto-complete.
        /// </summary>
        public Optional<bool> HasAutoComplete { get; set; }

        /// <summary>
        ///     Gets or sets the nested options of this option.
        /// </summary>
        public Optional<IList<LocalSlashCommandOption>> Options { get; set; }

        /// <summary>
        ///     Gets or sets the channel types of this option.
        /// </summary>
        public Optional<IList<ChannelType>> ChannelTypes { get; set; }

        public LocalSlashCommandOption()
        { }

        protected LocalSlashCommandOption(LocalSlashCommandOption other)
        {
            Guard.IsNotNull(other);

            Type = other.Type;
            Name = other.Name;
            Description = other.Description;
            IsRequired = other.IsRequired;
            Choices = Optional.Convert(other.Choices, choices => choices?.Select(choice => choice?.Clone()).ToList() as IList<LocalSlashCommandOptionChoice>);
            HasAutoComplete = other.HasAutoComplete;
            Options = Optional.Convert(other.Options, options => options?.Select(option => option?.Clone()).ToList() as IList<LocalSlashCommandOption>);
            ChannelTypes = Optional.Convert(other.ChannelTypes, channelTypes => channelTypes?.ToList() as IList<ChannelType>);
        }

        public virtual LocalSlashCommandOption Clone()
            => new(this);

        object ICloneable.Clone()
            => Clone();
    }
}
