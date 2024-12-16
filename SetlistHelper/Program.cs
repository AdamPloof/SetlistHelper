using SetlistHelper.Services;
using SetlistHelper.Models;

namespace SetlistHelper;

static class Program {
    static void Main(string[] args) {
        SongManager songManager = new SongManager();
        TemplateManager templateManager = new TemplateManager();
        SetBuilder setBuilder = new SetBuilder(songManager);
        ArgParser parser = new ArgParser(args);
        parser.Parse();
        App app = new App(songManager, templateManager, setBuilder);
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

    private readonly SongManager _songManager;
    private readonly TemplateManager _templateManager;
    private readonly SetBuilder _setBuilder;
    private readonly List<EditAction> _optionActions;
    private EditModes _editMode = EditModes.Song;

    public App(SongManager songManager, TemplateManager templateManager, SetBuilder setBuilder) {
        _songManager = songManager;
        _templateManager = templateManager;
        _setBuilder = setBuilder;

        // Note: order of options is important -- earlier options take precedence
        _optionActions = new List<EditAction> {
            {new EditAction() {Option="--help",   ShouldContinue=false, Editor=Help}},
            {new EditAction() {Option="--build",  ShouldContinue=false, Editor=Build}},
            {new EditAction() {Option="--mode",   ShouldContinue=true,  Editor=SetEditMode}},
            {new EditAction() {Option="--add",    ShouldContinue=false, Editor=Add}},
            {new EditAction() {Option="--remove", ShouldContinue=false, Editor=Remove}},
            {new EditAction() {Option="--update", ShouldContinue=false, Editor=Update}},
            {new EditAction() {Option="--list",   ShouldContinue=false, Editor=List}},
            {new EditAction() {Option="--show",   ShouldContinue=false, Editor=Show}},
        };
    }

    public void Run(Dictionary<string, string?> options) {
        if (options.Count == 0) {
            Console.Write("No options provided. Exiting.");
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
        SetTemplate? template = _templateManager.GetTemplate(templateName);
        if (template == null) {
            Console.WriteLine($"Could not build setlist. No template found for {templateName}");
            return;
        }

        Setlist setlist = _setBuilder.Build(template, GetSetName());
        setlist.Print();
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

        Console.WriteLine($"Mode: {mode}");
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

    public void List(string type = "") {
        if (type == "template") {
            ListTemplates();
        } else {
            ListSongs();
        }
    }

    public void Show(string identifier) {
        if (_editMode == EditModes.Song) {
            ShowSong(identifier);
        } else {
            ShowTemplate(identifier);
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
                --mode -m [song|template]  Set the edit mode. This affects whether other actions
                                           such as add, list, etc. apply to songs or templates
                --add -a TITLE             Add a new song/template
                --remove -r TITLE          Remove a song/template
                --update -u TITLE          Update a song/template
                --list -l [song|template]  List all songs or templates, default=song
            """;

        Console.Write(help);
    }

    private void AddSong(string title) {
        Song song = SongMaker.MakeSong(title);
        _songManager.Add(song);
    }

    private void RemoveSong(string title) {
        _songManager.Remove(title);
    }

    private void UpdateSong(string title) {
        Song? song = _songManager.GetSong(title);
        if (song == null) {
            Console.WriteLine($"Could not find a song for title: {title}");
            Console.WriteLine("Would you like to add it as a new song?");

            string addNew = Console.ReadLine() ?? "n";
            addNew = addNew.ToLower();
            if (addNew == "y" || addNew == "yes") {
                AddSong(title);
            }

            return;
        }

        Console.WriteLine("-- Updating song --");
        song.Print();
        Console.WriteLine();

        string oldTitle = song.Title;
        Console.WriteLine("Change title?");
        string changeTitle = Console.ReadLine() ?? "n";
        changeTitle = changeTitle.ToLower();
        if (changeTitle == "y" || changeTitle == "yes") {
            song.Title = SongMaker.PromptForTitle();
        }

        song.Length = SongMaker.PromptForLength();
        song.DynamicLevel = SongMaker.PromptForDynamicLevel();

        if (oldTitle == song.Title) {
            _songManager.Update(song);
        } else {
            _songManager.Remove(oldTitle);
            _songManager.Add(song);
        }
    }

    private void ListSongs() {
        _songManager.List();
    }

    private void ShowSong(string title) {
        Song? song = _songManager.GetSong(title);
        if (song == null) {
            Console.WriteLine($"Could not find a song for title: {title}");
            return;
        }

        song.Print();
    }

    private void AddTemplate(string templateName) {
        SetTemplate template = TemplateMaker.MakeTemplate(templateName);
        _templateManager.Add(template);
    }

    private void RemoveTemplate(string templateName) {
        _templateManager.Remove(templateName);
    }

    private void UpdateTemplate(string templateName) {
        SetTemplate? template = _templateManager.GetTemplate(templateName);
        if (template == null) {
            Console.WriteLine($"Could not find a template for name: {templateName}");
            Console.WriteLine("Would you like to add it as a new template?");

            string addNew = Console.ReadLine() ?? "n";
            addNew = addNew.ToLower();
            if (addNew == "y" || addNew == "yes") {
                AddTemplate(templateName);
            }

            return;
        }

        Console.WriteLine("-- Updating template --");
        template.Print();
        Console.WriteLine();

        string oldName = template.Name;
        Console.WriteLine("Change name?");
        string changeName = Console.ReadLine() ?? "n";
        changeName = changeName.ToLower();
        if (changeName == "y" || changeName == "yes") {
            template.Name = TemplateMaker.PromptForName();
        }

        Console.WriteLine();

        int stepIdx = 0;
        foreach (int lvl in template.DynamicPlot.ToArray()) {
            Console.WriteLine($"Song # {stepIdx} level: {lvl}");
            template.SetStep(stepIdx, TemplateMaker.PromptForDynamicLevel(lvl));
            stepIdx++;
        }

        Console.WriteLine("Additional steps (press enter to stop adding steps)...");
        TemplateMaker.PromptForSteps(ref template, stepIdx);

        if (oldName == template.Name) {
            _templateManager.Update(template);
        } else {
            _templateManager.Remove(oldName);
            _templateManager.Add(template);
        }
    }

    private void ListTemplates() {
        _templateManager.List();
    }

    private void ShowTemplate(string templateName) {
        SetTemplate? template = _templateManager.GetTemplate(templateName);
        if (template == null) {
            Console.WriteLine($"Could not find template for name: {templateName}");
            return;
        }

        template.Print();
    }

    private string GetSetName() {
        Console.WriteLine("Enter set name:");
        string? name = Console.ReadLine();
        if (name == null) {
            Console.WriteLine("Set name cannot be empty");
            name = GetSetName();
        }

        return name;
    }
}
