using System;
using System.Threading.Tasks;
using Disqord.Serialization.Json;

namespace Disqord.Gateway.Default.Dispatcher
{
    public sealed class RuntimeHandler : Handler<JsonModel, EventArgs>
    {
        private readonly Func<JsonModel, Task> _func;

        public RuntimeHandler(Func<JsonModel, Task> func)
        {
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            _func = func;
        }

        public override async Task<EventArgs> HandleDispatchAsync(JsonModel model)
        {
            await _func(model).ConfigureAwait(false);
            return null;
        }
    }
}
