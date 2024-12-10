namespace SetlistHelper.Services;

using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;

using SetlistHelper.Models;

/*
 * TemplateManager is responsible for reading/writing Template data
 * from storage.
 */
public class TemplateManager {
    static readonly string TemplatesPath = Path.Combine(AppContext.BaseDirectory, "./data/templates.json");

    private readonly Dictionary<string, SetTemplate> _templates;

    public TemplateManager() {
        _templates = [];
        LoadTemplates();
    }

    public SetTemplate? GetTemplate(string templateName) {
        _templates.TryGetValue(templateName, out SetTemplate? template);

        return template;
    }

    public Dictionary<string, SetTemplate> GetTemplates() {
        return _templates;
    }

    public void Add(SetTemplate template) {
        try {
            _templates.Add(template.Name, template);
            Commit();
            Console.WriteLine($"Added template {template.Name}");
        } catch (ArgumentException) {
            // TODO: let the user a template with this title is already in the setlist
            // TODO: check for this explicitly rather than catching as an exception
        }
    }

    public void Update(SetTemplate template) {
        _templates[template.Name] = template;
        Commit();
    }

    public void Remove(string name) {
        if (_templates.Remove(name)) {
            Commit();
            Console.WriteLine($"Removed template: {name}");
        } else {
            Console.WriteLine($"Could not remove template. No template exists for name: {name}");
        }
    }

    public void List() {
        Console.WriteLine("Templates\n-------");
        foreach (SetTemplate template in _templates.Values.ToList()) {
            Console.WriteLine(template.Name);
        }
    }

    /**
     * Commit the current template list to JSON
     */
    private void Commit() {
        string jsonTemplates = JsonSerializer.Serialize(
            _templates.Values.ToList(),
            new JsonSerializerOptions {
                PropertyNamingPolicy=JsonNamingPolicy.CamelCase,
                WriteIndented=true
        });
        File.WriteAllText(TemplatesPath, jsonTemplates);
    }

    private void LoadTemplates() {
        using FileStream fs = File.OpenRead(TemplatesPath);
        SetTemplate[]? templates = JsonSerializer.Deserialize<SetTemplate[]>(
            fs,
            new JsonSerializerOptions {
                PropertyNamingPolicy=JsonNamingPolicy.CamelCase
            }
        );
        if (templates == null) {
            throw new Exception("Unable to load templates data");
        }

        foreach (SetTemplate t in templates) {
            _templates.Add(t.Name, t);
        }
    }
}