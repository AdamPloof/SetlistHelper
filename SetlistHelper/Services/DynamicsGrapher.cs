namespace SetlistHelper.Services;

using System.Collections.Generic;
using System.Text;
using SetlistHelper.Models;

/**
 * DynamicsGrapher draws a graph of the the dynamic plot of a SetTemplate
 */
public static class DynamicsGrapher {
    public static void DrawGraph(SetTemplate template) {
        DrawGraph(template.GetDynamicPlot());
    }

    public static void DrawGraph(List<int> dynamicPlot) {
        int ceiling = dynamicPlot.Max();
        int stepCount = dynamicPlot.Count;
        int graphWidth = (stepCount * 2) + stepCount;
        StringBuilder graph = new StringBuilder();
        for (int key = 10; key > 0; key--) {
            graph.Append($"{key} ");
            if (key > ceiling) {
                graph.Append(new string(' ', graphWidth));
            } else {
                foreach (int lvl in dynamicPlot) {
                    if (lvl >= key) {
                        graph.Append(" ##");
                    } else {
                        graph.Append("   ");
                    }
                }
            }

            graph.Append('\n');
        }

        graph.Append('-', graphWidth + 2);
        graph.Append('\n');
        graph.Append("   ");

        for (int step = 1; step <= stepCount; step++) {
            graph.Append($"{step}  ");
        }

        Console.WriteLine(graph);
    }
}
