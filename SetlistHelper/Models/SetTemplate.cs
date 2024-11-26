namespace SetlistHelper.Models;

using System.Collections.Generic;

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
