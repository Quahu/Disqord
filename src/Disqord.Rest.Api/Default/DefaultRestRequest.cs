using System;
using System.Threading.Tasks;
using Disqord.Http;

namespace Disqord.Rest.Api.Default
{
    /// <inheritdoc/>
    public class DefaultRestRequest : IRestRequest
    {
        /// <inheritdoc/>
        public FormattedRoute Route { get; }

        /// <inheritdoc/>
        public IRestRequestContent Content { get; }

        /// <inheritdoc/>
        public IRestRequestOptions Options { get; }

        protected HttpRequestContent _httpContent;

        private readonly TaskCompletionSource<IRestResponse> _tcs;

        public DefaultRestRequest(FormattedRoute route, IRestRequestContent content, IRestRequestOptions options = null)
        {
            if (route == null)
                throw new ArgumentNullException(nameof(route));

            Route = route;
            Content = content;
            Options = options;

            _tcs = new TaskCompletionSource<IRestResponse>(TaskCreationOptions.RunContinuationsAsynchronously);
        }

        public virtual HttpRequestContent GetOrCreateHttpContent(IRestApiClient client)
        {
            if (_httpContent == null && Content != null)
                _httpContent = Content.CreateHttpContent(client, Options);

            return _httpContent;
        }

        /// <inheritdoc/>
        public Task<IRestResponse> WaitAsync()
            => _tcs.Task;

        /// <inheritdoc/>
        public void Complete(IRestResponse response)
        {
            if (response == null)
                throw new ArgumentNullException(nameof(response));

            _tcs.SetResult(response);
        }

        /// <inheritdoc/>
        public void Complete(Exception exception)
        {
            if (exception == null)
                throw new ArgumentNullException(nameof(exception));

            _tcs.SetException(exception);
        }

        /// <inheritdoc/>
        public virtual void Dispose()
        {
            _httpContent?.Dispose();
        }
    }
}
