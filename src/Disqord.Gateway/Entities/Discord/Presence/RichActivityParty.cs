using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway
{
    public class RichActivityParty
    {
        public string Id { get; }

        public int? Size { get; }

        public int? MaxSize { get; }

        public RichActivityParty(ActivityPartyJsonModel model)
        {
            Id = model.Id.GetValueOrDefault();
            Size = model.Size.GetValueOrDefault()?[0];
            MaxSize = model.Size.GetValueOrDefault()?[1];
        }
    }
}
