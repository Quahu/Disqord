using System;
using System.Collections.Generic;
using System.Globalization;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models;

public class ApplicationCommandOptionChoiceJsonModel : JsonModel
{
    [JsonProperty("name")]
    public string Name = null!;

    [JsonProperty("name_localizations")]
    public Optional<Dictionary<string, string>?> NameLocalizations;

    [JsonProperty("value")]
    public IJsonValue Value = null!;

    protected override void OnValidate()
    {
        Guard.IsNotNullOrWhiteSpace(Name);
        Guard.HasSizeBetweenOrEqualTo(Name, Discord.Limits.ApplicationCommand.Option.Choice.MinNameLength, Discord.Limits.ApplicationCommand.Option.Choice.MaxNameLength);

        Guard.IsNotNull(Value);
        Guard.IsNotNull(Value.Value);

        var value = Guard.IsAssignableToType<IConvertible>(Value.Value, nameof(Value));
        switch (value.GetTypeCode())
        {
            case TypeCode.SByte:
            case TypeCode.Byte:
            case TypeCode.Int16:
            case TypeCode.UInt16:
            case TypeCode.Int32:
            case TypeCode.UInt32:
            case TypeCode.Int64:
            case TypeCode.UInt64:
            {
                var integralValue = value.ToInt64(CultureInfo.InvariantCulture);
                Guard.IsBetweenOrEqualTo(integralValue, Discord.Limits.ApplicationCommand.Option.Choice.MinIntegralValue, Discord.Limits.ApplicationCommand.Option.Choice.MaxIntegralValue);
                break;
            }
            case TypeCode.Single:
            case TypeCode.Double:
            case TypeCode.Decimal:
            {
                var floatingPointValue = value.ToDouble(CultureInfo.InvariantCulture);
                Guard.IsFinite(floatingPointValue);
                break;
            }
            case TypeCode.String:
            {
                var stringValue = value.ToString(CultureInfo.InvariantCulture);
                Guard.HasSizeLessThanOrEqualTo(stringValue, Discord.Limits.ApplicationCommand.Option.Choice.MaxStringValueLength);
                break;
            }
            default:
            {
                Throw.ArgumentException("Invalid value provided. Must be an integral, floating-point, or a string value.");
                return;
            }
        }
    }
}