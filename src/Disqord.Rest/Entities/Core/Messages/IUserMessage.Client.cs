﻿using System;
using System.Threading.Tasks;

namespace Disqord
{
    public partial interface IUserMessage : IMessage
    {
        Task ModifyAsync(Action<ModifyMessageProperties> action, RestRequestOptions options = null);

        Task PinAsync(RestRequestOptions options = null);

        Task UnpinAsync(RestRequestOptions options = null);
    }
}
