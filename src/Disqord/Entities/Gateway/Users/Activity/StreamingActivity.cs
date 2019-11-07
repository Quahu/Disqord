using Disqord.Models;

namespace Disqord
{
    public sealed class StreamingActivity : Activity
    {
        public string Url { get; }

        internal StreamingActivity(ActivityModel model) : base(model)
        {
            Url = model.Url;
        }
    }
}
