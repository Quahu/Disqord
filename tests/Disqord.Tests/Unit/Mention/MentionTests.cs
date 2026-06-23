using System.Linq;

namespace Disqord.Tests.Unit.Mention;

public class MentionTests
{
    private const ulong UserId = 123456789012345678;

    private const ulong ChannelId = 234567890123456789;

    private const ulong RoleId = 345678901234567890;

    private const ulong CommandId = 456789012345678901;

    [Test]
    public void User_WithoutNick_ReturnsBareMention()
    {
        // Act
        var mention = Disqord.Mention.User(UserId);

        // Assert
        Assert.That(mention, Is.EqualTo($"<@{UserId}>"));
    }

    [Test]
    public void User_WithNick_ReturnsNickMention()
    {
        // Act
        var mention = Disqord.Mention.User(UserId, hasNick: true);

        // Assert
        Assert.That(mention, Is.EqualTo($"<@!{UserId}>"));
    }

    [Test]
    public void Channel_Snowflake_ReturnsChannelMention()
    {
        // Act
        var mention = Disqord.Mention.Channel(ChannelId);

        // Assert
        Assert.That(mention, Is.EqualTo($"<#{ChannelId}>"));
    }

    [Test]
    public void Role_Snowflake_ReturnsRoleMention()
    {
        // Act
        var mention = Disqord.Mention.Role(RoleId);

        // Assert
        Assert.That(mention, Is.EqualTo($"<@&{RoleId}>"));
    }

    [Test]
    public void SlashCommand_NameAndId_ReturnsCommandMention()
    {
        // Act
        var mention = Disqord.Mention.SlashCommand(CommandId, "ban");

        // Assert
        Assert.That(mention, Is.EqualTo($"</ban:{CommandId}>"));
    }

    [Test]
    public void SlashCommand_WithSubcommand_ReturnsCommandMention()
    {
        // Act
        var mention = Disqord.Mention.SlashCommand(CommandId, "config", "get");

        // Assert
        Assert.That(mention, Is.EqualTo($"</config get:{CommandId}>"));
    }

    [Test]
    public void SlashCommand_WithSubcommandAndGroup_ReturnsCommandMention()
    {
        // Act
        var mention = Disqord.Mention.SlashCommand(CommandId, "config", "roles", "add");

        // Assert
        Assert.That(mention, Is.EqualTo($"</config roles add:{CommandId}>"));
    }

    [Test]
    public void TryParseUser_BareMention_ReturnsTrueAndId()
    {
        // Act
        var parsed = Disqord.Mention.TryParseUser($"<@{UserId}>", out var result);

        // Assert
        Assert.That(parsed, Is.True);
        Assert.That(result.RawValue, Is.EqualTo(UserId));
    }

    [Test]
    public void TryParseUser_NickMention_ReturnsTrueAndId()
    {
        // Act
        var parsed = Disqord.Mention.TryParseUser($"<@!{UserId}>", out var result);

        // Assert
        Assert.That(parsed, Is.True);
        Assert.That(result.RawValue, Is.EqualTo(UserId));
    }

    [Test]
    public void TryParseUser_RoleMention_ReturnsFalse()
    {
        // Act
        var parsed = Disqord.Mention.TryParseUser($"<@&{RoleId}>", out var result);

        // Assert
        Assert.That(parsed, Is.False);
        Assert.That(result.RawValue, Is.EqualTo(0UL));
    }

    [Test]
    public void TryParseUser_NonMention_ReturnsFalse()
    {
        // Act
        var parsed = Disqord.Mention.TryParseUser("not a mention", out var result);

        // Assert
        Assert.That(parsed, Is.False);
        Assert.That(result.RawValue, Is.EqualTo(0UL));
    }

    [Test]
    public void TryParseChannel_ChannelMention_ReturnsTrueAndId()
    {
        // Act
        var parsed = Disqord.Mention.TryParseChannel($"<#{ChannelId}>", out var result);

        // Assert
        Assert.That(parsed, Is.True);
        Assert.That(result.RawValue, Is.EqualTo(ChannelId));
    }

    [Test]
    public void TryParseChannel_UserMention_ReturnsFalse()
    {
        // Act
        var parsed = Disqord.Mention.TryParseChannel($"<@{UserId}>", out var result);

        // Assert
        Assert.That(parsed, Is.False);
        Assert.That(result.RawValue, Is.EqualTo(0UL));
    }

