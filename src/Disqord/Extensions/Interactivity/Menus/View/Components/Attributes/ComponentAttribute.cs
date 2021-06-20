using System;

namespace Disqord.Extensions.Interactivity.Menus
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
    public abstract class ComponentAttribute : Attribute
    {
        /// <summary>
        ///     Gets or sets the component row the component should appear on.
        ///     Defaults to <c>-1</c> for automatic positioning.
        /// </summary>
        public int Row { get; init; } = -1;

        /// <summary>
        ///     Gets or sets the position the component should appear on in the component row.
        ///     Defaults to <c>-1</c> for automatic positioning.
        /// </summary>
        public int Position { get; init; } = -1;

        protected ComponentAttribute()
        { }
    }
}
