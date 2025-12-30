namespace HtmlSerializer;

public class HtmlElement
{
    public string Id { get; set; }
    public string Name { get; set; }
    public Dictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();
    public List<string> Classes { get; set; } = new List<string>();
    public string InnerHtml { get; set; } = "";
    public HtmlElement Parent { get; set; }
    public List<HtmlElement> Children { get; set; } = new List<HtmlElement>();

    // מחזירה את כל הצאצאים - שימוש ב-Queue לייעול הזיכרון (BFS)
    public IEnumerable<HtmlElement> Descendants()
    {
        Queue<HtmlElement> queue = new Queue<HtmlElement>();
        queue.Enqueue(this);
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            yield return current;
            foreach (var child in current.Children) queue.Enqueue(child);
        }
    }

    // מחזירה את כל האבות (Ancestors) של האלמנט
    public IEnumerable<HtmlElement> Ancestors()
    {
        var current = this.Parent;
        while (current != null)
        {
            yield return current;
            current = current.Parent;
        }
    }

    // מנוע החיפוש המשלב ריקורסיה על עץ האלמנטים מול עץ הסלקטורים
    public HashSet<HtmlElement> FindBySelector(Selector selector)
    {
        HashSet<HtmlElement> results = new HashSet<HtmlElement>();
        FindElementsRecursive(this, selector, results);
        return results;
    }

    private void FindElementsRecursive(HtmlElement current, Selector selector, HashSet<HtmlElement> results)
    {
        var descendants = current.Descendants().Where(e => e != current);
        var matches = descendants.Where(e => IsMatch(e, selector));

        if (selector.Child != null)
        {
            foreach (var match in matches)
                FindElementsRecursive(match, selector.Child, results);
        }
        else
        {
            foreach (var match in matches) results.Add(match);
        }
    }

    private bool IsMatch(HtmlElement element, Selector selector)
    {
        if (selector.TagName != null && element.Name != selector.TagName) return false;
        if (selector.Id != null && element.Id != selector.Id) return false;
        if (selector.Classes.Any() && !selector.Classes.All(c => element.Classes.Contains(c))) return false;
        return true;
    }
}