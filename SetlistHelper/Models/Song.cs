namespace SetlistHelper;

public class Song {
    public required string Title { get; set; }
    public int DynamicLevel { get; set; }
    public required int Length { get; set; } // in minutes
}
