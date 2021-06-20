using System;

namespace Disqord
{
    /// <summary>
    ///     Represents a local construct.
    /// </summary>
    public interface ILocalConstruct : ICloneable
    {
        /// <summary>
        ///     Checks whether this local entity is valid for interacting with the Discord API. 
        /// </summary>
        void Validate();
    }
}
