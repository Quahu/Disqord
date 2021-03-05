using System;
using System.Threading.Tasks;

namespace Disqord.Utilities
{
    public static class RuntimeDisposal
    {
        public static ValueTask DisposeAsync(object instance, bool disposeBoth = false)
        {
            if (instance is IAsyncDisposable asyncDisposable)
            {
                return asyncDisposable.DisposeAsync();
            }

            if (instance is IDisposable disposable && (disposeBoth || instance is not IAsyncDisposable))
            {
                disposable.Dispose();
            }

            return default;
        }

        public static RuntimeAsyncDisposable WrapAsync(object instance, bool disposeBoth = false)
            => new(instance, disposeBoth);

        public static RuntimeDisposable Wrap(object instance)
            => new(instance);

        public readonly struct RuntimeAsyncDisposable : IAsyncDisposable
        {
            private readonly object _instance;
            private readonly bool _disposeBoth;

            public RuntimeAsyncDisposable(object instance, bool disposeBoth)
            {
                _instance = instance;
                _disposeBoth = disposeBoth;
            }

            public ValueTask DisposeAsync()
            {
                if (_instance is IAsyncDisposable asyncDisposable)
                {
                    return asyncDisposable.DisposeAsync();
                }

                if (_instance is IDisposable disposable && (_disposeBoth || _instance is not IAsyncDisposable))
                {
                    disposable.Dispose();
                }

                return default;
            }
        }

        public readonly struct RuntimeDisposable : IDisposable
        {
            private readonly object _instance;

            public RuntimeDisposable(object instance)
            {
                _instance = instance;
            }

            public void Dispose()
            {
                if (_instance is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
        }
    }
}
