using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using KnowledgeAssistant.Api.Options;
using Microsoft.Extensions.Options;

namespace KnowledgeAssistant.Api.Services;

public class GptMakerAiClient : IAiClient
{
    private readonly HttpClient _http;
    private readonly AiOptions _options;

    public GptMakerAiClient(HttpClient http, IOptions<AiOptions> options)
    {
        _http = http;
        _options = options.Value;
    }

    public async Task<string> GenerateAnswerAsync(string systemPrompt, string userPrompt, CancellationToken ct = default)
    {
        var payload = new
        {
            contextId = "knowledge-assistant-demo",
            prompt = $"{systemPrompt}\n\n{userPrompt}"
        };

        using var request = new HttpRequestMessage(
            HttpMethod.Post,
            $"{_options.BaseUrl.TrimEnd('/')}/v2/agent/{_options.AgentId}/conversation");

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _options.ApiKey);
        request.Content = new StringContent(
            JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

        using var response = await _http.SendAsync(request, ct);
        response.EnsureSuccessStatusCode();

        using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync(ct));
        return json.RootElement.GetProperty("message").GetString() ?? string.Empty;
    }
}