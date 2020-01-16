using Disqord.Models;

namespace Disqord
{
    /// <summary>
    ///     Represents a user's custom activity.
    /// </summary>
    public sealed class CustomActivity : Activity
    {
        public string Text { get; }

        public IEmoji Emoji { get; }

        internal CustomActivity(ActivityModel model) : base(model)
        {
            Text = model.State;

            if (model.Emoji != null)
                Emoji = model.Emoji.ToEmoji();
        }

        public override string ToString()
            => Emoji != null
                ? $"{Emoji} {Text}"
                : Text;
    }
}
