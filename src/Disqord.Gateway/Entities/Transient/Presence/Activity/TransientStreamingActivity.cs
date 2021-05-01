using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway
{
    public class TransientStreamingActivity : TransientActivity, IStreamingActivity
    {
        /// <inheritdoc/>
        public string Url { get; }

        public TransientStreamingActivity(IClient client, ActivityJsonModel model)
            : base(client, model)
        {
            Url = model.Url.GetValueOrDefault();
        }

        public override string ToString()
            => $"{Name} at {Url}";
    }
}
