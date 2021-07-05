namespace Disqord.Models
{
    public static partial class LocalEntityExtensions
    {
        public static EmojiJsonModel ToModel(this LocalEmoji emoji)
        {
            if (emoji == null)
                return null;

            var model = new EmojiJsonModel
            {
                Name = emoji.Name
            };

            if (emoji is LocalCustomEmoji customEmoji)
            {
                model.Id = customEmoji.Id;
                model.Animated = customEmoji.IsAnimated;
            }

            return model;
        }
    }
}
