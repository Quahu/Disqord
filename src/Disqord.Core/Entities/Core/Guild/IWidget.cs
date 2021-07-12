using Disqord.Models;

namespace Disqord
{
    /// <summary>
    ///     Represents widget settings.
    /// </summary>
    public interface IWidget : IJsonUpdatable<GuildWidgetSettingsJsonModel>
    {
        /// <summary>
        ///     Gets whether the widget is enabled.
        /// </summary>
        bool Enabled { get; }

        /// <summary>
        ///     The id of the widget channel.
        /// </summary>
        Snowflake ChannelId { get; }
    }
}