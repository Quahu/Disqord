using System.Collections.Generic;
using System.Linq;
using Qommon;

namespace Disqord
{
    public class LocalSlashCommand : LocalApplicationCommand
    {
        /// <summary>
        ///     Gets or sets the description of this command.
        /// </summary>
        /// <remarks>
        ///     This property is required.
        /// </remarks>
        public Optional<string> Description { get; set; }

        /// <summary>
        ///     Gets or sets the options of this command.
        /// </summary>
        public Optional<IList<LocalSlashCommandOption>> Options { get; set; }

        public LocalSlashCommand()
        { }

        protected LocalSlashCommand(LocalSlashCommand other)
            : base(other)
        {
            Description = other.Description;
            Options = Optional.Convert(other.Options, options => options?.Select(option => option?.Clone()).ToList() as IList<LocalSlashCommandOption>);
        }

        public override LocalSlashCommand Clone()
            => new(this);
    }
}
