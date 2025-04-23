namespace Disqord;

public static partial class Discord
{
    public static partial class Limits
    {
        /// <summary>
        ///     Represents limits within Discord's REST API.
        /// </summary>
        public static class Rest
        {
            /// <summary>
            ///     Represents the maximum length of the <c>X-Audit-Log-Reason</c> header; used for sending audit log reasons.
            /// </summary>
            public const int MaxAuditLogReasonLength = 512;

            /// <summary>
            ///     Represents the page size for fetching audit logs.
            /// </summary>
            public const int FetchAuditLogsPageSize = 100;

            /// <summary>
            ///     Represents the page size for fetching messages.
            /// </summary>
            public const int FetchMessagesPageSize = 100;

            /// <summary>
            ///     Represents the page size for deleting messages.
            /// </summary>
            public const int DeleteMessagesPageSize = 100;

            /// <summary>
            ///     Represents the page size for fetching reactions.
            /// </summary>
            public const int FetchReactionsPageSize = 100;

            /// <summary>
            ///     Represents the page size for fetching members.
            /// </summary>
            public const int FetchMembersPageSize = 1000;

            /// <summary>
            ///     Represents the page size for fetching bans.
            /// </summary>
            public const int FetchBansPageSize = 1000;

            /// <summary>
            ///     Represents the page size for fetching guilds.
            /// </summary>
            public const int FetchGuildsPageSize = 200;

            /// <summary>
            ///     Represents the page size for fetching thread members.
            /// </summary>
            public const int FetchThreadMembersPageSize = 100;

            /// <summary>
            ///     Represents the page size for fetching threads.
            /// </summary>
            public const int FetchThreadsPageSize = 100;

            /// <summary>
            ///     Represents the page size for fetching event users.
            /// </summary>
            public const int FetchGuildEventUsersPageSize = 100;

            /// <summary>
            ///     Represents the page size for fetching entitlements.
            /// </summary>
            public const int FetchEntitlementsPageSize = 100;

            /// <summary>
            ///     Represents the page size for fetching poll answer voters.
            /// </summary>
            public const int FetchPollAnswerVotersPageSize = 100;
        }
    }
}
