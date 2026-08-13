namespace KnowledgeAssistant.Api.Models;

public class SearchRequest
{
    public string Q { get; set; } = string.Empty;
    public int? Top { get; set; } = 3;
}