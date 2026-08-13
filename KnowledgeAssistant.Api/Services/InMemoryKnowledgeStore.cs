using KnowledgeAssistant.Api.Models;

namespace KnowledgeAssistant.Api.Services;

public class InMemoryKnowledgeStore
{
    public List<KnowledgeItem> Items { get; } = new()
    {
        new KnowledgeItem
        {
            Id = Guid.NewGuid(),
            Title = "O que é RAG?",
            Content = "RAG (Retrieval-Augmented Generation) combina busca em uma base de conhecimento com geração de texto, permitindo que o modelo responda com base em dados reais da empresa.",
            Category = "IA"
        },
        new KnowledgeItem
        {
            Id = Guid.NewGuid(),
            Title = "O que é Function Calling?",
            Content = "Function Calling permite que o modelo de IA decida chamar funções/APIs específicas para executar ações reais, como consultar um serviço ou buscar dados.",
            Category = "IA"
        },
        new KnowledgeItem
        {
            Id = Guid.NewGuid(),
            Title = "O que é um Agente de IA?",
            Content = "Um agente de IA usa modelos generativos para agir: busca informações, chama APIs, executa fluxos e resolve tarefas de ponta a ponta, indo além de simples respostas.",
            Category = "IA"
        },
        new KnowledgeItem
        {
            Id = Guid.NewGuid(),
            Title = "Política de férias",
            Content = "As férias devem ser solicitadas com pelo menos 30 dias de antecedência e aprovadas pelo gestor direto.",
            Category = "RH"
        }
    };
}