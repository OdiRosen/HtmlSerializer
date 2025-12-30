using System.Text.RegularExpressions;

namespace HtmlSerializer;

public class Selector
{
    public string TagName { get; set; }
    public string Id { get; set; }
    public List<string> Classes { get; set; } = new List<string>();
    public Selector Parent { get; set; }
    public Selector Child { get; set; }

    public static Selector FromQuery(string query)
    {
        var parts = query.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        Selector root = null;
        Selector current = null;

        foreach (var part in parts)
        {
            var newSelector = new Selector();
            // פירוק לפי מחלקות ו-IDs בעזרת Regex חכם
            var components = Regex.Split(part, "(?=[#.])");

            foreach (var comp in components)
            {
                if (comp.StartsWith("#")) newSelector.Id = comp.Substring(1);
                else if (comp.StartsWith(".")) newSelector.Classes.Add(comp.Substring(1));
                else if (!string.IsNullOrEmpty(comp) && HtmlHelper.Instance.AllTags.Contains(comp))
                    newSelector.TagName = comp;
            }

            if (root == null) root = newSelector;
            if (current != null) { current.Child = newSelector; newSelector.Parent = current; }
            current = newSelector;
        }
        return root;
    }
}