namespace SetlistHelper;

using System.IO;

public class Song {
    public required string Title { get; set; }
    public int DynamicLevel { get; set; }
    public required int Length { get; set; } // in minutes

    public void Print() {
        Console.WriteLine($"Song: {Title}");
        Console.WriteLine($"Dynamic level: {DynamicLevel}");
        Console.WriteLine($"Length: {Length}");
    }
}
