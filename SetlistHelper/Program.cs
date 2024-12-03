using SetlistHelper.Services;

namespace SetlistHelper;

static class Program {
    static void Main(string[] args) {
        SongManager manager = new SongManager();
        SetBuilder setBuilder = new SetBuilder(); 
        ArgParser parser = new ArgParser(args);
        parser.Parse();
        App app = new App(manager, setBuilder);
        app.Run(parser.GetParsedOptions());
    }
}

internal class App {
    private enum EditModes {
        Song,
        Template
    }

    /**
     * Stores the Action to call for each option
     *
     * Option: the name of option (e.g. --help, --add, etc.)
     * ShouldContinue: Should the application continue after executing the action
     * Action: the method to call for this option 
     */ 
    private struct EditAction {
        public string Option;
        public bool ShouldContinue;
        public Action<string> Editor;
    }

    private readonly SongManager _manager;
    private readonly SetBuilder _setBuilder;
    private readonly List<EditAction> _optionActions;
    private EditModes _editMode = EditModes.Song;

    public App(SongManager manager, SetBuilder setBuilder) {
        _manager = manager;
        _setBuilder = setBuilder;

        // Note: order of options is important -- earlier options take precedence
        _optionActions = new List<EditAction> {
            {new EditAction() {Option="--help",   ShouldContinue=false, Editor=Help}},
            {new EditAction() {Option="--edit",   ShouldContinue=true,  Editor=SetEditMode}},
            {new EditAction() {Option="--add",    ShouldContinue=false, Editor=Add}},
            {new EditAction() {Option="--remove", ShouldContinue=false, Editor=Remove}},
            {new EditAction() {Option="--update", ShouldContinue=false, Editor=Update}},
        };
    }

    public void Run(Dictionary<string, string?> options) {
        // TODO: validation options
        if (options.Count == 0) {
            // TODO: exit with error, no options provided
            return;
        }

        foreach (EditAction e in _optionActions) {
            if (!options.TryGetValue(e.Option, out string? val)) {
                continue;
            }

            e.Editor.Invoke(val ?? "");
            if (!e.ShouldContinue) {
                break;
            }
        }
    }

    public void Build(string templateName) {
        Console.WriteLine($"Building template: {templateName}");
    }

    public void SetEditMode(string mode) {
        if (mode == "song") {
            _editMode = EditModes.Song;
        } else if (mode == "template") {
            _editMode = EditModes.Template;
        } else {
            // TODO: exit with error message instead of exception
            throw new ArgumentException($"Edit mode must be either \"song\" or \"template\". {mode} provided");
        }

        Console.WriteLine($"Setting edit mode: {mode}");
    }

    public void Add(string identifier) {
        if (_editMode == EditModes.Song) {
            AddSong(identifier);
        } else {
            AddTemplate(identifier);
        }
    }

    public void Remove(string identifier) {
        if (_editMode == EditModes.Song) {
            RemoveSong(identifier);
        } else {
            RemoveTemplate(identifier);
        }
    }

    public void Update(string identifier) {
        if (_editMode == EditModes.Song) {
            UpdateSong(identifier);
        } else {
            UpdateTemplate(identifier);
        }
    }

    public void Help(string _ = "") {
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

    private void AddSong(string title) {
        Console.WriteLine($"Adding song {title}...");
    }

    private void RemoveSong(string title) {
        Console.WriteLine($"Removing song {title}...");
    }

    private void UpdateSong(string title) {
        Console.WriteLine($"Updating song {title}...");
    }

    private void AddTemplate(string templateName) {
        Console.WriteLine($"Adding template {templateName}...");
    }

    private void RemoveTemplate(string templateName) {
            Console.WriteLine($"Removing template {templateName}...");
    }

    private void UpdateTemplate(string templateName) {
        Console.WriteLine($"Updating tempated {templateName}...");
    }
}
