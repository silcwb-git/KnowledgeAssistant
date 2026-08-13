using KnowledgeAssistant.Api.Models;

namespace KnowledgeAssistant.Api.Services;

public interface IKnowledgeSearch
{
    List<KnowledgeItem> Search(string query, int top = 3);
}