using KnowledgeAssistant.Api.Models;
using KnowledgeAssistant.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace KnowledgeAssistant.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class KnowledgeItemsController : ControllerBase
{
    private readonly InMemoryKnowledgeStore _store;
    private readonly IKnowledgeSearch _search;

    public KnowledgeItemsController(InMemoryKnowledgeStore store, IKnowledgeSearch search)
    {
        _store = store;
        _search = search;
    }

    [HttpGet]
    public ActionResult<IEnumerable<KnowledgeItem>> GetAll()
        => Ok(_store.GetAll());

    [HttpGet("{id:guid}")]
    public ActionResult<KnowledgeItem> GetById(Guid id)
    {
        var item = _store.GetById(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpGet("count")]
    public ActionResult<int> Count()
        => Ok(_store.Count());

    // 🆕 NOVO: busca semântica por palavra-chave
    [HttpPost("search")]
    public ActionResult<IEnumerable<KnowledgeItem>> Search([FromBody] SearchRequest request)
        => Ok(_search.Search(request.Q ?? string.Empty, request.Top ?? 3));

    [HttpPost]
    public ActionResult<KnowledgeItem> Create(KnowledgeItem item)
    {
        var created = _store.Add(item);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public IActionResult Update(Guid id, KnowledgeItem item)
    {
        var existing = _store.GetById(id);
        if (existing is null) return NotFound();

        existing.Title = item.Title;
        existing.Content = item.Content;
        existing.Category = item.Category;
        _store.Update(existing);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
        => _store.Delete(id) ? NoContent() : NotFound();
}