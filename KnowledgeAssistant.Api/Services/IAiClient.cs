namespace KnowledgeAssistant.Api.Services;

public interface IAiClient
{
    Task<string> GenerateAnswerAsync(string systemPrompt, string userPrompt, CancellationToken ct = default);
}