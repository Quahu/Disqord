using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Disqord
{
    public sealed partial class CachedMember : CachedUser, IMember
    {
        // TODO: LINQ opt and so on
        public sealed class RoleCollection : IReadOnlyDictionary<Snowflake, CachedRole>
        {
            public int Count
            {
                get
                {
                    lock (_ids)
                    {
                        return _ids.Count;
                    }
                }
            }

            public IEnumerable<Snowflake> Keys
            {
                get
                {
                    lock (_ids)
                    {
                        return _ids.ToArray();
                    }
                }
            }

            public IEnumerable<CachedRole> Values => this.Select(x => x.Value);

            public CachedRole this[Snowflake key]
            {
                get
                {
                    if (ContainsKey(key))
                        return _guild.Roles[key];

                    throw new KeyNotFoundException();
                }
            }

            private readonly CachedGuild _guild;
            private readonly List<Snowflake> _ids;

            public RoleCollection(CachedGuild guild, ulong[] ids)
            {
                _guild = guild;
                _ids = new List<Snowflake>(ids.Length);
                for (var i = 0; i < ids.Length; i++)
                {
                    var id = ids[i];
                    if (!guild.Roles.ContainsKey(id))
                        continue;

                    _ids.Add(id);
                }
            }

            internal void Update(ulong[] ids)
            {
                lock (_ids)
                {
                    _ids.Clear();
                    for (var i = 0; i < ids.Length; i++)
                    {
                        var id = ids[i];
                        if (!_guild.Roles.ContainsKey(id))
                            continue;

                        _ids.Add(id);
                    }
                }
            }

            public bool ContainsKey(Snowflake key)
            {
                lock (_ids)
                {
                    return _ids.Contains(key);
                }
            }

            public bool TryGetValue(Snowflake key, out CachedRole value)
            {
                if (ContainsKey(key) && _guild.Roles.TryGetValue(key, out value))
                    return true;

                value = null;
                return false;
            }

            public IEnumerator<KeyValuePair<Snowflake, CachedRole>> GetEnumerator()
            {
                Snowflake[] ids;
                lock (_ids)
                {
                    ids = _ids.ToArray();
                }

                for (var i = 0; i < ids.Length; i++)
                {
                    var id = ids[i];
                    if (_guild.Roles.TryGetValue(id, out var role))
                        yield return KeyValuePair.Create(id, role);
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
                => GetEnumerator();
        }
    }
}
