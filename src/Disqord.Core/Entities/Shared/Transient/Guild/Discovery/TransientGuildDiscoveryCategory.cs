using System.Collections.Generic;
using System.Globalization;
using Disqord.Collections;
using Disqord.Models;
using Disqord.Serialization.Json;

namespace Disqord
{
    public class TransientGuildDiscoveryCategory : TransientEntity<GuildDiscoveryCategoryJsonModel>, IGuildDiscoveryCategory
    {
        public string Name => Model.Name.Default;

        public int Id => Model.Id;

        public bool IsPrimary => Model.IsPrimary;

        public IReadOnlyDictionary<CultureInfo, string> LocalizedNames => _localizedNames ??= Optional.ConvertOrDefault(Model.Name.Localizations,
            x => x.ToReadOnlyDictionary(x => Discord.Internal.GetLocale(x.Key), x => (x.Value as IJsonValue).Value as string), ReadOnlyDictionary<CultureInfo, string>.Empty);
        private IReadOnlyDictionary<CultureInfo, string> _localizedNames;

        public TransientGuildDiscoveryCategory(IClient client, GuildDiscoveryCategoryJsonModel model)
            : base(client, model)
        { }
    }
}