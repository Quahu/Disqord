namespace Disqord.Rest;

/// <summary>
///     Represents the type of an interaction response deferral.
/// </summary>
/// <seealso cref="InteractionResponseHelper"/>
/// <seealso cref="InteractionResponseHelper.DeferAsync(DeferralType,bool,IRestRequestOptions,System.Threading.CancellationToken)"/>
public enum DeferralType
{
    /// <summary>
    ///     The followup will send a new message (application commands).
    /// </summary>
    ChannelMessage,

    /// <summary>
    ///     The followup will update the original message (components).
    /// </summary>
    MessageUpdate
}