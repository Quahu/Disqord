using System;
using Qmmands;

namespace Disqord.Bot;

/// <summary>
///     Marks the given method as a callback called by <see cref="DiscordBotBase.MutateModule"/>.
/// </summary>
/// <remarks>
///     The method must be public, static, and have the following parameters,
///     in order: <see cref="DiscordBotBase"/>, <see cref="IModuleBuilder"/>.
/// </remarks>
[AttributeUsage(AttributeTargets.Method)]
public class MutateModuleAttribute : Attribute
{
    /// <summary>
    ///     Instantiates anew <see cref="MutateModuleAttribute"/>.
    /// </summary>
    public MutateModuleAttribute()
    { }
}
