using System.Collections.Generic;
using Xunit;

using SetlistHelper.Services;
using SetlistHelper.Models;

namespace SetlistHelper.Tests.Services;

public class TemplateManagerTests {
    [Fact]
    public void TemplatesAreLoaded() {
        TemplateManager manager = new TemplateManager();
        Dictionary<string, SetTemplate> templates = manager.GetTemplates();
        Assert.Equal(2, templates.Count);
    }

    [Fact]
    public void TemplatesContainExpectedNames() {
        TemplateManager manager = new TemplateManager();
        Dictionary<string, SetTemplate> templates = manager.GetTemplates();
        Assert.True(templates.TryGetValue("default", out SetTemplate? _));
        Assert.True(templates.TryGetValue("test", out SetTemplate? _));
    }
}
