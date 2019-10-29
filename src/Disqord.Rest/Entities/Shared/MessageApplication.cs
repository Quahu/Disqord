using Disqord.Models;

namespace Disqord
{
    public sealed class MessageApplication
    {
        public Snowflake Id { get; }

        public string CoverImageHash { get; }

        public string Description { get; }

        public string IconHash { get; }

        public string Name { get; }

        internal MessageApplication(MessageApplicationModel model)
        {
            Id = model.Id;
            CoverImageHash = model.CoverImage.GetValueOrDefault();
            Description = model.Description;
            IconHash = model.Icon;
            Name = model.Name;
        }

        public override string ToString()
            => $"{Name} ({Id})";
    }
}
