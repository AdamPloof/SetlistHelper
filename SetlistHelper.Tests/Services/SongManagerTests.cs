using System.Collections.Generic;
using Xunit;

using SetlistHelper.Services;
using SetlistHelper.Models;

namespace SetlistHelper.Tests.Services;

public class SongManagerTests {
    [Fact]
    public void SongsListContainsAllSongsInDataSource() {
        SongManager manager = new SongManager();
        Dictionary<string, Song> songs = manager.GetSongs();

        Assert.Single(songs);
    }

    [Fact]
    public void GetSongByTitleReturnCorrectSong() {
        SongManager manager = new SongManager();
        Song? song = manager.GetSong("Yesterdays");

        Assert.NotNull(song);
        if (song != null) {
            Assert.Equal("Yesterdays", song.Title);
        }
    }
}
