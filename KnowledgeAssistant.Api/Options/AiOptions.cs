namespace KnowledgeAssistant.Api.Options;

public class AiOptions
{
    public const string SectionName = "Ai";

    public string BaseUrl { get; set; } = "https://api.gptmaker.ai";
    public string ApiKey { get; set; } = string.Empty;
    public string AgentId { get; set; } = string.Empty;
}