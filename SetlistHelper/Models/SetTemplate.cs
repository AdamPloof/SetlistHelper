namespace SetlistHelper.Models;

using System.IO;
using System.Collections.Generic;

using SetlistHelper.Services;

/// <summary>
/// SetTemplate stores the name and dynamic steps of a set.
/// </summary
/// 
/// <example>
/// Here's the plot for a set that should start mellow, gradually rise,
/// slightly dip around 3/4 of the way through the set and crescendos
/// to the finish:
/// 
/// <c>[2, 3, 4, 4, 5, 7, 3, 8, 10]</c>
/// </example>
public class SetTemplate {
    public required string Name { get; set; }
    public List<int> DynamicPlot {
        get { return _dynamicPlot; }
        set { _dynamicPlot = value; }
    }

    private List<int> _dynamicPlot = new List<int>();

    public SetTemplate() {}

    [System.Diagnostics.CodeAnalysis.SetsRequiredMembers]
    public SetTemplate(string name) {
        Name = name;
    }

    public void AddStep(int lvl) {
        _dynamicPlot.Add(lvl);
    }

    public void RemoveStep(int stepIdx) {
        _dynamicPlot.RemoveAt(stepIdx);
    }

    public void SetStep(int stepIdx, int lvl) {
        if (lvl <= 0 || lvl > 10) {
            throw new ArgumentException($"Level must be between 1-10. {lvl} provided");
        }

        if (stepIdx < 0 || stepIdx >= _dynamicPlot.Count) {
            throw new ArgumentException($"Step index must already be set before updating. {stepIdx} provided");
        }

        _dynamicPlot[stepIdx] = lvl;
    }

    public void Print() {
        Console.WriteLine($"Template: {Name}");
        DynamicsGrapher.DrawGraph(_dynamicPlot);
    }
}
