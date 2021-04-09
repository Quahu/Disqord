using System;
using System.Threading.Tasks;

namespace Disqord.Utilities
{
    /// <summary>
    ///     Represents utilities for disposing types implementing <see cref="IAsyncDisposable"/> and <see cref="IDisposable"/>
    ///     that are not known at the time of compilation.
    /// </summary>
    public static class RuntimeDisposal
    {
        /// <summary>
        ///     Asynchronously disposes of the specified instance by calling either
        ///     <see cref="IAsyncDisposable.DisposeAsync"/> or <see cref="IDisposable.Dispose"/>,
        ///     or both if <paramref name="disposeBoth"/> is set to <see langword="true"/>.
        /// </summary>
        /// <param name="instance"> The instance to dispose. </param>
        /// <param name="disposeBoth">
        ///     Whether to call both <see cref="IAsyncDisposable.DisposeAsync"/> 
        ///     and <see cref="IDisposable.Dispose"/> or only one of them.
        /// </param>
        /// <returns>
        ///     The <see cref="ValueTask"/> representing the disposal work.
        /// </returns>
        public static async ValueTask DisposeAsync(object instance, bool disposeBoth = false)
        {
            if (instance is IAsyncDisposable asyncDisposable)
                await asyncDisposable.DisposeAsync().ConfigureAwait(false);

            if (instance is IDisposable disposable && (disposeBoth || instance is not IAsyncDisposable))
                disposable.Dispose();
        }

        /// <summary>
        ///     Wraps the specified instance in a <see cref="RuntimeAsyncDisposable"/>.
        /// </summary>
        /// <param name="instance"> The instance to wrap. </param>
        /// <param name="disposeBoth">
        ///     Whether the <see cref="RuntimeAsyncDisposable"/> should call both
        ///     <see cref="IAsyncDisposable.DisposeAsync"/> and <see cref="IDisposable.Dispose"/> or only one of them. 
        /// </param>
        /// <returns>
        ///     The <see cref="RuntimeAsyncDisposable"/> wrapping the instance.
        /// </returns>
        public static RuntimeAsyncDisposable WrapAsync(object instance, bool disposeBoth = false)
            => new(instance, disposeBoth);

        /// <summary>
        ///     Wraps the specified instance in a <see cref="RuntimeDisposable"/>.
        /// </summary>
        /// <remarks>
        ///     <see cref="WrapAsync(object, bool)"/> should be preferred, if the instance might be implementing <see cref="IAsyncDisposable"/>.
        /// </remarks>
        /// <param name="instance"> The instance to wrap. </param>
        /// <param name="disposeBoth">
        ///     Whether the <see cref="RuntimeDisposable"/> should call both
        ///     <see cref="IAsyncDisposable.DisposeAsync"/> and <see cref="IDisposable.Dispose"/> or only one of them. 
        /// </param>
        /// <returns>
        ///     The <see cref="RuntimeDisposable"/> wrapping the instance.
        /// </returns>
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
                => RuntimeDisposal.DisposeAsync(_instance, _disposeBoth);
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
                    disposable.Dispose();
            }
        }
    }
}
