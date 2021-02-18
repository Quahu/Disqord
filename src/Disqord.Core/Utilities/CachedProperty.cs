using System;

namespace Disqord.Utilities
{
    internal static class CachedProperty
    {
        public static CachedProperty<T, TState> Create<T, TState>(Func<TState, T> factory, TState state) where T : class
            => new CachedProperty<T, TState>(factory, state);
    }
}
