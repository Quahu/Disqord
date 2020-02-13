using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Disqord
{
    public sealed partial class CachedMember : CachedUser, IMember
    {
        private sealed class RoleCollection : IDictionary<Snowflake, CachedRole>, IReadOnlyDictionary<Snowflake, CachedRole>
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

            public ICollection<Snowflake> Keys
            {
                get
                {
                    lock (_ids)
                    {
                        return _ids.ToArray();
                    }
                }
            }

            public ICollection<CachedRole> Values => this.Select(x => x.Value).ToArray();

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
                _ids = new List<Snowflake>(ids.Length + 1)
                {
                    guild.Id
                };
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
                    _ids.Add(_guild.Id);
                    for (var i = 0; i < ids.Length; i++)
                    {
                        var id = ids[i];
                        if (!_guild.Roles.ContainsKey(id))
                            continue;

                        _ids.Add(id);
                    }
                }
            }

            public bool Contains(KeyValuePair<Snowflake, CachedRole> item)
                => ContainsKey(item.Key);

            public void CopyTo(KeyValuePair<Snowflake, CachedRole>[] array, int arrayIndex)
            {
                if (array == null)
                    throw new ArgumentNullException(nameof(array));

                if (arrayIndex < 0 || arrayIndex >= array.Length)
                    throw new ArgumentOutOfRangeException(nameof(arrayIndex));

                var index = arrayIndex;
                foreach (var kvp in this)
                    array[index++] = kvp;
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
                //static IEnumerable<KeyValuePair<Snowflake, CachedRole>> GetEnumerable(RoleCollection collection)
                //{
                //    for (var i = 0; i < collection._ids.Count; i++)
                //    {
                //        var id = collection._ids[i];
                //        if (collection._guild.Roles.TryGetValue(id, out var role))
                //            yield return KeyValuePair.Create(id, role);
                //    }
                //}

                //return GetEnumerable(this).GetEnumerator().Locked(_ids);

                foreach (var id in Keys)
                {
                    if (_guild.Roles.TryGetValue(id, out var role))
                        yield return KeyValuePair.Create(id, role);
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
                => GetEnumerator();

            IEnumerable<Snowflake> IReadOnlyDictionary<Snowflake, CachedRole>.Keys => Keys;
            IEnumerable<CachedRole> IReadOnlyDictionary<Snowflake, CachedRole>.Values => Values;
            bool ICollection<KeyValuePair<Snowflake, CachedRole>>.IsReadOnly => true;
            CachedRole IDictionary<Snowflake, CachedRole>.this[Snowflake key]
            {
                get => this[key];
                set => throw new NotSupportedException();
            }

            void IDictionary<Snowflake, CachedRole>.Add(Snowflake key, CachedRole value)
                => throw new NotSupportedException();

            bool IDictionary<Snowflake, CachedRole>.Remove(Snowflake key)
                => throw new NotSupportedException();

            void ICollection<KeyValuePair<Snowflake, CachedRole>>.Add(KeyValuePair<Snowflake, CachedRole> item)
                => throw new NotSupportedException();

            bool ICollection<KeyValuePair<Snowflake, CachedRole>>.Remove(KeyValuePair<Snowflake, CachedRole> item)
                => throw new NotSupportedException();

            void ICollection<KeyValuePair<Snowflake, CachedRole>>.Clear()
                => throw new NotSupportedException();
        }
    }
}