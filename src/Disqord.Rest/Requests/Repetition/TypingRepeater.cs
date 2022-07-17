using System;
using System.Threading;
using System.Threading.Tasks;

namespace Disqord.Rest.Repetition;

/// <summary>
///     Represents a repeater that intervally triggers typing in a channel.
/// </summary>
public class TypingRepeater : Repeater
{
    /// <inheritdoc/>
    public override TimeSpan Interval => TimeSpan.FromSeconds(7.5);

    /// <summary>
    ///     Gets the ID of the channel to trigger typing in.
    /// </summary>
    public Snowflake ChannelId { get; }

    /// <summary>
    ///     Instantiates a new <see cref="TypingRepeater"/>.
    /// </summary>
    /// <param name="client"> The client to execute the requests with. </param>
    /// <param name="channelId"> The ID of the channel to trigger typing in. </param>
    /// <param name="options"> The request options. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    protected internal TypingRepeater(IRestClient client,
        Snowflake channelId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
        : base(client, options, cancellationToken)
    {
        ChannelId = channelId;
    }

    /// <inheritdoc/>
    protected override Task ExecuteAsync(CancellationToken cancellationToken)
    {
        return Client.TriggerTypingAsync(ChannelId, Options, cancellationToken);
    }
}
