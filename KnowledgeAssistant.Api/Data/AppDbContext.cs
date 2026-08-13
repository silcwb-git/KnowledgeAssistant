using KnowledgeAssistant.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeAssistant.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<KnowledgeItem> KnowledgeItems => Set<KnowledgeItem>();
}