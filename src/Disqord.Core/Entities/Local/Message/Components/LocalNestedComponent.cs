namespace Disqord
{
    public abstract class LocalNestedComponent : LocalComponent
    {
        /// <summary>
        ///     Gets or sets whether this interactive component is disabled.
        /// </summary>
        // TODO: this might have to move ☜(ﾟヮﾟ☜)
        public bool IsDisabled { get; set; }

        protected LocalNestedComponent()
        { }

        protected LocalNestedComponent(LocalNestedComponent other)
        {
            IsDisabled = other.IsDisabled;
        }
    }
}
