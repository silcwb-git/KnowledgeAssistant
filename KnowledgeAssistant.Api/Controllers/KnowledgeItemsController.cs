using KnowledgeAssistant.Api.Models;
using KnowledgeAssistant.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace KnowledgeAssistant.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class KnowledgeItemsController : ControllerBase
{
    private readonly InMemoryKnowledgeStore _store;

    public KnowledgeItemsController(InMemoryKnowledgeStore store)
    {
        _store = store;
    }

    [HttpGet]
    public ActionResult<IEnumerable<KnowledgeItem>> GetAll()
        => Ok(_store.Items);

    [HttpGet("{id:guid}")]
    public ActionResult<KnowledgeItem> GetById(Guid id)
    {
        var item = _store.Items.FirstOrDefault(x => x.Id == id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public ActionResult<KnowledgeItem> Create(KnowledgeItem item)
    {
        item.Id = Guid.NewGuid();
        item.CreatedAt = DateTime.UtcNow;
        _store.Items.Add(item);
        return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
    }

    [HttpPut("{id:guid}")]
    public IActionResult Update(Guid id, KnowledgeItem item)
    {
        var existing = _store.Items.FirstOrDefault(x => x.Id == id);
        if (existing is null) return NotFound();

        existing.Title = item.Title;
        existing.Content = item.Content;
        existing.Category = item.Category;
        existing.UpdatedAt = DateTime.UtcNow;
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        var item = _store.Items.FirstOrDefault(x => x.Id == id);
        if (item is null) return NotFound();

        _store.Items.Remove(item);
        return NoContent();
    }
}