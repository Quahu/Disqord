using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Disqord.Rest.Pagination;

namespace Disqord.Rest
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static partial class RestEntityExtensions
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IRestClient GetRestClient(this IEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (entity.Client is not IRestClient client)
                throw new InvalidOperationException("This entity's client is not a REST client implementation.");

            return client;
        }
    }
}
