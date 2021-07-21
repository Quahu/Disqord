﻿namespace Disqord.Rest
{
    public enum RestApiErrorCode
    {
        Unknown = 0,

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

        UnknownBan = 10026,

        UnknownSku = 10027,

        UnknownStoreListing = 10028,

        UnknownEntitlement = 10029,

        UnknownBuild = 10030,

        UnknownLobby = 10031,

        UnknownBranch = 10032,

        UnknownRedistributable = 10036,

        UnknownGuildTemplate = 10057,

        UnknownDiscoverableCategory = 10059,

        UnknownSticker = 10060,

        UnknownApplicationCommand = 10063,

        UnknownStage = 10067,

        UnknownMemberVerificationForm = 10068,

        UnknownWelcomeScreen = 10069,

        UserOnlyEndpoint = 20001,

        BotOnlyEndpoint = 20002,

        AnnouncementRateLimit = 20022,

        ChannelWriteRateLimit = 20028,

        /// <summary>
        ///     The stage topic, guild name, guild description, or channel names contain words that are not allowed.
        /// </summary>
        DisallowedStageWords = 20031,

        MaximumGuildAmountReached = 30001,

        MaximumFriendAmountReached = 30002,

        MaximumPinAmountReached = 30003,

        MaximumRoleAmountReached = 30005,

        MaximumWebhookAmountReached = 30007,

        MaximumReactionAmountReached = 30010,

        MaximumGuildChannelAmountReached = 30013,

        MaximumAttachmentAmountReached = 30015,

        MaximumInviteAmountReached = 30016,

        MaximumAnimatedEmojiAmountReached = 30018,

        MaximumMemberAmountReached = 30019,

        MaximumDiscoverableCategoryAmountReached = 30030,

        GuildAlreadyHasATemplate = 30031,

        MaximumThreadParticipantAmountReached = 30033,

        MaximumNonMemberBanAmountReached = 30035,

        BanFetchRateLimit = 30037,

        MaximumStickerAmountReached = 30039,

        Unauthorized = 40001,

        FeatureTemporarilyDisabled = 40006,

        UserBanned = 40007,

        MessageAlreadyCrossposted = 40033,

        MissingAccess = 50001,

        InvalidAccountType = 50002,

        InvalidDirectChannelAction = 50003,

        WidgetDisabled = 50004,

        CannotEditMessagesAuthoredByAnotherUser = 50005,

        CannotSendAnEmptyMessage = 50006,

        CannotSendMessagesToThisUser = 50007,

        CannotSendMessagesInAVoiceChannel = 50008,

        ChannelVerificationLevelIsTooHigh = 50009,

        OAuth2ApplicationdDoesNotHaveABot = 50010,

        OAuth2ApplicationLimitReached = 50011,

        InvalidOAuth2State = 50012,

        MissingPermissions = 50013,

        InvalidAuthenticationToken = 50014,

        NoteTooLong = 50015,

        ProvidedTooFewOrTooManyMessagesToDeleteMustProvideAtLeast2AndFewerThan100MessagesToDelete = 50016,

        AMessageCanOnlyBePinnedToTheChannelItWasSent = 50019,

        InviteCodeIsEitherInvalidOrTaken = 50020,

        CannotExecuteActionOnASystemMessage = 50021,

        CannotExecuteActionOnThisChannelType = 50024,

        InvalidOAuth2AccessToken = 50025,

        MissingOAuth2Scope = 50026,

        InvalidWebhookToken = 50027,

        InvalidRole = 50028,

        AMessageProvidedWasTooOldToBulkDelete = 50034,

        InvalidFormBody = 50035,

        AnInviteWasAcceptedToAGuildTheApplicationsBotIsNotIn = 50036,

        InvalidApiVersion = 50041,

        MaximumFileSizeExceeded = 50045,

        InvalidFileType = 50046,

        CannotDeleteAChannelRequiredForCommunityGuilds = 50074,

        InvalidStickerSent = 50081,

        ThreadArchived = 50083,

        InvalidThreadNotifications = 50084,

        /// <summary>
        ///     <c>before</c> value is earlier than the thread creation date.
        /// </summary>
        ValueBeforeThreadCreation = 50085,

        TwoFactorRequired = 60003,

        IncomingFriendRequestsDisabled = 80000,

        FriendRequestBlocked = 80001,

        BotsCannotHaveFriends = 80002,

        CannotSendFriendRequestToSelf = 80003,

        NoUsersWithDiscordTagExist = 80004,

        ReactionBlocked = 90001,

        IndexNotYetAvailable = 110000,

        ResourceOverloaded = 130000,

        StageAlreadyOpen = 150006,

        ThreadAlreadyCreated = 160004,

        ThreadLocked = 160005,

        MaximumActiveThreadAmountReached = 160006,

        MaximumActiveAnnouncementThreadAmountReached = 160007,

        InvalidLottieFile = 170001,

        LottieCannotContainRasterizedImages = 170002,

        MaximumStickerFrameRateExceeded = 170003,

        MaximumStickerFrameCountExceeded = 170004,

        MaximumLottieDimensionsExceeded = 170005,

        InvalidStickerFrameRate = 170006,

        MaximumLottieDurationExceeded = 170007
    }
}
