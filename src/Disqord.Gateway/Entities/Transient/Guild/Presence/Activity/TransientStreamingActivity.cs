using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway;

public class TransientStreamingActivity : TransientActivity, IStreamingActivity
{
    /// <inheritdoc/>
    public string Url => Model.Url.Value;

    public TransientStreamingActivity(IClient client, ActivityJsonModel model)
        : base(client, model)
    { }

    public override string ToString()
    {
        return $"{Name} at {Url}";
    }
}