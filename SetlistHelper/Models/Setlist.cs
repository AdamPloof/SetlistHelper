using System.Collections.Generic;
using System.IO;

namespace SetlistHelper.Models;

/// <summary>
/// A container for a set list of songs
/// </summary>
public class Setlist {
    public required string Name { get; set; }
    public List<Song> Songs {
        get => _songs;
        set => _songs = value;
    }

    private List<Song> _songs = new List<Song>();

    [System.Diagnostics.CodeAnalysis.SetsRequiredMembers]
    public Setlist(string name) {
        Name = name;
    }

    public void AddSong(Song song) {
        _songs.Add(song);
    }

    public void Print() {
        Console.WriteLine($"Set: {Name}");
        Console.WriteLine("-------------");
        foreach (Song song in _songs) {
            Console.WriteLine(song.Title);
        }
    }
}
