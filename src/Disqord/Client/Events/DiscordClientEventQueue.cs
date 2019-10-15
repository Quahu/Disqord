//using System;
//using System.Collections.Concurrent;
//using System.Threading.Tasks;
//using Disqord.Logging;

//namespace Disqord.Events
//{
//    // TODO: ???
//    internal sealed class DiscordClientEventQueue
//    {
//        private const int HANDLER_TIMEOUT_SECONDS = 5;
//        private static readonly TimeSpan _timeout = TimeSpan.FromSeconds(HANDLER_TIMEOUT_SECONDS);
//        private readonly ConcurrentQueue<(GatewayDispatch Dispatch, Func<Task> Func)> _queue = new ConcurrentQueue<(GatewayDispatch Dispatch, Func<Task> Func)>();
//        private readonly object _lock = new object();
//        private readonly DiscordClientBase _client;
//        private Task _runTask;

//        public DiscordClientEventQueue(DiscordClientBase client)
//        {
//            _client = client;
//        }

//        public void Enqueue((GatewayDispatch, Func<Task>) tuple)
//        {
//            _queue.Enqueue(tuple);
//            lock (_lock)
//            {
//                if (_runTask == null || _runTask.IsCompleted)
//                    _runTask = Task.Run(RunAsync);
//            }
//        }

//        public void Clear()
//            => _queue.Clear();

//        private async Task RunAsync()
//        {
//            while (_queue.TryDequeue(out var tuple))
//            {
//                var delayTask = Task.Delay(_timeout);
//                var funcTask = tuple.Func();
//                var anyTask = await Task.WhenAny(delayTask, funcTask).ConfigureAwait(false);
//                if (anyTask == delayTask)
//                {
//                    await _client.LogAsync(LogLevel.Warning,
//                        $"One of the {tuple.Dispatch} handlers is taking over {HANDLER_TIMEOUT_SECONDS} seconds to execute and is blocking other event handlers from executing.");
//                }
//                try
//                {
//                    await anyTask.ConfigureAwait(false);
//                }
//                catch { }
//            }
//        }
//    }
//}
