namespace SetlistHelper.Services;

using System.Collections.Generic;
using SetlistHelper.Models;

public interface ISongStorage {
    /// <summary>
    /// Retrieve a Song by title
    /// </summary>
    /// <param name="title">The song title</param>
    /// <returns>Song if found; otherwise, null</returns>
    public Song? GetSong(string title);

    /// <summary>
    /// Returns all songs managed by the ISongStorage implementation 
    /// </summary>
    /// <returns>Dictionary of Songs with the keys being the song titles</returns>
    public Dictionary<string, Song> GetSongs();

    /// <summary>
    /// Add a new song to storage
    /// </summary>
    /// <param name="song">The song</param>
    public void Add(Song song);

    /// <summary>
    /// Update the song in storage
    /// </summary>
    /// <param name="song">The song</param>
    public void Update(Song song);

    /// <summary>
    /// Remove the song from storage by title.
    /// </summary>
    /// <param name="title">The song title</param>
    public void Remove(string title);

    /// <summary>
    /// Output a song list for displaying to the user
    /// </summary>
    public void List();
}
