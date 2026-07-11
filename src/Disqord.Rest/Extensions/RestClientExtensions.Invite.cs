using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Rest.Api;
using Qommon;

namespace Disqord.Rest;

public static partial class RestClientExtensions
{
    public static async Task<IInvite> FetchInviteAsync(this IRestClient client,
        string code, bool? withCounts = null, bool? withExpiration = null, Snowflake? eventId = null,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var model = await client.ApiClient.FetchInviteAsync(code, withCounts, withExpiration, eventId, options, cancellationToken).ConfigureAwait(false);
        return TransientInvite.Create(client, model);
    }

    public static Task DeleteInviteAsync(this IRestClient client,
        string code,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        return client.ApiClient.DeleteInviteAsync(code, options, cancellationToken);
    }

    public static async Task<IReadOnlyList<Snowflake>> FetchInviteTargetUsersAsync(this IRestClient client,
        string code,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var csv = await client.ApiClient.FetchInviteTargetUsersAsync(code, options, cancellationToken).ConfigureAwait(false);
        return ParseTargetUserIds(csv);
    }

    public static Task UpdateInviteTargetUsersAsync(this IRestClient client,
        string code, IEnumerable<Snowflake> targetUsers,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(targetUsers);

        var content = new UpdateInviteTargetUsersMultipartRestRequestContent
        {
            TargetUsersFile = CreateTargetUsersFile(targetUsers)
        };

        return client.ApiClient.UpdateInviteTargetUsersAsync(code, content, options, cancellationToken);
    }

    public static async Task<IInviteTargetUsersJobStatus> FetchInviteTargetUsersJobStatusAsync(this IRestClient client,
        string code,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var model = await client.ApiClient.FetchInviteTargetUsersJobStatusAsync(code, options, cancellationToken).ConfigureAwait(false);
        return new TransientInviteTargetUsersJobStatus(model);
    }

    private static Stream CreateTargetUsersFile(IEnumerable<Snowflake> targetUsers)
    {
        var builder = new StringBuilder("user_id\n");
        foreach (var userId in targetUsers)
        {
            builder.Append(userId.RawValue).Append('\n');
        }

        return new MemoryStream(Encoding.UTF8.GetBytes(builder.ToString()));
    }

    private static IReadOnlyList<Snowflake> ParseTargetUserIds(string csv)
    {
        if (string.IsNullOrWhiteSpace(csv))
        {
            return Array.Empty<Snowflake>();
        }

        var lines = csv.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        var userIds = new List<Snowflake>(lines.Length);
        foreach (var line in lines)
        {
            if (line.Equals("user_id", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            if (ulong.TryParse(line, out var userId))
            {
                userIds.Add(new Snowflake(userId));
            }
        }

        return userIds;
    }
}
