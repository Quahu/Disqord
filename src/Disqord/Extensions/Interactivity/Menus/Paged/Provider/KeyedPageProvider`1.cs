// using System.Collections.Generic;
// using System.Linq;
//
// namespace Disqord.Extensions.Interactivity.Menus.Paged
// {
//     class KeyedPageProvider<TKey> : ListPageProvider
//     {
//         public IReadOnlyList<TKey> Keys { get; }
//
//         public KeyedPageProvider(IReadOnlyDictionary<TKey, Page> pages)
//             : base(pages?.Select(x => x.Value))
//         {
//             Keys = pages.Select(x => x.Key).ToArray();
//         }
//     }
// }
