namespace Disqord;

/// <summary>
///     Represents the type of role connection metadata.
/// </summary>
public enum ApplicationRoleConnectionMetadataType
{
    /// <summary>
    ///     The metadata value (integer) is less than or equal to the guild's configured value.
    /// </summary>
    IntegerLessThanOrEqual = 1,

    /// <summary>
    ///     The metadata value (integer) is greater than or equal to the guild's configured value.
    /// </summary>
    IntegerGreaterThanOrEqual = 2,

    /// <summary>
    ///     The metadata value (integer) is equal to the guild's configured value.
    /// </summary>
    IntegerEqual = 3,

    /// <summary>
    ///     The metadata value (integer) is not equal to the guild's configured value.
    /// </summary>
    IntegerNotEqual = 4,

    /// <summary>
    ///     The metadata value (date time; ISO8601 string) is less than or equal to the guild's configured value (in days).
    /// </summary>
    DateTimeLessThanOrEqual = 5,

    /// <summary>
    ///     The metadata value (date time; ISO8601 string) is greater than or equal to the guild's configured value (in days).
    /// </summary>
    DateTimeGreaterThanOrEqual = 6,

    /// <summary>
    ///     The metadata value (integer) is equal to the guild's configured value (<c>1</c>).
    /// </summary>
    BooleanEqual = 7,

    /// <summary>
    ///     The metadata value (integer) is not equal to the guild's configured value (<c>1</c>).
    /// </summary>
    BooleanNotEqual = 8
}
