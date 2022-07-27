// using System.Threading.Tasks;
// using Disqord.Gateway.Api;
// using Disqord.Models;
//
// namespace Disqord.Gateway.Default.Dispatcher
// {
//     public class ThreadMemberUpdateHandler : Handler<ThreadMemberJsonModel, ThreadMemberUpdatedEventArgs>
//     {
//         public override ValueTask<ThreadMemberUpdatedEventArgs?> HandleDispatchAsync(IGateway shard, ThreadMemberJsonModel model)
//         {
//             var e = new ThreadMemberUpdatedEventArgs(thread);
//             return new(e);
//         }
//     }
// }
