namespace Disqord.Gateway;

public static partial class GatewayEntityExtensions
{
    /// <summary>
    ///     Gets the cached channel of this stage.
    /// </summary>
    /// <param name="stage"> The stage to get the channel of. </param>
    /// <returns>
    ///     The channel or <see langword="null"/> if it was not cached.
    /// </returns>
    public static CachedStageChannel? GetChannel(this IStage stage)
    {
        var client = stage.GetGatewayClient();
        return client.GetChannel(stage.GuildId, stage.ChannelId) as CachedStageChannel;
    }
}
