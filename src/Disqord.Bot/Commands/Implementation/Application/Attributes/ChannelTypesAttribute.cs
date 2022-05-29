using System;

namespace Disqord.Bot.Commands.Application;

/// <summary>
///     <b>Do not use this for text commands!</b>
///     <para/>
///     Restricts the <see cref="IChannel"/> <b>slash command option</b> to given channel types.
/// </summary>
/// <remarks>
///     For slash commands this attribute is turned into API-side validation.
///     <br/>
///     For text commands this attribute does nothing because text commands
///     have the channel type matched against the type of the parameter.
/// </remarks>
[AttributeUsage(AttributeTargets.Parameter)]
public class ChannelTypesAttribute : Attribute
{
    /// <summary>
    ///     Gets the channel types of this attribute.
    /// </summary>
    public ChannelType[] ChannelTypes { get; }

    /// <summary>
    ///     Instantiates a new <see cref="ChannelTypesAttribute"/> with the specified channel types.
    /// </summary>
    /// <param name="channelTypes"> The channel types to restrict the option to. </param>
    public ChannelTypesAttribute(params ChannelType[] channelTypes)
    {
        ChannelTypes = channelTypes;
    }
}
