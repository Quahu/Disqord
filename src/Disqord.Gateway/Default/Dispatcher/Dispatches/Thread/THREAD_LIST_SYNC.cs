using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;
using Disqord.Models;
using Qommon.Collections.Synchronized;

namespace Disqord.Gateway.Default.Dispatcher;

public class ThreadListSyncDispatchHandler : DispatchHandler<ThreadListSyncJsonModel, ThreadsSynchronizedEventArgs>
{
    public override ValueTask<ThreadsSynchronizedEventArgs?> HandleDispatchAsync(IShard shard, ThreadListSyncJsonModel model)
    {
        var threadModelDictionary = new Dictionary<Snowflake, List<ChannelJsonModel>>();
        if (model.ChannelIds.HasValue)
            threadModelDictionary = model.ChannelIds.Value.ToDictionary(x => x, _ => new List<ChannelJsonModel>());

        foreach (var threadModel in model.Threads)
        {
            var memberModel = Array.Find(model.Members, x => x.Id == threadModel.Id);
            if (memberModel != null)
                threadModel.Member = memberModel;

            if (!model.ChannelIds.HasValue || !threadModelDictionary.TryGetValue(threadModel.ParentId.Value!.Value, out var threadModels))
                threadModelDictionary.Add(threadModel.ParentId.Value!.Value, threadModels = new List<ChannelJsonModel>());

            threadModels.Add(threadModel);
        }

        var threads = threadModelDictionary.ToDictionary(x => x.Key, x => new List<IThreadChannel>(x.Value.Count) as IReadOnlyList<IThreadChannel>);
        var uncachedThreads = new Dictionary<Snowflake, IReadOnlyList<CachedThreadChannel>>();

        static void UncacheThread(ISynchronizedDictionary<Snowflake, CachedGuildChannel> channelCache, Dictionary<Snowflake, IReadOnlyList<CachedThreadChannel>> uncachedThreads, KeyValuePair<Snowflake, List<ChannelJsonModel>> kvp, CachedThreadChannel cachedThreadChannel)
        {
            channelCache.Remove(cachedThreadChannel.Id);
            List<CachedThreadChannel> list;
            if (uncachedThreads.TryGetValue(kvp.Key, out var boxedList))
                list = (boxedList as List<CachedThreadChannel>)!;
            else
                uncachedThreads.Add(kvp.Key, list = new List<CachedThreadChannel>());

            list.Add(cachedThreadChannel);
        }

        if (CacheProvider.TryGetChannels(model.GuildId, out var channelCache))
        {
            foreach (var kvp in threadModelDictionary)
            {
                if (kvp.Value.Count == 0)
                {
                    foreach (var cachedChannel in channelCache.Values)
                    {
                        if (cachedChannel is CachedThreadChannel cachedThreadChannel && cachedThreadChannel.ChannelId == kvp.Key)
                            UncacheThread(channelCache, uncachedThreads, kvp, cachedThreadChannel);
                    }
                }
                else
                {
                    foreach (var cachedChannel in channelCache.Values)
                    {
                        if (cachedChannel is CachedThreadChannel cachedThreadChannel && cachedThreadChannel.ChannelId == kvp.Key && !kvp.Value.Any(x => x.Id == cachedThreadChannel.Id))
                            UncacheThread(channelCache, uncachedThreads, kvp, cachedThreadChannel);
                    }

                    var list = (threads[kvp.Key] as List<IThreadChannel>)!;
                    foreach (var threadModel in kvp.Value)
                    {
                        if (!channelCache.TryGetValue(threadModel.Id, out var cachedChannel))
                        {
                            cachedChannel = new CachedThreadChannel(Client, threadModel);
                            channelCache.Add(threadModel.Id, cachedChannel);
                        }
                        else
                        {
                            cachedChannel.Update(threadModel);
                        }

                        list.Add((cachedChannel as IThreadChannel)!);
                    }
                }
            }
        }
        else
        {
            foreach (var kvp in threadModelDictionary)
            {
                if (threads.TryGetValue(kvp.Key, out var boxedList))
                {
                    var list = (boxedList as List<IThreadChannel>)!;
                    foreach (var threadModel in kvp.Value)
                        list.Add(new TransientThreadChannel(Client, threadModel));
                }
            }
        }

        var e = new ThreadsSynchronizedEventArgs(model.GuildId, threads, uncachedThreads);
        return new(e);
    }
}
