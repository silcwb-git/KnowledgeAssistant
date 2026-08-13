namespace KnowledgeAssistant.Api.Models;

public class ChatResponse
{
    public string Answer { get; set; } = string.Empty;
    public List<string> Sources { get; set; } = new();
    public bool IsDemo { get; set; }
}