namespace Disqord;

public enum ComponentType : byte
{
    Row = 1,

    Button = 2,

    StringSelection = 3,

    TextInput = 4,

    UserSelection = 5,

    RoleSelection = 6,

    MentionableSelection = 7,

    ChannelSelection = 8,

    // Components V2
    Section = 9,

    TextDisplay = 10,

    Thumbnail = 11,

    MediaGallery = 12,

    File = 13,

    Separator = 14,

    Container = 17
}
