namespace SetlistHelper.Services;

using SetlistHelper.Models;

/**
 * TemplateMaker walks the user through creating SetTemplates
 */
public static class TemplateMaker {
    public static SetTemplate MakeTemplate(string name) {
        if (name == "") {
            name = PromptForName();
        }

        SetTemplate template = new SetTemplate(name);
        PromptForSteps(ref template);
        if (template.DynamicPlot.Count == 0) {
            Console.WriteLine("Dynamic plot cannot be empty");
            PromptForSteps(ref template);
        }

        return template;
    }

    public static string PromptForName() {
        Console.WriteLine("What is the template name?");
        string name = Console.ReadLine() ?? "";
        if (name != "") {
            return name;
        }

        Console.WriteLine("Template name is required");
        return PromptForName();
    }

    public static void PromptForSteps(ref SetTemplate template, int stepCount = 1) {
        Console.WriteLine($"Enter song {stepCount} dynamic level (1-10): ");
        while (true) {
            string input = Console.ReadLine() ?? "";
            if (input == "" && template.DynamicPlot.Count > 0) {
                break;
            }

            if (!int.TryParse(input, out int lvl) || lvl > 10 || lvl < 1) {
                Console.WriteLine("Dynamic level must be a whole number between 1 and 10");
                PromptForSteps(ref template, stepCount);
            }

            template.AddStep(lvl);
            stepCount++;
            Console.WriteLine($"Template {template.Name} dynamics:");
            DynamicsGrapher.DrawGraph(template);
            Console.WriteLine();

            Console.WriteLine("Next step (press enter to stop adding steps): ");
        }
    }
}
