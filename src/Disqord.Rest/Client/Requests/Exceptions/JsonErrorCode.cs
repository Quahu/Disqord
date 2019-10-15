namespace Disqord.Rest
{
    public enum JsonErrorCode
    {
        UnknownAccount = 10001,

        UnknownApplication = 10002,

        UnknownChannel = 10003,

        UnknownGuild = 10004,

        UnknownIntegration = 10005,

        UnknownInvite = 10006,

        UnknownMember = 10007,

        UnknownMessage = 10008,

        UnknownOverwrite = 10009,

        UnknownProvider = 10010,

        UnknownRole = 10011,

        UnknownToken = 10012,

        UnknownUser = 10013,

        UnknownEmoji = 10014,

        UnknownWebhook = 10015,

        UserOnlyEndpoint = 20001,

        BotOnlyEndpoint = 20002,

        MaximumGuildAmount = 30001,

        MaximumFriendAmount = 30002,

        MaximumPinAmount = 30003,

        MaximumRoleAmount = 30005,

        MaximumReactionAmount = 30010,

        MaximumGuildChannelAmount = 30013,

        Unauthorized = 40001,

        MissingAccess = 50001,

        InvalidAccountType = 50002,

        InvalidDmChannelAction = 50003,

        WidgetDisabled = 50004,

        CannotEditMessagesAuthoredByAnotherUser = 50005,

        CannotSendAnEmptyMessage = 50006,

        CannotSendMessagesToThisUser = 50007,

        CannotSendMessagesInAVoiceChannel = 50008,

        ChannelVerificationLevelIsTooHigh = 50009,

        OAuth2ApplicationdDesNotHaveABot = 50010,

        OAuth2ApplicationLimitRseached = 50011,

        InvalidOAuthState = 50012,

        MissingPermissions = 50013,

        InvalidAuthenticationToken = 50014,

        NoteIsTooLong = 50015,

        ProvidedTooFewOrTooManyMessagesToDeleteMustProvideAtLeast2AndFewerThan100MessagesToDelete = 50016,

        AMessageCanOnlyBePinnedToTheChannelItWasSent= 50019,

        InviteCodeIsEitherInvalidOrTaken = 50020,

        CannotExecuteActionOnASystemMessage = 50021,

        InvalidOAuth2AccessToken = 50025,

        AMessageProvidedWasTooOldToBulkDelete = 50034,

        InvalidFormBody = 50035,

        AnInviteWasAcceptedToAGuildTheApplicationsBotIsNot= 50036,

        InvalidApiVersion = 50041,

        ReactionBlocked = 90001
    }
}
