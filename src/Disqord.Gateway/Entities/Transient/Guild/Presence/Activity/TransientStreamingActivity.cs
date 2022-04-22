using Disqord.Gateway.Api.Models;
using Qommon;

namespace Disqord.Gateway
{
    public class TransientStreamingActivity : TransientActivity, IStreamingActivity
    {
        /// <inheritdoc/>
        public string Url => Model.Url.GetValueOrDefault();

        public TransientStreamingActivity(IClient client, ActivityJsonModel model)
            : base(client, model)
        { }

        public override string ToString()
            => $"{Name} at {Url}";
    }
}
