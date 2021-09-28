using System;
using System.Collections.Generic;
using System.Linq;

namespace Disqord
{
    public class LocalSlashCommand : LocalApplicationCommand
    {
        public const int MaxDescriptionLength = 100;

        public const int MaxOptionsAmount = 25;

        public string Description
        {
            get => _description;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException(nameof(value), "The command's description must not be empty or whitespace.");

                if (value.Length > MaxDescriptionLength)
                    throw new ArgumentOutOfRangeException($"The command's description must not be longer than {MaxDescriptionLength} characters.");

                _description = value;
            }
        }
        private string _description;

        public IList<LocalSlashCommandOption> Options
        {
            get => _options;
            set => this.WithOptions(value);
        }
        internal readonly List<LocalSlashCommandOption> _options;

        public LocalSlashCommand(string name, string description)
            : base(name)
        {
            Description = description;
            _options = new List<LocalSlashCommandOption>();
        }

        protected LocalSlashCommand(LocalSlashCommand other)
            : base(other)
        {
            Description = other.Description;
            _options = other.Options.Select(x => x.Clone()).ToList();
        }

        public override LocalSlashCommand Clone()
            => new(this);


        public override void Validate()
        {
            if (_options.Count > MaxOptionsAmount)
                throw new InvalidOperationException($"The command must not contain more than {MaxOptionsAmount} options.");

            for (var i = 0; i < _options.Count; i++)
                _options[i].Validate();

            base.Validate();
        }
    }
}
