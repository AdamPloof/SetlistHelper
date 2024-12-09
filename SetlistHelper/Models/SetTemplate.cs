namespace SetlistHelper.Models;

using System.Collections.Generic;

/**
 * SetTemplate stores the dynamic steps of a set. For instance, a set
 * that should start mellow, gradually rise, slightly dip around 3/4 of the
 * way through the set and crescendo to the finish could be represented by
 * the dynamic plot:
 *
 *  [2, 3, 4, 4, 5, 7, 3, 8, 10]
 */
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

    public void AddStep(int step) {
        _dynamicPlot.Add(step);
    }

    public void RemoveStep(int stepIdx) {
        _dynamicPlot.RemoveAt(stepIdx);
    }
}
