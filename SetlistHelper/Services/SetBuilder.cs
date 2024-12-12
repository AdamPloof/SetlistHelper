using SetlistHelper.Models;
using SetlistHelper.Extensions;

namespace SetlistHelper.Services;

/// <summary>
/// SetBuilder takes in a set template and outputs a setlist made up
/// of songs available in the repertoire
/// </summary>
public class SetBuilder {
    /// <summary>
    /// The maximum dynamic level for songs in the repertoire. If one day this were to be
    /// dynamic, then probably better to make this a globally accessible config param.
    /// </summary>
    public static int MaxDynamicLevel = 10;

    private SongManager _songManager;

    /// <value>Property <c>_notPlayed</c> The songs in the repertoire not used in the set yet</value>
    /// keys in the dict are the dynamic levels of the song
    private Dictionary<int, Stack<Song>> _notPlayed;

    /// <value>Property <c>_played</c> The songs in the repertoire that have been used</value>
    /// keys in the dict are the dynamic levels of the song
    private List<Song> _played;
    
    public SetBuilder(SongManager songManager) {
        _songManager = songManager;
        _notPlayed = new Dictionary<int, Stack<Song>>();
        _played = new List<Song>();

        for (int i = 1; i <= 10; i++) {
            _notPlayed[i] = new Stack<Song>();
        }
    }

    /// <summary>
    /// Build a set list from the provided template
    /// </summary>
    /// 
    /// <param name="template">The template to base the setlist on</param>
    /// <param name="name">The name for the set</param>
    /// <returns>the Setlist</returns>
    public Setlist Build(SetTemplate template, string name) {
        if (LoadSongs() == 0) {
            throw new Exception("Could not build setlist. There are no songs in the repertoire.");
        }

        Setlist setlist = new Setlist(name == "" ? GetDefaultSetName() : name);
        foreach (int lvl in template.DynamicPlot) {
            AddSongToSet(lvl, ref setlist);
        }

        return setlist;
    }

    /// <summary>
    /// Load songs from data source. Shuffle list of songs to ensure there's
    /// some randomness to the setlists.
    /// </summary>
    /// <returns>the number of songs loaded</returns>
    private int LoadSongs() {
        List<Song> songs = _songManager.GetSongs().Values.ToList();
        songs.Shuffle();
        foreach (Song song in songs) {
            _notPlayed[song.DynamicLevel].Push(song);
        }

        return songs.Count;
    }

    /// <summary>
    /// Find a song in _notPlayed with the closest dynamicLevel to the given level. If _notPlayed
    /// has been exhausted then put all songs in _played back in _notPlayed and play them again.
    /// 
    /// If more than one song are available for the given dynamic level, choose randomly between them.
    /// </summary>
    /// <param name="dynamicLvl"></param>
    /// <param name="setlist"></param>
    private void AddSongToSet(int dynamicLvl, ref Setlist setlist) {
        if (_notPlayed[dynamicLvl].Count > 0) {
            Song song = _notPlayed[dynamicLvl].Pop();
            setlist.AddSong(song);
            _played.Add(song);

            return;
        }

        int high = dynamicLvl < 10 ? dynamicLvl + 1 : 10;
        int low = dynamicLvl > 1 ? dynamicLvl - 1 : 1;
        int closest = -1;
        while (high <= 10 && low > 0 && closest == -1) {
            // TODO: randomly set closest to high or low if both are valid
            if (high <= 10 && _notPlayed[high].Count > 0) {
                closest = high;
            } else if (low > 0 && _notPlayed[low].Count > 0) {
                closest = low;
            } else {
                high++;
                low--;
            }
        }

        if (closest > 0) {
            Song song = _notPlayed[closest].Pop();
            setlist.AddSong(song);
            _played.Add(song);
        } else {
            // There were no songs left in the _notPlayed list, time to reshuffle the deck
            _played.Shuffle();
            foreach (Song playedSong in _played) {
                _notPlayed[playedSong.DynamicLevel].Push(playedSong);
            }

            _played = new List<Song>();
        }
    }

    private string GetDefaultSetName() {
        // TODO: make default name based on date
        return "Default set";
    }
}