    [Test]
    public void TryParseRole_RoleMention_ReturnsTrueAndId()
    {
        // Act
        var parsed = Disqord.Mention.TryParseRole($"<@&{RoleId}>", out var result);

        // Assert
        Assert.That(parsed, Is.True);
        Assert.That(result.RawValue, Is.EqualTo(RoleId));
    }

    [Test]
    public void TryParseRole_UserMention_ReturnsFalse()
    {
        // Act
        var parsed = Disqord.Mention.TryParseRole($"<@{UserId}>", out var result);

        // Assert
        Assert.That(parsed, Is.False);
        Assert.That(result.RawValue, Is.EqualTo(0UL));
    }

    [Test]
    public void TryParseRole_NonMention_ReturnsFalse()
    {
        // Act
        var parsed = Disqord.Mention.TryParseRole("<@&abc>", out var result);

        // Assert
        Assert.That(parsed, Is.False);
        Assert.That(result.RawValue, Is.EqualTo(0UL));
    }

    [Test]
    public void Role_ThenTryParseRole_ReturnsOriginalId()
    {
        // Arrange
        var mention = Disqord.Mention.Role(RoleId);

        // Act
        var parsed = Disqord.Mention.TryParseRole(mention, out var result);

        // Assert
        Assert.That(parsed, Is.True);
        Assert.That(result.RawValue, Is.EqualTo(RoleId));
    }

    [Test]
    public void ParseUsers_MixedMentions_ReturnsAllIds()
    {
        // Arrange
        var content = $"hey <@{UserId}> and <@!{ChannelId}> there";

        // Act
        var ids = Disqord.Mention.ParseUsers(content).Select(static snowflake => snowflake.RawValue).ToArray();

        // Assert
        Assert.That(ids, Is.EqualTo(new[] { UserId, ChannelId }));
    }

    [Test]
    public void ParseUsers_NoMentions_ReturnsEmpty()
    {
        // Act
        var ids = Disqord.Mention.ParseUsers("nothing here").ToArray();

        // Assert
        Assert.That(ids, Is.Empty);
    }

    [Test]
    public void ParseChannels_MultipleMentions_ReturnsAllIds()
    {
        // Arrange
        var content = $"<#{ChannelId}> see also <#{RoleId}>";

        // Act
        var ids = Disqord.Mention.ParseChannels(content).Select(static snowflake => snowflake.RawValue).ToArray();

        // Assert
        Assert.That(ids, Is.EqualTo(new[] { ChannelId, RoleId }));
    }

    [Test]
    public void ParseRoles_MultiDigitMention_ReturnsFullId()
    {
        // Act
        var ids = Disqord.Mention.ParseRoles($"<@&{RoleId}>").Select(static snowflake => snowflake.RawValue).ToArray();

        // Assert
        Assert.That(ids, Is.EqualTo(new[] { RoleId }));
    }

    [Test]
    public void ParseRoles_MultipleMentions_ReturnsAllIds()
    {
        // Arrange
        var content = $"<@&{RoleId}> ping <@&{UserId}>";

        // Act
        var ids = Disqord.Mention.ParseRoles(content).Select(static snowflake => snowflake.RawValue).ToArray();

        // Assert
        Assert.That(ids, Is.EqualTo(new[] { RoleId, UserId }));
    }

    [Test]
    public void ParseRoles_NoMentions_ReturnsEmpty()
    {
        // Act
        var ids = Disqord.Mention.ParseRoles($"<@{UserId}> <#{ChannelId}>").ToArray();

        // Assert
        Assert.That(ids, Is.Empty);
    }

    [Test]
    public void Escape_Everyone_InsertsZeroWidthSpace()
    {
        // Act
        var escaped = Disqord.Mention.Escape("@everyone");

        // Assert
        Assert.That(escaped, Is.EqualTo($"@{(char) 0x200B}everyone"));
    }

    [Test]
    public void Escape_Here_InsertsZeroWidthSpace()
    {
        // Act
        var escaped = Disqord.Mention.Escape("@here");

        // Assert
        Assert.That(escaped, Is.EqualTo($"@{(char) 0x200B}here"));
    }

    [Test]
    public void Escape_NumericMention_InsertsZeroWidthSpace()
    {
        // Act
        var escaped = Disqord.Mention.Escape($"<@{UserId}>");

        // Assert
        Assert.That(escaped, Is.EqualTo($"<@{(char) 0x200B}{UserId}>"));
    }
}
