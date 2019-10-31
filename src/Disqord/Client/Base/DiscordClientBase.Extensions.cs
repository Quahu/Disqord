using System;
using System.Collections.Generic;

namespace Disqord
{
    public abstract partial class DiscordClientBase : IRestDiscordClient, IAsyncDisposable
    {
        internal Dictionary<Type, DiscordClientExtension> Extensions;

        public T GetExtension<T>() where T : DiscordClientExtension
            => (T) Extensions.GetValueOrDefault(typeof(T));
    }
}
