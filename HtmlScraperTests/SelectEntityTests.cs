using HtmlAgilityPack;
using HtmlScraperLibrary.Entities;
using System.Text.Encodings.Web;
using System.Text.Json;
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
        var jsonTxt = JsonSerializer.Serialize(json, new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            WriteIndented = true
        });

        // Assert - Results
        Assert.True(json.ContainsKey("results"));
        var results = json["results"] as JsonArray;
        Assert.NotNull(results);
        Assert.Single(results); // Il y a un seul élément (un tableau imbriqué)

        var resultsArray = results[0] as JsonArray;
        Assert.NotNull(resultsArray);
        Assert.Equal(6, resultsArray.Count); // 3 titres + 3 descriptions

        Assert.Equal(">> Titre 1 <<", resultsArray[0]?.ToString());
        Assert.Equal("Description 1", resultsArray[1]?["description"]?.ToString());
        Assert.Equal(">> Titre 2 <<", resultsArray[2]?.ToString());
        Assert.Equal("Description 2", resultsArray[3]?["description"]?.ToString());
        Assert.Equal(">> Titre 3 <<", resultsArray[4]?.ToString());
        Assert.Equal("Description 3", resultsArray[5]?["description"]?.ToString());

        // Assert - List
        Assert.True(json.ContainsKey("lst"));
        var lst = json["lst"] as JsonArray;
        Assert.NotNull(lst);
        Assert.Equal(5, lst.Count);
        Assert.Equal("Item: TEST1;", lst[0]?.ToString());
        Assert.Equal("Item: TEST2;", lst[1]?.ToString());
        Assert.Equal("Item: TEST3;", lst[2]?.ToString());
        Assert.Equal("Item: TEST4;", lst[3]?.ToString());
        Assert.Equal("Item: TEST5;", lst[4]?.ToString());

        // Assert - Images
        Assert.True(json.ContainsKey("images"));
        var images = json["images"] as JsonArray;
        Assert.NotNull(images);
        Assert.Equal(2, images.Count);

        var img1 = images[0] as JsonObject;
        Assert.NotNull(img1);
        Assert.Equal("img1.png", img1["src"]?.ToString());
        Assert.Equal("Image 1", img1["alt"]?.ToString());
        Assert.Equal("101", img1["id"]?.ToString());

        var img2 = images[1] as JsonObject;
        Assert.NotNull(img2);
        Assert.Equal("img2.png", img2["src"]?.ToString());
        Assert.Equal("Image 2", img2["alt"]?.ToString());
        Assert.Equal("102", img2["id"]?.ToString());

        // Assert - Empty
        Assert.True(json.ContainsKey("empty"));
        var empty = json["empty"] as JsonNode;
        Assert.NotNull(empty);
        Assert.Equal("EmptyData", empty);
    }
}