using System.Collections.Generic;
using Xunit;

using SetlistHelper.Services;
using SetlistHelper.Models;

namespace SetlistHelper.Tests.Services;

public class SetBuilderTests {
    [Fact]
    public void SetWithSingleSongIsBuilt() {
        Dictionary<string, Song> songs = makeSongs(1);
        MockSongManager songManager = new MockSongManager(songs);
        SetBuilder builder = new SetBuilder(songManager);
        SetTemplate template = makeTemplate();
        Setlist setList = builder.Build(template, "Test");

        int expectedSetLength = template.DynamicPlot.Count;
        Assert.Equal(expectedSetLength, setList.Songs.Count);
        foreach (Song song in setList.Songs) {
            Assert.Equal("One", song.Title);
            Assert.Equal(1, song.DynamicLevel);
            Assert.Equal(1, song.Length);
        }
    }

    [Fact]
    public void SetWithNoSongsThrowsException() {
        Dictionary<string, Song> songs = new Dictionary<string, Song>();
        MockSongManager songManager = new MockSongManager(songs);
        SetBuilder builder = new SetBuilder(songManager);
        SetTemplate template = makeTemplate();
        Exception e = Assert.Throws<Exception>(() => builder.Build(template, "Test"));
        Assert.Equal("Could not build setlist. There are no songs in the repertoire.", e.Message);
    }

    [Fact]
    public void SetWithNotEnoughSongsAreReused() {
        Dictionary<string, Song> songs = makeSongs(6);
        MockSongManager songManager = new MockSongManager(songs);
        SetBuilder builder = new SetBuilder(songManager);
        SetTemplate template = makeTemplate();
        Setlist setList = builder.Build(template, "Test");

        int expectedSetLength = template.DynamicPlot.Count;
        Assert.Equal(expectedSetLength, setList.Songs.Count);
        Dictionary<string, int> songReusedCount = [];
        foreach (Song song in setList.Songs) {
            if (songReusedCount.TryGetValue(song.Title, out int _)) {
                songReusedCount[song.Title]++;
            } else {
                songReusedCount[song.Title] = 1;
            }
        }

        List<Song> songsUsedMoreThanOnce = [];
        foreach (KeyValuePair<string, int> songCount in songReusedCount) {
            if (songCount.Value > 1) {
                songsUsedMoreThanOnce.Add(songs[songCount.Key]);
            }
        }

        Assert.Single(songsUsedMoreThanOnce);
    }

    [Fact]
    public void SetWithEnoughSongsAreNotReused() {
        Dictionary<string, Song> songs = makeSongs(10);
        MockSongManager songManager = new MockSongManager(songs);
        SetBuilder builder = new SetBuilder(songManager);
        SetTemplate template = makeTemplate();
        Setlist setList = builder.Build(template, "Test");

        int expectedSetLength = template.DynamicPlot.Count;
        Assert.Equal(expectedSetLength, setList.Songs.Count);
        Dictionary<string, int> songReusedCount = [];
        foreach (Song song in setList.Songs) {
            if (songReusedCount.TryGetValue(song.Title, out int _)) {
                songReusedCount[song.Title]++;
            } else {
                songReusedCount[song.Title] = 1;
            }
        }

        List<Song> songsUsedMoreThanOnce = [];
        foreach (KeyValuePair<string, int> songCount in songReusedCount) {
            if (songCount.Value > 1) {
                songsUsedMoreThanOnce.Add(songs[songCount.Key]);
            }
        }

        Assert.Empty(songsUsedMoreThanOnce);
    }

    private SetTemplate makeTemplate() {
        SetTemplate template = new SetTemplate("test");
        template.AddStep(2);
        template.AddStep(2);
        template.AddStep(4);
        template.AddStep(5);
        template.AddStep(3);
        template.AddStep(7);
        template.AddStep(8);

        return template;
    }

    private Dictionary<string, Song> makeSongs(int count) {
        List<Song> songs = [];
        songs.Add(new Song() {Title="One", DynamicLevel=1, Length=1});
        songs.Add(new Song() {Title="Two", DynamicLevel=2, Length=2});
        songs.Add(new Song() {Title="Three", DynamicLevel=3, Length=3});
        songs.Add(new Song() {Title="Four", DynamicLevel=4, Length=4});
        songs.Add(new Song() {Title="Five", DynamicLevel=5, Length=5});
        songs.Add(new Song() {Title="Six", DynamicLevel=6, Length=6});
        songs.Add(new Song() {Title="Seven", DynamicLevel=7, Length=7});
        songs.Add(new Song() {Title="Eight", DynamicLevel=8, Length=8});
        songs.Add(new Song() {Title="Nine", DynamicLevel=9, Length=9});
        songs.Add(new Song() {Title="Ten", DynamicLevel=10, Length=10});

        Dictionary<string, Song> songsDict = [];
        for (int i = 0; i < count; i++) {
            songsDict.Add(songs[i].Title, songs[i]);
        }

        return songsDict;
    }
}
