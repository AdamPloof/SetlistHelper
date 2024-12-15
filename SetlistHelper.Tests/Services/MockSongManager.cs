namespace SetlistHelper.Tests.Services;

using SetlistHelper.Services;
using SetlistHelper.Models;

class MockSongManager : ISongStorage {
    private readonly Dictionary<string, Song> _songs;

    public MockSongManager(Dictionary<string, Song> songs) {
        _songs = songs;
    }

    public Song? GetSong(string title) {
        _songs.TryGetValue(title, out Song? song);

        return song;
    }

    public Dictionary<string, Song> GetSongs() {
        return _songs;
    }

    public void Add(Song song) {
        _songs.Add(song.Title, song);
    }

    public void Update(Song song) {
        _songs[song.Title] = song;
    }

    public void Remove(string title) {
        _songs.Remove(title);
    }

    public void List() {}
}
