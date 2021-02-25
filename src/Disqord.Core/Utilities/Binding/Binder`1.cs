using System;
using System.Diagnostics.CodeAnalysis;

namespace Disqord.Utilities.Binding
{
    /// <summary>
    ///     Represents a helper type that wraps types implementing <see cref="IBindable{TBind}"/>.
    /// </summary>
    /// <typeparam name="TBind"> The type to bind to. </typeparam>
    public class Binder<TBind>
    {
        /// <summary>
        ///     Gets the bound value of this <see cref="Binder{TBind}"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     Thrown if the instance is not bound.
        /// </exception>
        public TBind Value
        {
            get
            {
                if (_value == null)
                    throw new InvalidOperationException($"This {_bindable.GetType().Name} was never bound to an instance of '{typeof(TBind).Name}'.");

                return _value;
            }
        }

        /// <summary>
        ///     Gets whether this <see cref="Binder{TBind}"/> has a value.
        /// </summary>
        public bool IsBound => _value != null;

        private TBind? _value;
        private readonly IBindable<TBind> _bindable;
        private readonly Action<TBind>? _check;
        private readonly bool _allowRebinding;

        /// <summary>
        ///     Instantiates a new binder for the provided <see cref="IBindable{TBind}"/>.
        /// </summary>
        /// <param name="bindable"> The bindable to wrap. </param>
        /// <param name="check"> The optional check used for throwing exceptions. </param>
        /// <param name="allowRebinding"> Whether rebinding should be allowed. </param>
        public Binder(IBindable<TBind> bindable, Action<TBind>? check = null, bool allowRebinding = false)
        {
            if (bindable == null)
                throw new ArgumentNullException(nameof(bindable));

            _bindable = bindable;
            _check = check;
            _allowRebinding = allowRebinding;
        }

        /// <summary>
        ///     Binds the underlying <see cref="IBindable{TBind}"/> to the specified value.
        /// </summary>
        /// <param name="value"> The value to bind to. </param>
        public void Bind(TBind value)
        {
            if (value == null)
                throw new ArgumentNullException($"Cannot bind this '{_bindable.GetType().Name}' to a null instance of '{typeof(TBind).Name}'.");

            if (_value != null && !_allowRebinding)
                throw new InvalidOperationException($"This '{_bindable.GetType().Name}' is already bound to an instance of '{_value.GetType().Name}'.");

            _check?.Invoke(value);
            _value = value;
        }

        public bool TryGetValue([MaybeNullWhen(false)] out TBind value)
        {
            value = _value;
            return _value != null;
        }

        /// <summary>
        ///     Gets the bound value of this <see cref="Binder{TBind}"/> or the <see langword="default"/> value.
        /// </summary>
        public TBind? GetValueOrDefault()
            => _value;
    }
}
