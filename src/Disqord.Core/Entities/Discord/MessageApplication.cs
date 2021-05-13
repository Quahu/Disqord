using Disqord.Models;

namespace Disqord
{
    public sealed class MessageApplication : IIdentifiable, INamable
    {
        public Snowflake Id { get; }

        public string Name { get; }

        public string CoverImageHash { get; }

        public string Description { get; }

        public string IconHash { get; }

        public MessageApplication(MessageApplicationJsonModel model)
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
