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
    private List<int> _dynamicPlot = [];

    public List<int> GetDynamicPlot() {
        return _dynamicPlot;
    }

    public void SetDynamicPlot(List<int> plot) {
        _dynamicPlot = plot;
    }

    public void AddStep(int step) {
        _dynamicPlot.Add(step);
    }

    public void RemoveStep(int stepIdx) {
        _dynamicPlot.RemoveAt(stepIdx);
    }
}
