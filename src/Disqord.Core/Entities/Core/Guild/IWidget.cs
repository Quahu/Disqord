using Disqord.Models;

namespace Disqord
{
    /// <summary>
    ///     Represents a guild's widget settings.
    /// </summary>
    public interface IWidget : IGuildEntity, IChannelEntity, IJsonUpdatable<GuildWidgetSettingsJsonModel>
    {
        /// <summary>
        ///     Gets whether the widget is enabled.
        /// </summary>
        bool IsEnabled { get; }
    }
}
