using System.Text.Json;

namespace HtmlSerializer;

public class HtmlHelper
{
    // מימוש Singleton - מופע יחיד לכל האפליקציה
    private readonly static HtmlHelper _instance = new HtmlHelper();
    public static HtmlHelper Instance => _instance;

    public string[] AllTags { get; private set; }
    public string[] SelfClosingTags { get; private set; }

    private HtmlHelper()
    {
        var allTagsJson = File.ReadAllText("JSON Files/HtmlTags.json");
        var selfClosingTagsJson = File.ReadAllText("JSON Files/HtmlVoidTags.json");

        AllTags = JsonSerializer.Deserialize<string[]>(allTagsJson);
        SelfClosingTags = JsonSerializer.Deserialize<string[]>(selfClosingTagsJson);
    }
}