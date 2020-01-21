using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Disqord.Logging;
using Disqord.Models;
using Disqord.Serialization.Json;

namespace Disqord.Rest
{
    public partial class RestDiscordClient : IRestDiscordClient
    {
        private LoginModel _lastLoginResponse;
        private DateTimeOffset _lastLoginAt;

        /// <summary>
        ///     Performs a user account login using the provided <paramref name="email"/> and <paramref name="password"/>.
        ///     This method can only be used with a <see cref="RestDiscordClient"/> that was created using <see cref="CreateWithoutAuthorization(ILogger, IJsonSerializer)"/>.
        ///     If MFA is enabled, you must specify the <see cref="RestRequestOptions.MfaCode"/>.
        ///     This method does not support captcha.
        /// </summary>
        /// <param name="email"> The email of the account. </param>
        /// <param name="password"> The password of the account. </param>
        /// <param name="options"> The optional <see cref="RestRequestOptions"/> to use. </param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task LoginAsync(string email, string password, RestRequestOptions options = null)
        {
            if (HasAuthorization)
                throw new InvalidOperationException("Login may only be used from a client without authorization.");

            ApiClient.SetTokenType(TokenType.User);
            string mfaCode = null;
            if (options != null)
            {
                mfaCode = options.MfaCode;
                options = new RestRequestOptionsBuilder()
                    .WithTimeout(options.Timeout)
                    .WithCancellationToken(options.CancellationToken)
                    .Build();
            }

            if (_lastLoginResponse == null || _lastLoginAt != default && (DateTimeOffset.UtcNow - _lastLoginAt).TotalMinutes > 1)
            {
                _lastLoginResponse = await ApiClient.LoginAsync(email, password, options).ConfigureAwait(false);
                _lastLoginAt = DateTimeOffset.UtcNow;
                if (_lastLoginResponse.Token != null)
                {
                    ApiClient.SetToken(_lastLoginResponse.Token);
                    return;
                }

                await Task.Delay(5000).ConfigureAwait(false);
            }

            await HandleTotpAsync(mfaCode, options).ConfigureAwait(false);
        }

        /// <summary>
        ///     Performs a user account logout after it was logged in using <see cref="LoginAsync(string, string, RestRequestOptions)"/>.
        /// </summary>
        /// <param name="options"> The optional <see cref="RestRequestOptions"/> to use. </param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task LogoutAsync(RestRequestOptions options = null)
        {
            if (_lastLoginResponse == null)
                throw new InvalidOperationException("Logout may only be used from a client that has been logged in.");

            await ApiClient.LogoutAsync(options).ConfigureAwait(false);
            ApiClient.ResetToken();
            _lastLoginResponse = null;
            _lastLoginAt = default;
        }

        private async Task HandleTotpAsync(string mfaCode, RestRequestOptions options)
        {
            if (mfaCode == null)
                throw new ArgumentException("A MFA code must be provided.");

            var totpResponse = await ApiClient.TotpAsync(_lastLoginResponse.Ticket, mfaCode, options).ConfigureAwait(false);
            ApiClient.SetToken(totpResponse.Token);
        }
    }
}
