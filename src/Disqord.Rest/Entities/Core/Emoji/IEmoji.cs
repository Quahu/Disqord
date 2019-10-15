using System;

namespace Disqord
{
    public interface IEmoji : IEquatable<IEmoji>
    {
        string Name { get; }

        string ReactionFormat { get; }

        string MessageFormat { get; }
    }
}
