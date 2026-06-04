using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Tests.Models;

public class SimpleTestModel : JsonModel
{
    [JsonProperty("name")]
    public string Name = null!;

    [JsonProperty("value")]
    public int Value;
}

public class SnowflakeTestModel : JsonModel
{
    [JsonProperty("id")]
    public Snowflake Id;

    [JsonProperty("optional_id")]
    public Optional<Snowflake> OptionalId;

    [JsonProperty("nullable_id")]
    public Snowflake? NullableId;

    [JsonProperty("ids")]
    public Optional<Snowflake[]> Ids;

    [JsonProperty("snowflake_dict")]
    public Optional<Dictionary<Snowflake, string>> SnowflakeDict;
}

public class OptionalTestModel : JsonModel
{
    [JsonProperty("name")]
    public Optional<string> Name;

    [JsonProperty("count")]
    public Optional<int> Count;

    [JsonProperty("nullable_name")]
    public Optional<string?> NullableName;

    [JsonProperty("nullable_snowflake")]
    public Optional<Snowflake?> NullableSnowflake;

    [JsonProperty("items")]
    public Optional<int[]> Items;

    [JsonProperty("child")]
    public Optional<SimpleTestModel> Child;
}

public class NullableTestModel : JsonModel
{
    [JsonProperty("nullable_string")]
    public string? NullableString;

    [JsonProperty("nullable_int")]
    public int? NullableInt;

    [JsonProperty("nullable_snowflake")]
    public Snowflake? NullableSnowflake;

    [JsonProperty("nullable_date")]
    public DateTimeOffset? NullableDate;
}

public class NullHandlingTestModel : JsonModel
{
    [JsonProperty("value", NullValueHandling.Ignore)]
    public string? Value;

    [JsonProperty("always")]
    public string? Always;
}

public class EnumTestModel : JsonModel
{
    [JsonProperty("component_type")]
    public ComponentType NumericEnum;

    [JsonProperty("team_role")]
    public TeamMemberRole StringEnum;

    [JsonProperty("optional_type")]
    public Optional<ComponentType> OptionalEnum;
}

public class NestedTestModel : JsonModel
{
    [JsonProperty("child")]
    public SimpleTestModel Child = null!;

    [JsonProperty("optional_child")]
    public Optional<SimpleTestModel> OptionalChild;

    [JsonProperty("children")]
    public SimpleTestModel[] Children = null!;
}

public class ExtensionDataTestModel : JsonModel
{
    [JsonProperty("name")]
    public string Name = null!;
}

[JsonSkippedProperties("skipped", "also_skipped")]
public class SkippedPropertiesTestModel : JsonModel
{
    [JsonProperty("name")]
    public string Name = null!;
}

public class JsonNodeTestModel : JsonModel
{
    [JsonProperty("node")]
    public Optional<IJsonNode> Node;

    [JsonProperty("value")]
    public string Value = null!;
}

public class ByteArrayTestModel : JsonModel
{
    [JsonProperty("data")]
    public byte[] Data = null!;
}

public class JsonIgnoreTestModel : JsonModel
{
    [JsonProperty("included")]
    public string Included = null!;

    [JsonIgnore]
    public string Ignored = null!;

    public string NoAttribute = null!;
}
