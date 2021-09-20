using System;
using System.Threading.Tasks;

namespace Disqord.Rest
{
    public static partial class RestEntityExtensions
    {
        public static Task<IGuildTemplate> SynchronizeAsync(this IGuildTemplate template, IRestRequestOptions options = null)
        {
            var client = template.GetRestClient();
            return client.SynchronizeTemplateAsync(template.GuildId, template.Code, options);
        }

        public static Task<IGuildTemplate> ModifyAsync(this IGuildTemplate template, Action<ModifyTemplateActionProperties> action, IRestRequestOptions options = null)
        {
            var client = template.GetRestClient();
            return client.ModifyTemplateAsync(template.GuildId, template.Code, action, options);
        }

        public static Task<IGuildTemplate> DeleteAsync(this IGuildTemplate template, IRestRequestOptions options = null)
        {
            var client = template.GetRestClient();
            return client.DeleteTemplateAsync(template.GuildId, template.Code, options);
        }
    }
}
