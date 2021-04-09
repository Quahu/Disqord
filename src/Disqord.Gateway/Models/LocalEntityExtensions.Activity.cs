using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway.Models
{
    public static class LocalEntityExtensions
    {
        public static ActivityJsonModel ToModel(this LocalActivity activity)
            => new()
            {
                Name = activity.Name,
                Type = activity.Type,
                Url = activity.Url
            };
    }
}
