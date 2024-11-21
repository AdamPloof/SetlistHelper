namespace SetlistHelper.Services;

using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using CsvHelper;

using SetlistHelper.Models;

// Uses CsvHelper
// https://joshclose.github.io/CsvHelper/getting-started/
class SongManager {
    private string _songsPath;
    private readonly Dictionary<string, Song> _songs;

    public SongManager() {
        _songsPath = Path.Combine(Environment.CurrentDirectory, "../data/songs.csv");
        _songs = [];
        loadSongs();
    }

    public Song? getSong(string title) {
        _songs.TryGetValue(title, out Song? song);

        return song;
    }

    public void add(Song song) {

    }

    public void update(Song song) {
        
    }

    public void remove(Song song) {
        
    }

    private void loadSongs() {
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
