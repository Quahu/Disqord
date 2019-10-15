using Disqord.Models;

namespace Disqord
{
    internal static partial class ModelExtensions
    {
        public static IEmoji ToEmoji(this EmojiModel model)
        {
            if (model.Id == null)
                return new Emoji
                {
                    Name = model.Name
                };
            else
                return new CustomEmoji
                {
                    Id = model.Id.Value,
                    Name = model.Name,
                    IsAnimated = model.Animated
                };
        }
    }
}
