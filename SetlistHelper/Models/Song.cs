namespace SetlistHelper;

using System.IO;

/// <summary>
/// A song, the stuff you play during your set.
/// </summary>
public class Song {
    private int _dynamicLevel;

    /// <summary>
    /// The title of the song
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    /// DynamicLevel is how intense the song is. 
    /// </summary>
    public int DynamicLevel {
        get => _dynamicLevel;
        set {
            if (value < 1 || value > 10) {
                throw new ArgumentException("Dynamic level must be between 0-10");
            }

            _dynamicLevel = value;
        }
    }

    /// <summary>
    /// The length of the song in minutes
    /// </summary>
    public required int Length { get; set; }

    public void Print() {
        Console.WriteLine($"Song: {Title}");
        Console.WriteLine($"Dynamic level: {DynamicLevel}");
        Console.WriteLine($"Length: {Length}");
    }
}
