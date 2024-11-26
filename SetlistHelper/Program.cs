using System.IO;

using SetlistHelper;
using SetlistHelper.Services;

SongManager manager = new SongManager();
Song? song = manager.GetSong("Yesterdays");

if (song is null) {
    Console.WriteLine("Song not found");
} else {
    Console.WriteLine(song.Title);
}
