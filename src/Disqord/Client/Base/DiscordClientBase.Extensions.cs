using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Disqord.Collections;
using Disqord.Rest;

namespace Disqord
{
    public abstract partial class DiscordClientBase : IRestDiscordClient, IAsyncDisposable
    {
        internal readonly LockedDictionary<Type, DiscordClientExtension> _extensions;

        public ValueTask AddExtensionAsync<T>(T extension) where T : DiscordClientExtension
        {
            ThrowIfDisposed();

            if (extension == null)
                throw new ArgumentNullException(nameof(extension));

            var type = typeof(T);
            if (!_extensions.TryAdd(type, extension))
                throw new ArgumentException($"The {type} extension has already been added to this client.", nameof(extension));

            extension.Client = this;
            return extension.InitialiseAsync();
        }

        public ValueTask RemoveExtensionAsync<T>() where T : DiscordClientExtension
        {
            ThrowIfDisposed();

            var type = typeof(T);
            if (!_extensions.TryRemove(type, out var extension))
                throw new ArgumentException($"The {type} extension has not been added to this client.");

            return extension.DisposeAsync();
        }

        public T GetExtension<T>() where T : DiscordClientExtension
        {
            ThrowIfDisposed();

            return _extensions.GetValueOrDefault(typeof(T)) as T;
        }
    }
}
