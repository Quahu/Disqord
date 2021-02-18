using System.Collections.Generic;
using System.Threading.Tasks;

namespace Disqord.Gateway
{
    public interface IGatewayCache<TEntity>
        where TEntity : IGatewayEntity, ISnowflakeEntity
    {
        ValueTask<TEntity> GetAsync(Snowflake id);

        ValueTask<IEnumerable<KeyValuePair<Snowflake, TEntity>>> GetRangeAsync(IEnumerable<Snowflake> ids);

        ValueTask AddAsync(TEntity entity);

        ValueTask AddRangeAsync(IEnumerable<TEntity> entities);

        ValueTask<TEntity> RemoveAsync(Snowflake id);

        ValueTask<IEnumerable<KeyValuePair<Snowflake, TEntity>>> RemoveRangeAsync(IEnumerable<Snowflake> ids);

        ValueTask<IEnumerable<KeyValuePair<Snowflake, TEntity>>> ClearAsync();
    }
}
