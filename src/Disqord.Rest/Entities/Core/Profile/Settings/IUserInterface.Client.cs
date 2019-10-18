using System;
using System.Threading.Tasks;

namespace Disqord
{
    public partial interface IUserSettings : IDiscordEntity
    {
        Task ModifyAsync(Action<ModifyUserSettingsProperties> action, RestRequestOptions options = null);
    }
}