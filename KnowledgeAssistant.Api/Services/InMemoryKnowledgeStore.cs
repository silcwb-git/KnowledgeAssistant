using KnowledgeAssistant.Api.Data;
using KnowledgeAssistant.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeAssistant.Api.Services;

public class InMemoryKnowledgeStore
{
    private readonly AppDbContext _db;

    public InMemoryKnowledgeStore(AppDbContext db)
    {
        _db = db;
        SeedIfEmpty();
    }

    public List<KnowledgeItem> GetAll() => _db.KnowledgeItems.ToList();

    public KnowledgeItem? GetById(Guid id) => _db.KnowledgeItems.Find(id);

    public int Count() => _db.KnowledgeItems.Count();

    public KnowledgeItem Add(KnowledgeItem item)
    {
        item.Id = Guid.NewGuid();
        item.CreatedAt = DateTime.UtcNow;
        _db.KnowledgeItems.Add(item);
        _db.SaveChanges();
        return item;
    }

    public void Update(KnowledgeItem item)
    {
        item.UpdatedAt = DateTime.UtcNow;
        _db.KnowledgeItems.Update(item);
        _db.SaveChanges();
    }

    public bool Delete(Guid id)
    {
        var item = _db.KnowledgeItems.Find(id);
        if (item is null) return false;
        _db.KnowledgeItems.Remove(item);
        _db.SaveChanges();
        return true;
    }

    private void SeedIfEmpty()
    {
        if (_db.KnowledgeItems.Any()) return;

        _db.KnowledgeItems.AddRange(
            new KnowledgeItem { Title = "O que é RAG?", Content = "RAG (Retrieval-Augmented Generation) combina busca em uma base de conhecimento com geração de texto, permitindo que o modelo responda com base em dados reais da empresa.", Category = "IA" },
            new KnowledgeItem { Title = "O que é Function Calling?", Content = "Function Calling permite que o modelo de IA decida chamar funções/APIs específicas para executar ações reais, como consultar um serviço ou buscar dados.", Category = "IA" },
            new KnowledgeItem { Title = "O que é um Agente de IA?", Content = "Um agente de IA usa modelos generativos para agir: busca informações, chama APIs, executa fluxos e resolve tarefas de ponta a ponta, indo além de simples respostas.", Category = "IA" },
            new KnowledgeItem { Title = "Política de férias", Content = "As férias devem ser solicitadas com pelo menos 30 dias de antecedência e aprovadas pelo gestor direto.", Category = "RH" }
        );
        _db.SaveChanges();
    }
}