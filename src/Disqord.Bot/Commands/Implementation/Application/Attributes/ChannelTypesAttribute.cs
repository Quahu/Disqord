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
    public ChannelType[] ChannelTypes { get; }

    public ChannelTypesAttribute(params ChannelType[] channelTypes)
    {
        ChannelTypes = channelTypes;
    }
}
