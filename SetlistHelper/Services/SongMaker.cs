namespace SetlistHelper.Services;

using System.IO;
using SetlistHelper.Models;

/**
 * SongMaker provides an interactive way to fetch song properties
 * from user input.
 */
public static class SongMaker {
    public static Song MakeSong(string title) {
        if (title == "") {
            title = PromptForTitle();
        }

        int length = PromptForLength();
        int lvl = PromptForDynamicLevel();

        return new Song() {Title=title, DynamicLevel=lvl, Length=length};
    }

    public static string PromptForTitle() {
        Console.WriteLine("What is the song title?");
        string title = Console.ReadLine() ?? "";
        if (title != "") {
            return title;
        }

        Console.WriteLine("Song title is required");
        return PromptForTitle();
    }

    public static int PromptForLength() {
        Console.WriteLine("How long is this song (in minutes)");
        if (int.TryParse(Console.ReadLine(), out int length)) {
            return length;
        }

        Console.WriteLine("Song length must be a whole number (in minutes)");
        return PromptForLength();
    }

    public static int PromptForDynamicLevel() {
        Console.WriteLine("What is the dynamic level of the song between 1-10?");
        if (int.TryParse(Console.ReadLine(), out int lvl)) {
            if (lvl > 0 && lvl <= 10) {
                return lvl;

            }
        }

        Console.WriteLine("Dynamic level must be a whole number between 1 and 10");
        return PromptForLength();
    }
}
