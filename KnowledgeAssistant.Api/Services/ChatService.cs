using KnowledgeAssistant.Api.Models;
using KnowledgeAssistant.Api.Options;
using Microsoft.Extensions.Options;

namespace KnowledgeAssistant.Api.Services;

public class ChatService
{
    private readonly IKnowledgeSearch _search;
    private readonly IAiClient _ai;
    private readonly AiOptions _options;

    public ChatService(IKnowledgeSearch search, IAiClient ai, IOptions<AiOptions> options)
    {
        _search = search;
        _ai = ai;
        _options = options.Value;
    }

    public async Task<ChatResponse> AskAsync(string question, CancellationToken ct = default)
    {
        // 1) RETRIEVAL — busca os trechos relevantes na base
        var sources = _search.Search(question, top: 3);

        // 2) Monta o contexto para o modelo
        var context = string.Join("\n\n",
            sources.Select((s, i) => $"[{i + 1}] {s.Title}: {s.Content}"));

        // 3) GENERATION — IA real ou modo demo
        if (string.IsNullOrWhiteSpace(_options.ApiKey) || string.IsNullOrWhiteSpace(_options.AgentId))
        {
            var demo = sources.Count == 0
                ? "Não encontrei nada relacionado na base de conhecimento."
                : $"Encontrei estes trechos na base de conhecimento:\n\n{context}";

            return new ChatResponse
            {
                Answer = demo,
                Sources = sources.Select(s => s.Title).ToList(),
                IsDemo = true
            };
        }

        var system = "You are a helpful assistant for a corporate knowledge base. " +
                     "Answer ONLY using the provided context. " +
                     "If the context does not contain the answer, say you don't know.";

        var prompt = $"Context:\n{context}\n\nQuestion: {question}";

        var answer = await _ai.GenerateAnswerAsync(system, prompt, ct);

        return new ChatResponse
        {
            Answer = answer,
            Sources = sources.Select(s => s.Title).ToList()
        };
    }
}