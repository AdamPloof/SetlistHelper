using SetlistHelper.Services;

namespace SetlistHelper;

static class Program {
    static void Main(string[] args) {
        SongManager manager = new SongManager();
        SetBuilder setBuilder = new SetBuilder(); 
        App app = new App(manager, setBuilder);
        app.ParseArgs(args);
        app.Run();
    }
}

internal class App {
    private readonly SongManager _manager;
    private readonly SetBuilder _setBuilder;

    public App(SongManager manager, SetBuilder setBuilder) {
        _manager = manager;
        _setBuilder = setBuilder;
    }

    public void ParseArgs(string[] args) {
        foreach (string arg in args) {
            Console.WriteLine(arg);
        }
    }

    public void Run() {
        // do some things
    }

    public void Help() {
        string help = """
            SetlistHelper
                Build a set list for the gig based on a dynamic map.

            Usage:
                setlist-helper [options] []
            
            Options:
                --help -h                  Show this message
                --build -b [TEMPLATE_NAME] Build a setlist from the template provided. If a
                                           template is not provided available templates will
                                           be listed. 
                --edit -e song|template    Set the edit mode. Must be combined
                                           with --add, --remove, or --update
                --add -a TITLE             Add a new song/template
                --remove -r TITLE          Remove a song/template
                --update -u TITLE          Update a song/template
            """;

        Console.Write(help);
    }
}
