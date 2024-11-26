namespace SetlistHelper.Services;

using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using CsvHelper;

using SetlistHelper.Models;

// Uses CsvHelper
// https://joshclose.github.io/CsvHelper/getting-started/
public class SongManager {
    private readonly string _songsPath;
    private readonly Dictionary<string, Song> _songs;

    public SongManager() {
        _songsPath = Path.Combine(AppContext.BaseDirectory, "./data/songs.csv");
        _songs = [];
        LoadSongs();
    }

    public Song? GetSong(string title) {
        _songs.TryGetValue(title, out Song? song);

        return song;
    }

    public Dictionary<string, Song> GetSongs() {
        return _songs;
    }

    public void Add(Song song) {
        try {
            _songs.Add(song.Title, song);
        } catch (ArgumentException) {
            // TODO: let the user a song with this title is already in the setlist
        }
    }

    public void Update(Song song) {
        _songs[song.Title] = song;
    }

    public void Remove(Song song) {
        _songs.Remove(song.Title);
    }

    private void LoadSongs() {
        using (StreamReader reader = new StreamReader(_songsPath))
        using (CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            IEnumerable<Song> songList = csv.GetRecords<Song>();
            foreach (Song s in songList) {
                _songs.Add(s.Title, s);
            }
        }
    }
}
