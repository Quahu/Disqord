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

namespace Disqord.Test.Modules.ApplicationCommands
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

        [SlashCommand("test1")]
        public IResult Test1(string first, int second, int third = 42)
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
        [AutoComplete("test1")]
        [AutoComplete("test2")]
        public void AutoCompleteTest(AutoComplete<string> first,
            Optional<int> second, /* 'second' isn't auto-completed, but we can still view its value */
            AutoComplete<int> third)
        {
            if (first.IsFocused)
            {
                if (first.RawArgument.StartsWith("Aurelio Voltaire"))
                {
                    first.Choices.AddRange("Captains All", "Dead", "The Headless Waltz");
                }
            }
            else if (third.IsFocused)
            {
                if (int.TryParse(third.RawArgument, out var intArgument))
                {
                    third.Choices.Add(intArgument + 1);
                }
                else
                {
                    third.Choices.Add(42);
                }
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
            if (artist.IsFocused)
            {
                if (artist.RawArgument != string.Empty)
                {
                    // Find artists matching the user input.
                    foreach (var existingArtist in _artists)
                    {
                        if (!existingArtist.StartsWith(artist.RawArgument, StringComparison.OrdinalIgnoreCase))
                            continue;

                        // Add the matching artists as choices.
                        artist.Choices.Add(existingArtist);
                    }
                }
                else
                {
                    // Add all artists as choices.
                    artist.Choices.AddRange(_artists);
                }
            }
            else if (album.IsFocused)
            {
                // If `album` is currently focused,
                // check if the user's current `artist` value is a valid artist.
                if (!_artists.TryGetValue(artist.Argument.GetValueOrDefault(""), out var existingArtist))
                    return;

                var albumPath = Path.Join(_musicPath, existingArtist);
                var existingAlbums = Directory.EnumerateDirectories(albumPath).Select(file => Path.GetFileName(file));
                if (album.RawArgument != string.Empty)
                {
                    // Find albums on the disk matching the user input.
                    foreach (var existingAlbum in existingAlbums)
                    {
                        if (!existingAlbum.StartsWith(album.RawArgument, StringComparison.OrdinalIgnoreCase))
                            continue;

                        // Add the matching albums as choices.
                        album.Choices.Add(existingAlbum);
                    }
                }
                else
                {
                    // Add all albums on the disk as choices.
                    album.Choices.AddRange(existingAlbums);
                }
            }
            else if (title.IsFocused)
            {
                // If `album` is currently focused,
                // check if the user's current `artist` value is a valid artist.
                if (!_artists.TryGetValue(artist.Argument.GetValueOrDefault(""), out var existingArtist)
                    || !album.Argument.TryGetValue(out var currentAlbum))
                    return;

                // Then check if the user's current `album` value is a valid album.
                var albumPath = Path.Join(_musicPath, existingArtist, currentAlbum);
                if (!Directory.Exists(albumPath))
                    return;

                var existingTitles = Directory.EnumerateFiles(albumPath, "*.flac").Select(file => Path.GetFileName(file));
                if (title.RawArgument != string.Empty)
                {
                    // Then find all matching song titles on the disk in the album folder.
                    foreach (var existingTitle in existingTitles)
                    {
                        if (!existingTitle.StartsWith(title.RawArgument, StringComparison.OrdinalIgnoreCase))
                            continue;

                        // Add the matching titles as choices.
                        title.Choices.Add(existingTitle);
                    }
                }
                else
                {
                    title.Choices.AddRange(existingTitles);
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
                .WithDescription(memberColor?.ToString() ?? "Member has no color");

            if (memberColor != null)
                embed.WithColor(memberColor.Value);

            return Response(embed);
        }

        [MessageCommand("Get Length")]
        public IResult GetLength(IMessage message)
        {
            return Response($"The message is {message.Content.Length} characters long");
        }
    }
}
