using System.Text.RegularExpressions;
using HtmlSerializer;

// טעינת תוכן ה-HTML בצורה אסינכרונית
var html = await Load("https://hebrewbooks.org/beis");

// ניקוי רווחים לבנים כפולים לשמירה על מבנה תקין
var cleanHtml = new Regex("\\s+").Replace(html, " ");

// פירוק המחרוזת לרשימת תגיות וטקסט באמצעות Regex
var htmlLines = new Regex("<(.*?)>").Split(cleanHtml).Where(s => s.Length > 0).ToList();

// אתחול שורש העץ ואלמנט נוכחי לסריקה
HtmlElement root = new HtmlElement { Name = "root" };
HtmlElement currentElement = root;

foreach (var line in htmlLines)
{
    var trimmedLine = line.Trim();
    var firstWord = trimmedLine.Split(' ')[0];

    // דילוג על הערות ותגיות מערכת
    if (firstWord.StartsWith("!")) continue;

    if (firstWord.Equals("/html", StringComparison.OrdinalIgnoreCase)) break; // סיום סריקה

    if (firstWord.StartsWith("/")) // תגית סוגרת - עליה ברמת העץ
    {
        currentElement = currentElement.Parent ?? currentElement;
    }
    else if (HtmlHelper.Instance.AllTags.Contains(firstWord)) // תגית פותחת מוכרת
    {
        var newElement = new HtmlElement { Name = firstWord, Parent = currentElement };

        // חילוץ מאפיינים (Attributes) באמצעות Regex
        var attributes = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(trimmedLine);
        foreach (Match attr in attributes)
        {
            var name = attr.Groups[1].Value;
            var value = attr.Groups[2].Value;

            if (name.ToLower() == "id") newElement.Id = value;
            else if (name.ToLower() == "class")
                newElement.Classes.AddRange(value.Split(' ', StringSplitOptions.RemoveEmptyEntries));
            else
                newElement.Attributes[name] = value;
        }

        currentElement.Children.Add(newElement);

        // (Self-Closing בדיקה אם התגית דורשת סגירה (אינה
        if (!trimmedLine.EndsWith("/") && !HtmlHelper.Instance.SelfClosingTags.Contains(firstWord))
            currentElement = newElement;
    }
    else // טקסט פנימי (InnerHtml)
    {
        currentElement.InnerHtml += " " + trimmedLine;
    }
}

// שלב השאילתה: חיפוש אלמנטים בעץ לפי סלקטור
Console.WriteLine("--- Search Results ---");
string query = "div";
var selector = Selector.FromQuery(query);
var results = root.FindBySelector(selector);

Console.WriteLine($"Query: {query} | Found: {results.Count} elements.");

foreach (var item in results.Take(5))
{
    Console.WriteLine($"Element: <{item.Name}> | ID: {item.Id}");
}

Console.ReadLine(); // השהיית המסך לצפייה בתוצאות

// פונקציית עזר לטעינת דף אינטרנט
async Task<string> Load(string url)
{
    using HttpClient client = new HttpClient();
    var response = await client.GetAsync(url);
    return await response.Content.ReadAsStringAsync();
}