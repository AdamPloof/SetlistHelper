namespace SetlistHelper.Services;

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Globalization;
using CsvHelper;

using SetlistHelper.Models;

// Uses CsvHelper
// https://joshclose.github.io/CsvHelper/getting-started/
public class SongManager : ISongStorage {
    static readonly string SongsPath = Path.Combine(AppContext.BaseDirectory, "./data/songs.csv");

    private readonly Dictionary<string, Song> _songs;

    public SongManager() {
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
            Commit();
            Console.WriteLine($"{song.Title} added.");
        } catch (ArgumentException) {
            // TODO: let the user a song with this title is already in the setlist
            // TODO: check for this explicitly rather than catching as an exception
        }
    }

    public void Update(Song song) {
        _songs[song.Title] = song;
        Commit();
        Console.WriteLine($"{song.Title} updated.");
    }

    public void Remove(string title) {
        if (_songs.Remove(title)) {
            Commit();
            Console.WriteLine($"Removed song: {title}");
        } else {
            Console.WriteLine($"Could not remove song. No song exists for title: {title}");
        }
    }

    public void List() {
        StringBuilder songTable = new StringBuilder("| Title");
        songTable.Append(new string(' ', 34));
        songTable.Append("| Length | Dynamic Level |\n");
        songTable.Append("| ");
        songTable.Append(new string('-', 38));
        songTable.Append(" | ------ | ------------- |\n");

        foreach (Song song in _songs.Values.ToList()) {
            songTable.Append(SongRow(song));
        }

        Console.WriteLine(songTable);
    }

    /**
     * Commit the current song list to CSV
     */
    private void Commit() {
        using (StreamWriter writer = new StreamWriter(SongsPath))
        using (CsvWriter csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(_songs.Values.ToList());
        }
    }

    private void LoadSongs() {
        using (StreamReader reader = new StreamReader(SongsPath))
        using (CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            IEnumerable<Song> songList = csv.GetRecords<Song>();
            foreach (Song s in songList) {
                _songs.Add(s.Title, s);
            }
        }
    }

    private string SongRow(Song song) {
        string row = "| {0, -38} | {1, 6} | {2, 13} |\n";

        return string.Format(row, song.Title, song.Length, song.DynamicLevel);
    }
}
