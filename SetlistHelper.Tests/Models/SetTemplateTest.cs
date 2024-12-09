using System.Collections.Generic;
using Xunit;

using SetlistHelper.Models;

namespace SetlistHelper.Tests.Models;

public class SetTemplateTests {
    [Fact]
    public void EmptyTemplateDefault() {
        SetTemplate t = new SetTemplate() {Name="Foo"};
        Assert.Empty(t.DynamicPlot);
    }

    [Fact]
    public void DynamicPlotIsSet() {
        List<int> plot = new List<int>() {1, 2, 3, 4, 5};
        SetTemplate t = new SetTemplate("Foo") {DynamicPlot=plot};
        Assert.True(plot.Equals(t.DynamicPlot));
    }

    [Fact]
    public void NameIsSet() {
        SetTemplate t = new SetTemplate("Foo");
        Assert.Equal("Foo", t.Name);
    }
}
