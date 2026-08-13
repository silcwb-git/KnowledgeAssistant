using KnowledgeAssistant.Api.Models;

namespace KnowledgeAssistant.Api.Services;

public class SimpleKnowledgeSearch : IKnowledgeSearch
{
    private static readonly HashSet<string> StopWords = new()
    {
        "o", "a", "os", "as", "de", "do", "da", "dos", "das",
        "que", "como", "qual", "para", "por", "com", "em", "um",
        "uma", "e", "é", "sobre", "me", "te", "se"
    };

    private readonly InMemoryKnowledgeStore _store;

    public SimpleKnowledgeSearch(InMemoryKnowledgeStore store)
    {
        _store = store;
    }

    public List<KnowledgeItem> Search(string query, int top = 3)
    {
        var words = Tokenize(query);

        return _store.Items
            .Select(item => new { Item = item, Score = Score(item, words) })
            .Where(x => x.Score > 0)
            .OrderByDescending(x => x.Score)
            .Take(top)
            .Select(x => x.Item)
            .ToList();
    }

    private static string[] Tokenize(string text)
        => text.ToLowerInvariant()
            .Split(new[] { ' ', '.', ',', '!', '?', ';', ':', '(', ')' },
                   StringSplitOptions.RemoveEmptyEntries)
            .Where(w => w.Length > 2 && !StopWords.Contains(w))
            .ToArray();

    private static int Score(KnowledgeItem item, string[] words)
    {
        var haystack = $"{item.Title} {item.Content} {item.Category}".ToLowerInvariant();
        return words.Count(haystack.Contains);
    }
}