using Disqord.Serialization.Json;

namespace Disqord.Rest.Api;

public class CreateEntitlementJsonRestRequestContent : JsonModelRestRequestContent
{
    [JsonProperty("sku_id")]
    public Snowflake SkuId;

    [JsonProperty("owner_id")]
    public Snowflake OwnerId;

    [JsonProperty("owner_type")]
    public EntitlementOwnerType OwnerType;
}
