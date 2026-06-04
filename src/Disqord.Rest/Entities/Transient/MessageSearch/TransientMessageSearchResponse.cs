using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Disqord.Models;
using Disqord.Serialization.Json;

namespace Disqord.Rest;

public class TransientMessageSearchResponse(IClient client, MessageSearchResponseJsonModel model) : TransientClientEntity<MessageSearchResponseJsonModel>(client, model), IMessageSearchResponse
{
    /// <inheritdoc/>
    public int TotalResultCount => Model.TotalResults;

    /// <inheritdoc/>
    public IReadOnlyCollection<IMessageSearchFoundMessage> FoundMessages
    {
        get
        {
            if (_foundMessages != null)
            {
                return _foundMessages;
            }

            var foundMessages = new List<IMessageSearchFoundMessage>();
            var foundMessageIds = new HashSet<Snowflake>();
            var messagesById = new Dictionary<Snowflake, IMessage>();
            foreach (var group in Model.Messages)
            {
                var messagesWithContext = new List<IMessage>(group.Length);
                var hitMessages = new List<IMessage>();
                foreach (var messageModel in group)
                {
                    if (!messagesById.TryGetValue(messageModel.Id, out var message))
                    {
                        message = TransientMessage.Create(Client, messageModel);
                        messagesById.Add(message.Id, message);
                    }

                    messagesWithContext.Add(message);

                    if (JsonModel.TryGetExtensionDatum<bool>(messageModel, "hit", out var hit) && hit && foundMessageIds.Add(message.Id))
                    {
                        hitMessages.Add(message);
                    }
                }

                foreach (var hitMessage in hitMessages)
                {
                    foundMessages.Add(new MessageSearchFoundMessage(hitMessage, messagesWithContext));
                }
            }

            _foundMessages = foundMessages;
            return _foundMessages;
        }
    }
    private List<IMessageSearchFoundMessage>? _foundMessages;

    /// <inheritdoc/>
    [field: MaybeNull]
    public IReadOnlyCollection<IChannel> Threads
    {
        get
        {
            if (field != null)
            {
                return field;
            }

            if (Model.Threads.HasValue && Model.Threads.Value is { } threadModels)
            {
                var threads = new List<IChannel>(threadModels.Length);
                foreach (var threadModel in threadModels)
                {
                    var channel = TransientChannel.Create(Client, threadModel);
                    if (channel != null)
                    {
                        threads.Add(channel);
                    }
                }

                field = threads;
            }
            else
            {
                field = [];
            }

            return field;
        }
    }

    /// <inheritdoc/>
    public bool IsDoingDeepHistoricalIndex => Model.DoingDeepHistoricalIndex;

    /// <inheritdoc/>
    public int? DocumentsIndexed => Model.DocumentsIndexed.HasValue ? Model.DocumentsIndexed.Value : null;
}
