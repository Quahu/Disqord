using System;
using System.Threading.Tasks;
using Disqord.Serialization.Json;

namespace Disqord.Gateway.Default.Dispatcher
{
    public sealed class RuntimeHandler<TModel> : Handler<TModel, EventArgs>
        where TModel : JsonModel
    {
        private readonly Func<TModel, Task> _func;

        public RuntimeHandler(Func<TModel, Task> func)
        {
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            _func = func;
        }

        public override async Task<EventArgs> HandleDispatchAsync(TModel model)
        {
            await _func(model).ConfigureAwait(false);
            return null;
        }
    }
}
