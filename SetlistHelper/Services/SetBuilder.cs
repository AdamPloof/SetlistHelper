using SetlistHelper.Models;

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
    private Dictionary<int, List<Song>> _notPlayed;

    /// <value>Property <c>_played</c> The songs in the repertoire that have been used</value>
    /// keys in the dict are the dynamic levels of the song
    private Dictionary<int, List<Song>> _played;
    
    public SetBuilder(SongManager songManager) {
        _songManager = songManager;
        _notPlayed = new Dictionary<int, List<Song>>();
        _played = new Dictionary<int, List<Song>>();

        for (int i = 1; i <= 10; i++) {
            _notPlayed[i] = new List<Song>();
            _played[i] = new List<Song>();
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
        LoadSongs();
        Setlist setlist = new Setlist(name == "" ? GetDefaultSetName() : name);
        foreach (int lvl in template.DynamicPlot) {
            AddSongToSet(lvl, ref setlist);
        }

        return setlist;
    }

    private void LoadSongs() {
        List<Song> songs = _songManager.GetSongs().Values.ToList();
        foreach (Song song in songs) {
            _notPlayed[song.DynamicLevel].Add(song);
        }
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
        
    }

    private string GetDefaultSetName() {
        // TODO: make default name based on date
        return "Default set";
    }
}
