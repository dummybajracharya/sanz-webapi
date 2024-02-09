
using Microsoft.EntityFrameworkCore;
using SanzWebApplication.Models;

namespace SanzWebApplication.DbContext;

public class ArticleDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public ArticleDbContext(DbContextOptions options) : base(options)
    {
        
    }

    public DbSet<Article> Articles { get; set; }
}