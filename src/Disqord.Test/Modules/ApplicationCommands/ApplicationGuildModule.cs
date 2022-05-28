using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Disqord.Bot.Commands;
using Disqord.Bot.Commands.Application;
using Disqord.Gateway;
using Qmmands;
using Qommon;

namespace Disqord.Test.Modules
{
    // [RequireGuild(416256456505950215, Group = 0)]
    // [RequireGuild(342514280848949249, Group = 0)] // Grouped guild checks sync the commands to all the guild IDs
    public class ApplicationGuildModule : DiscordApplicationGuildModuleBase // Guild module-base disables the commands in DMs.
    {
        [SlashCommand("deferral")]
        public async Task<IResult> Deferral()
        {
            await Deferral(isEphemeral: true);
            await Task.Delay(10_000);
            return Response("Hello after a delay!");
        }

        [SlashCommand("test")]
        public IResult Test(string first, int second, int third = 42)
        {
            return Response($"Results: {first} {second} {third}");
        }

        [SlashCommand("test2")]
        public IResult Test2(string first)
        {
            return Response($"Result: {first}");
        }

        /// <summary>
        ///     This auto-complete method is used for both the commands defined above.
        /// </summary>
        [AutoComplete("test")]
        [AutoComplete("test2")]
        public void AutoCompleteTest(AutoComplete<string> first, /* 'second' isn't auto-completed */ AutoComplete<int> third)
        {
            if (first.IsFocused)
            {
                if (first.Current.Value.StartsWith("Aurelio Voltaire"))
                {
                    first.Choices.AddRange("Captains All", "Dead", "The Headless Waltz");
                }
            }
            else if (third.IsCurrentlyFocused(out var currentValue))
            {
                if (currentValue != 0)
                    third.Choices.Add(currentValue + 1);
            }
        }

        private const string _musicPath = @"Q:\Music\";
        private static HashSet<string> _artists = new(StringComparer.OrdinalIgnoreCase)
        {
            "The Lonely Island",
            "Aurelio Voltaire"
        };

        [SlashCommand("play")]
        public IResult Song(string artist, string album, string title)
        {
            return Response(Path.Join(_musicPath, album, title));
        }

        /// <summary>
        ///     Advanced nested auto-completion example.
        /// </summary>
        /// <remarks>
        ///     The auto-complete doesn't allow the user to specify the parameters in any given order,
        ///     i.e. they have to be specified as they are declared: `artist` -> `album` -> `title`.
        /// </remarks>
        [AutoComplete("play")]
        [RateLimit(1, 1, RateLimitMeasure.Seconds, RateLimitBucketType.Member)]
        public void AutoCompletePlay(AutoComplete<string> artist, AutoComplete<string> album, AutoComplete<string> title)
        {
            if (artist.IsCurrentlyFocused(out var currentArtist))
            {
                // If `artist` is currently focused,
                // find the matching artist in the hashset.
                foreach (var existingArtist in _artists)
                {
                    if (!existingArtist.StartsWith(currentArtist, StringComparison.OrdinalIgnoreCase))
                        continue;

                    // Add the matching artists as choices.
                    artist.Choices.Add(existingArtist);
                }
            }
            else if (album.IsCurrentlyFocused(out var currentAlbum))
            {
                // If `album` is currently focused,
                // check if the user's current `artist` value is a valid artist.
                if (!_artists.TryGetValue(artist.Current.GetValueOrDefault(), out var existingArtist))
                    return;

                // Then find all matching albums on the disk in the artist folder.
                var albumPath = Path.Join(_musicPath, existingArtist);
                foreach (var existingAlbumPath in Directory.EnumerateDirectories(albumPath))
                {
                    var existingAlbum = Path.GetFileName(existingAlbumPath);
                    if (!existingAlbum.StartsWith(currentAlbum, StringComparison.OrdinalIgnoreCase))
                        continue;

                    // Add the matching albums as choices.
                    album.Choices.Add(existingAlbum);
                }
            }
            else if (title.IsCurrentlyFocused(out var currentTitle))
            {
                // If `album` is currently focused,
                // check if the user's current `artist` value is a valid artist.
                if (!_artists.TryGetValue(artist.Current.GetValueOrDefault(), out var existingArtist) || !album.Current.TryGetValue(out currentAlbum) || string.IsNullOrWhiteSpace(currentAlbum))
                    return;

                // Then check if the user's current `album` value is a valid album.
                var albumDirectory = Path.Join(_musicPath, existingArtist, currentAlbum);
                if (!Directory.Exists(albumDirectory))
                    return;

                // Then find all matching song titles on the disk in the album folder.
                foreach (var existingTitle in Directory.EnumerateFiles(albumDirectory, "*.flac").Select(Path.GetFileName))
                {
                    if (!existingTitle.StartsWith(currentTitle, StringComparison.OrdinalIgnoreCase))
                        continue;

                    // Add the matching titles as choices.
                    title.Choices.Add(existingTitle);
                }
            }
        }

        [SlashCommand("sum")]
        public IResult Sum(double a, double b)
        {
            return Response($"{a} + {b} = {a + b} which is very cool");
        }

        [UserCommand("Show Color")]
        public IResult GetColor(IMember member)
        {
            var memberColor = member.GetRoles().Values.Where(x => x.Color != null).OrderByDescending(x => x.Position).FirstOrDefault()?.Color;
            var embed = new LocalEmbed()
                .WithAuthor(member)
                .WithDescription(memberColor?.ToString() ?? "Member has no color")
                .WithColor(memberColor);

            return Response(embed);
        }

        [MessageCommand("Get Length")]
        public IResult GetLength(IMessage message)
        {
            return Response($"The message is {message.Content.Length} characters long");
        }
    }
}
