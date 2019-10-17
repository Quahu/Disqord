using System;
using System.Threading.Tasks;

namespace Disqord
{
    public partial interface IVoiceChannel : IGuildChannel
    {
        Task ModifyAsync(Action<ModifyVoiceChannelProperties> action, RestRequestOptions options = null);
    }
}
