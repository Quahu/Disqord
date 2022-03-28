namespace Disqord.Rest
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

        UnknownWebhookService = 10016,

        UnknownSession = 10020,

        UnknownBan = 10026,

        UnknownSku = 10027,

        UnknownStoreListing = 10028,

        UnknownEntitlement = 10029,

        UnknownBuild = 10030,

        UnknownLobby = 10031,

        UnknownBranch = 10032,

        UnknownStoreDirectoryLayout = 10033,

        UnknownRedistributable = 10036,

        UnknownGiftCode = 10038,

        UnknownStream = 10049,

        UnknownBoostCooldown = 10050,

        UnknownGuildTemplate = 10057,

        UnknownDiscoverableCategory = 10059,

        UnknownSticker = 10060,

        UnknownInteraction = 10062,

        UnknownApplicationCommand = 10063,

        UnknownVoiceState = 10065,

        UnknownApplicationCommandPermissions = 10066,

        UnknownStage = 10067,

        UnknownMemberVerificationForm = 10068,

        UnknownWelcomeScreen = 10069,

        UnknownGuildEvent = 10070,

        UnknownGuildEventUser = 10071,

        UserOnlyEndpoint = 20001,

        BotOnlyEndpoint = 20002,

        ExplicitContentCannotBeSentToRecipient = 20009,

        UnauthorizedToPerformActionOnApplication = 20012,

        SlowmodeRateLimit = 20016,

        AccountOwnerOnlyAction = 20018,

        AnnouncementRateLimit = 20022,

        ChannelWriteRateLimit = 20028,

        GuildWriteRateLimit = 20029,

        /// <summary>
        ///     The stage topic, guild name, guild description, or channel names contain words that are not allowed.
        /// </summary>
        DisallowedStageWords = 20031,

        GuildBoostLevelTooLow = 20035,

        MaximumGuildAmountReached = 30001,

        /// <summary>
        ///     The maximum of <c>1000</c> friends was reached.
        /// </summary>
        MaximumFriendAmountReached = 30002,

        /// <summary>
        ///     The maximum of <c>50</c> pins was reached.
        /// </summary>
        MaximumPinAmountReached = 30003,

        /// <summary>
        ///     The maximum of <c>10</c> recipients was reached.
        /// </summary>
        MaximumRecipientsAmountReached = 30004,

        /// <summary>
        ///     The maximum of <c>250</c> roles was reached.
        /// </summary>
        MaximumRoleAmountReached = 30005,

        /// <summary>
        ///     The maximum of <c>10</c> webhooks was reached.
        /// </summary>
        MaximumWebhookAmountReached = 30007,

        MaximumEmojisAmountReached = 30008,

        /// <summary>
        ///     The maximum of <c>20</c> reactions was reached.
        /// </summary>
        MaximumReactionAmountReached = 30010,

        /// <summary>
        ///     The maximum of <c>500</c> channels was reached.
        /// </summary>
        MaximumGuildChannelAmountReached = 30013,

        /// <summary>
        ///     The maximum of <c>10</c> attachments was reached.
        /// </summary>
        MaximumAttachmentAmountReached = 30015,

        /// <summary>
        ///     The maximum of <c>1000</c> invites was reached.
        /// </summary>
        MaximumInviteAmountReached = 30016,

        MaximumAnimatedEmojiAmountReached = 30018,

        MaximumMemberAmountReached = 30019,

        /// <summary>
        ///     The maximum of <c>5</c> categories was reached.
        /// </summary>
        MaximumDiscoverableCategoryAmountReached = 30030,

        GuildAlreadyHasATemplate = 30031,

        MaximumThreadParticipantAmountReached = 30033,

        MaximumNonMemberBanAmountReached = 30035,

        BanFetchRateLimit = 30037,

        /// <summary>
        ///     The maximum of <c>100</c> uncompleted guild events was reached.
        /// </summary>
        MaximumUncompletedGuildEventsReached = 30038,

        MaximumStickerAmountReached = 30039,

        MaximumPruneRequestAmountReached = 30040,

        MaximumGuildWidgetSettingsUpdatesReached = 30042,

        /// <summary>
        ///     The maximum number of edits to messages older than 1 hour was reached.
        /// </summary>
        MaximumEditsToOldMessagesReached = 30046,

        Unauthorized = 40001,

        VerifiedAccountRequiredToPerformAction = 40002,

        DirectMessageChannelRateLimit = 40003,

        SendingMessagesTemporarilyDisabled = 40004,

        RequestEntityTooLarge = 40005,

        FeatureTemporarilyDisabled = 40006,

        UserBanned = 40007,

        TargetUserNotConnectedToVoice = 40032,

        MessageAlreadyCrossposted = 40033,

        ApplicationCommandAlreadyExists = 40041,

        InteractionAlreadyAcknowledged = 40060,

        MissingAccess = 50001,

        InvalidAccountType = 50002,

        InvalidDirectChannelAction = 50003,

        WidgetDisabled = 50004,

        CannotEditMessagesAuthoredByAnotherUser = 50005,

        CannotSendAnEmptyMessage = 50006,

        CannotSendMessagesToThisUser = 50007,

        CannotSendMessagesInANonTextChannel = 50008,

        ChannelVerificationLevelIsTooHigh = 50009,

        OAuth2ApplicationDoesNotHaveABot = 50010,

        OAuth2ApplicationLimitReached = 50011,

        InvalidOAuth2State = 50012,

        MissingPermissions = 50013,

        InvalidAuthenticationToken = 50014,

        NoteTooLong = 50015,

        /// <summary>
        ///     An invalid amount of messages was provided to the bulk message deletion endpoint which accepts from <c>2</c> to <c>100</c> messages.
        /// </summary>
        InvalidBulkMessageDeletionAmount = 50016,

        AMessageCanOnlyBePinnedToTheChannelItWasSent = 50019,

        InviteCodeIsEitherInvalidOrTaken = 50020,

        CannotExecuteActionOnASystemMessage = 50021,

        CannotExecuteActionOnThisChannelType = 50024,

        InvalidOAuth2AccessToken = 50025,

        MissingOAuth2Scope = 50026,

        InvalidWebhookToken = 50027,

        InvalidRole = 50028,

        InvalidRecipient = 50033,

        AMessageProvidedWasTooOldToBulkDelete = 50034,

        InvalidFormBody = 50035,

        AnInviteWasAcceptedToAGuildTheApplicationsBotIsNotIn = 50036,

        InvalidApiVersion = 50041,

        MaximumFileSizeExceeded = 50045,

        InvalidFileType = 50046,

        CannotSelfRedeemGift = 50054,

        InvalidGuild = 50055,

        InvalidMessageType = 50068,

        PaymentSourceRequiredToRedeemGift = 50070,

        CannotDeleteAChannelRequiredForCommunityGuilds = 50074,

        InvalidStickerSent = 50081,

        ThreadArchived = 50083,

        InvalidThreadNotifications = 50084,

        /// <summary>
        ///     <c>before</c> value is earlier than the thread creation date.
        /// </summary>
        ValueBeforeThreadCreation = 50085,

        CommunityChannelsMustBeTextChannels = 50086,

        GuildNotAvailableInCurrentLocation = 50095,

        GuildMonetizationRequiredForAction = 50097,

        GuildBoostsRequiredForAction = 50101,

        RequestBodyContainsInvalidJson = 50109,

        TwoFactorRequired = 60003,

        NoUsersWithDiscordTagExist = 80004,

        ReactionBlocked = 90001,

        ResourceOverloaded = 130000,

        StageAlreadyOpen = 150006,

        CannotReplyWithoutReadHistoryPermissions = 160002,

        ThreadAlreadyCreated = 160004,

        ThreadLocked = 160005,

        MaximumActiveThreadAmountReached = 160006,

        MaximumActiveAnnouncementThreadAmountReached = 160007,

        InvalidLottieFile = 170001,

        LottieCannotContainRasterizedImages = 170002,

        MaximumStickerFrameRateExceeded = 170003,

        /// <summary>
        ///     The maximum of <c>1000</c> frames was reached.
        /// </summary>
        MaximumStickerFrameCountExceeded = 170004,

        MaximumLottieDimensionsExceeded = 170005,

        InvalidStickerFrameRate = 170006,

        /// <summary>
        ///     The maximum duration of <c>5</c> seconds was reached.
        /// </summary>
        MaximumLottieDurationExceeded = 170007,

        CannotUpdateFinishedEvent = 180000,

        FailedToCreateEventStageChannel = 180002
    }
}
