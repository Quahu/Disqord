using System;

namespace Disqord.Utilities
{
    internal sealed class CachedProperty<T, TState>
    {
        public T Value
        {
            get
            {
                lock (this)
                {
                    if (!_hasValue)
                    {
                        _value = _factory(_state);
                        _hasValue = true;
                    }

                    return _value;
                }
            }
        }
        private T _value;

        private bool _hasValue;

        private readonly Func<TState, T> _factory;
        private readonly TState _state;

        public CachedProperty(Func<TState, T> factory, TState state)
        {
            _factory = factory;
            _state = state;
        }

        public void Reset()
        {
            _hasValue = false;
            _value = default;
        }
    }
}
