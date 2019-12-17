using System;
using System.Threading.Tasks;
using Disqord.Rest;

namespace Disqord
{
    public partial interface IUserSettings : IDiscordEntity
    {
        Task ModifyAsync(Action<ModifyUserSettingsProperties> action, RestRequestOptions options = null);
    }
}