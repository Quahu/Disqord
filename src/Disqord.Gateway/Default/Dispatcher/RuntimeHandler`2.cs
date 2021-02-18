using System;
using System.Threading.Tasks;
using Disqord.Serialization.Json;

namespace Disqord.Gateway.Default.Dispatcher
{
    public sealed class RuntimeHandler<TModel, TEventArgs> : Handler<TModel, TEventArgs>
        where TModel : JsonModel
        where TEventArgs : EventArgs
    {
        private readonly Func<TModel, Task<TEventArgs>> _func;

        public RuntimeHandler(Func<TModel, Task<TEventArgs>> func)
        {
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            _func = func;
        }

        public override Task<TEventArgs> HandleDispatchAsync(TModel model)
            => _func(model);
    }
}
