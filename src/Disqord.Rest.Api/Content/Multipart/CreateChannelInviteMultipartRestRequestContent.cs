using System.IO;
using Disqord.Http;
using Disqord.Serialization.Json;

namespace Disqord.Rest.Api;

public class CreateChannelInviteMultipartRestRequestContent : MultipartRestRequestContent
{
    public CreateChannelInviteJsonRestRequestContent Payload = null!;

    public Stream TargetUsersFile = null!;

    /// <inheritdoc/>
    public override HttpRequestContent CreateHttpContent(IJsonSerializer serializer, IRestRequestOptions? options = null)
    {
        var content = Add(TargetUsersFile, "target_users_file", "users.csv");
        content.Headers["Content-Type"] = "text/csv";
        FormData.Insert(0, (Payload.CreateHttpContent(serializer, options), "payload_json", null));
        return base.CreateHttpContent(serializer, options);
    }
}
