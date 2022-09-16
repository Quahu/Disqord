using System.Collections.Generic;
using Disqord.Models;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Rest.Api;

public class CreateForumThreadJsonRestRequestContent : CreateThreadJsonRestRequestContent, IAttachmentRestRequestContent
{
    [JsonProperty("message")]
    public CreateMessageJsonRestRequestContent Message = null!;

    [JsonProperty("applied_tags")]
    public Optional<Snowflake[]> AppliedTags;

    IList<PartialAttachmentJsonModel> IAttachmentRestRequestContent.Attachments
    {
        set
        {
            Guard.IsNotNull(Message);

            Message.Attachments = new(value);
        }
    }
}
