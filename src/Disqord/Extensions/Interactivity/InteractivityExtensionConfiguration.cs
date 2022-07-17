using System;

namespace Disqord.Extensions.Interactivity;

public class InteractivityExtensionConfiguration
{
    /// <summary>
    ///     Gets or sets the default timeout used for waiting for events.
    ///     Defaults to <c>30</c> seconds.
    /// </summary>
    public virtual TimeSpan DefaultWaitTimeout { get; set; } = TimeSpan.FromSeconds(30);

    /// <summary>
    ///     Gets or sets the default timeout used for menus.
    ///     Defaults to <c>15</c> minutes.
    /// </summary>
    public virtual TimeSpan DefaultMenuTimeout { get; set; } = TimeSpan.FromMinutes(15);
}