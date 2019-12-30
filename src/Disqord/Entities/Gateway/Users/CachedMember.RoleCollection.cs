using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Qommon.Collections;

namespace Disqord
{
    public sealed partial class CachedMember : CachedUser, IMember
    {
        internal sealed class RoleCollection : IReadOnlyDictionary<Snowflake, CachedRole>
        {
            public int Count => _ids.Count;

            public IEnumerable<Snowflake> Keys => new ReadOnlySet<Snowflake>(_ids);

            public IEnumerable<CachedRole> Values => this.Select(x => x.Value);

            public CachedRole this[Snowflake key]
            {
                get
                {
                    if (_ids.Contains(key))
                        return _guild.Roles[key];

                    throw new KeyNotFoundException();
                }
            }

            private readonly CachedGuild _guild;
            private readonly HashSet<Snowflake> _ids;

            public RoleCollection(CachedGuild guild, ulong[] ids)
            {
                _guild = guild;
                _ids = new HashSet<Snowflake>(ids.Length + 1);
                Update(ids);
            }

            public void Update(ulong[] ids)
            {
                _ids.Clear();
                _ids.Add(_guild.Id);
                for (var i = 0; i < ids.Length; i++)
                {
                    var id = ids[i];
                    if (!_guild.Roles.ContainsKey(id))
                        continue;

                    _ids.Add(id);
                }
            }

            public bool ContainsKey(Snowflake key)
                => _ids.Contains(key);

            public bool TryGetValue(Snowflake key, out CachedRole value)
            {
                if (_ids.Contains(key) && _guild.Roles.TryGetValue(key, out value))
                    return true;

                value = null;
                return false;
            }

            public IEnumerator<KeyValuePair<Snowflake, CachedRole>> GetEnumerator()
            {
                foreach (var id in _ids)
                {
                    if (_guild.Roles.TryGetValue(id, out var role))
                        yield return KeyValuePair.Create(id, role);
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
                => GetEnumerator();
        }
    }
}
