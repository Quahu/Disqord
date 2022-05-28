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
    [RequireGuild(416256456505950215, Group = 0)]
    [RequireGuild(342514280848949249, Group = 0)]
    public class ApplicationGuildModule : DiscordApplicationGuildModuleBase
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

        [AutoComplete("test")]
        [AutoComplete("test2")]
        public IResult AutoCompleteTest(AutoComplete<string> first, /* 'second' isn't auto-completed */ AutoComplete<int> third)
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

            return Results.Success;
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

        [AutoComplete("play")]
        [RateLimit(1, 1, RateLimitMeasure.Seconds, RateLimitBucketType.Member)]
        public void AutoCompletePlay(AutoComplete<string> artist, AutoComplete<string> album, AutoComplete<string> title)
        {
            if (artist.IsCurrentlyFocused(out var currentArtist))
            {
                foreach (var existingArtist in _artists)
                {
                    if (!existingArtist.StartsWith(currentArtist, StringComparison.OrdinalIgnoreCase))
                        continue;

                    artist.Choices.Add(existingArtist);
                }
            }
            else if (album.IsCurrentlyFocused(out var currentAlbum))
            {
                if (!_artists.TryGetValue(artist.Current.GetValueOrDefault(), out var existingArtist))
                    return;

                var albumPath = Path.Join(_musicPath, existingArtist);
                foreach (var existingAlbumPath in Directory.EnumerateDirectories(albumPath))
                {
                    var existingAlbum = Path.GetFileName(existingAlbumPath);
                    if (!existingAlbum.StartsWith(currentAlbum, StringComparison.OrdinalIgnoreCase))
                        continue;

                    album.Choices.Add(existingAlbum);
                }
            }
            else if (title.IsCurrentlyFocused(out var currentTitle))
            {
                if (!_artists.TryGetValue(artist.Current.GetValueOrDefault(), out var existingArtist) || !album.Current.TryGetValue(out currentAlbum) || string.IsNullOrWhiteSpace(currentAlbum))
                    return;

                var albumDirectory = Path.Join(_musicPath, existingArtist, currentAlbum);
                if (!Directory.Exists(albumDirectory))
                    return;

                foreach (var existingTitle in Directory.EnumerateFiles(albumDirectory, "*.flac").Select(Path.GetFileName))
                {
                    if (!existingTitle.StartsWith(currentTitle, StringComparison.OrdinalIgnoreCase))
                        continue;

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
            return Response(new LocalEmbed()
                .WithAuthor(member)
                .WithDescription(memberColor?.ToString() ?? "Member has no color")
                .WithColor(memberColor));
        }

        [MessageCommand("Get Length")]
        public IResult GetLength(IMessage message)
        {
            return Response($"The message is {message.Content.Length} characters long");
        }
    }
}
