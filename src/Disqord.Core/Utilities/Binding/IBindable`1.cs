namespace Disqord.Utilities.Binding
{
    /// <summary>
    ///     Represents a type that can be bound to another type.
    ///     This exists purely for circular dependencies in dependency-injectable Disqord components.
    /// </summary>
    /// <typeparam name="TBind"> The type to bind to. </typeparam>
    public interface IBindable<TBind>
    {
        /// <summary>
        ///     Binds this <see cref="IBindable{TBind}"/> to the specified value.
        /// </summary>
        /// <param name="value"> The value to bind to. </param>
        void Bind(TBind value);
    }
}
