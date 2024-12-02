using System.Collections.Generic;
using Xunit;

using SetlistHelper.Services;

namespace SetlistHelper.Tests.Services;

public class ArgParserTests {
    [Fact]
    public void OptionsContainsAllShortOpts() {
        string[] args = new string[8];
        args[0] = "-h";
        args[1] = "-b";
        args[2] = "-e";
        args[3] = "foo";
        args[4] = "-a";
        args[5] = "-r";
        args[6] = "bar";
        args[7] = "-u";

        string[] expectedOpts = new string[6];
        expectedOpts[0] = "--help";
        expectedOpts[1] = "--build";
        expectedOpts[2] = "--edit";
        expectedOpts[3] = "--add";
        expectedOpts[4] = "--remove";
        expectedOpts[5] = "--update";
        ArgParser parser = new ArgParser(args);
        parser.Parse();
        Dictionary<string, string?> options = parser.GetParsedOptions();
        for (int i = 0; i < expectedOpts.Length; i++) {
            Assert.True(options.TryGetValue(expectedOpts[i], out string? _));
        }
    }

    [Fact]
    public void OptionsContainAllLongOpts() {
        string[] args = new string[8];
        args[0] = "--help";
        args[1] = "--build";
        args[2] = "foo";
        args[3] = "--edit";
        args[4] = "--add";
        args[5] = "--remove";
        args[6] = "bar";
        args[7] = "--update";

        string[] expectedOpts = new string[6];
        expectedOpts[0] = "--help";
        expectedOpts[1] = "--build";
        expectedOpts[2] = "--edit";
        expectedOpts[3] = "--add";
        expectedOpts[4] = "--remove";
        expectedOpts[5] = "--update";
        ArgParser parser = new ArgParser(expectedOpts);
        parser.Parse();
        Dictionary<string, string?> options = parser.GetParsedOptions();
        for (int i = 0; i < expectedOpts.Length; i++) {
            Assert.True(options.TryGetValue(expectedOpts[i], out string? _));
        }
    }

    [Fact]
    public void OptionsContainAllMixedOpts() {
        string[] args = new string[8];
        args[0] = "--help";
        args[1] = "-b";
        args[2] = "foo";
        args[3] = "--edit";
        args[4] = "-a";
        args[5] = "-r";
        args[6] = "bar";
        args[7] = "--update";

        string[] expectedOpts = new string[6];
        expectedOpts[0] = "--help";
        expectedOpts[1] = "--build";
        expectedOpts[2] = "--edit";
        expectedOpts[3] = "--add";
        expectedOpts[4] = "--remove";
        expectedOpts[5] = "--update";
        ArgParser parser = new ArgParser(args);
        parser.Parse();
        Dictionary<string, string?> options = parser.GetParsedOptions();
        for (int i = 0; i < expectedOpts.Length; i++) {
            Assert.True(options.TryGetValue(expectedOpts[i], out string? _));
        }
    }

    [Fact]
    public void OptionsContainCorrectValues() {
        string[] args = new string[8];
        args[0] = "--help";
        args[1] = "-b";
        args[2] = "foo";
        args[3] = "--edit";
        args[4] = "-a";
        args[5] = "-r";
        args[6] = "bar";
        args[7] = "--update";

        Dictionary<string, string?> expectedOpts = new Dictionary<string, string?> {
            {"--help", null},
            {"--build", "foo"},
            {"--edit", null},
            {"--add", null},
            {"--remove", "bar"},
            {"--update", null},
        };

        ArgParser parser = new ArgParser(args);
        parser.Parse();
        Dictionary<string, string?> options = parser.GetParsedOptions();
        foreach (KeyValuePair<string, string?> kvp in options) {
            Assert.True(expectedOpts.TryGetValue(kvp.Key, out string? actual));
            Assert.Equal(actual, kvp.Value);
        }
    }

    [Fact]
    public void OptionsAreEmpty() {
        string[] args = [];
        ArgParser parser = new ArgParser(args);
        Dictionary<string, string?> options = parser.GetParsedOptions();
        Assert.Empty(options);
    }

    [Fact]
    public void InvalidOptionsAreDiscarded() {
        string[] args = new string[8];
        args[0] = "--help";
        args[1] = "-b";
        args[2] = "foo";
        args[3] = "baz";
        args[4] = "bim";
        args[5] = "bam";
        args[6] = "--update";
        args[7] = "bar";

        Dictionary<string, string?> expectedOpts = new Dictionary<string, string?> {
            {"--help", null},
            {"--build", "foo"},
            {"--update", "bar"},
        };

        ArgParser parser = new ArgParser(args);
        parser.Parse();
        Dictionary<string, string?> options = parser.GetParsedOptions();
        foreach (KeyValuePair<string, string?> kvp in options) {
            Assert.True(expectedOpts.TryGetValue(kvp.Key, out string? actual));
            Assert.Equal(actual, kvp.Value);
        }
    }
}
