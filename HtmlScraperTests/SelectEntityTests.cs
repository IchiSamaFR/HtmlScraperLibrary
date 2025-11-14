using HtmlAgilityPack;
using HtmlScraperLibrary.Entities;
using System.Text.Json.Nodes;
using System.Xml.Linq;

public class SelectEntityTests
{
    [Fact]
    public async Task Extract_GoogleHtml_ReturnsExpectedJson()
    {
        // Arrange
        var document = new HtmlDocument();
        document.LoadHtml(File.ReadAllText("Configs/google.html"));

        var html = File.ReadAllText("Configs/googleConfig.xml");
        var xdoc = XDocument.Parse(html);
        var rootElement = xdoc.Root;

        var rootEntity = new RootEntity(rootElement);

        // Act
        var json = await rootEntity.Extract(document);
        var jsonTxt = json.ToString();

        // Assert
        Assert.True(json.ContainsKey("results"));
        var results = json["results"] as JsonArray;
        Assert.NotNull(results);
        Assert.Equal(2, results.Count);
        Assert.Equal("Title 1", results[0]["title"].ToString());
        Assert.Equal("Description 1", results[0]["description"].ToString());
        Assert.Equal("Title 2", results[1]["title"].ToString());
        Assert.Equal("Description 2", results[1]["description"].ToString());
    }
}