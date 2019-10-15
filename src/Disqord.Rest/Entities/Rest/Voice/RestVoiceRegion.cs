using Disqord.Models;

namespace Disqord.Rest
{
    public class RestVoiceRegion : RestDiscordEntity
    {
        public string Id { get; }

        public string Name { get; }

        public bool IsVip { get; }

        public bool IsOptimal { get; }

        public bool IsDeprecated { get; }

        public bool IsCustom { get; }

        internal RestVoiceRegion(RestDiscordClient client, VoiceRegionModel model) : base(client)
        {
            Id = model.Id;
            Name = model.Name;
            IsVip = model.Vip;
            IsOptimal = model.Optimal;
            IsDeprecated = model.Deprecated;
            IsCustom = model.Custom;
        }
    }
}
