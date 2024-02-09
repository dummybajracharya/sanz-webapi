using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SanzWebApplication.DbContext;
using SanzWebApplication.Models;

namespace SanzWebApplication.Controllers;

[ApiController]
[Route("[controller]")]
public class ArticlesController : ControllerBase
{
    private readonly ILogger<ArticlesController> _logger;
    private readonly ArticleDbContext _context;

    public ArticlesController(ILogger<ArticlesController> logger, ArticleDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet(Name = "GetArticles")]
    public async Task<IEnumerable<Article>> Get()
    {
        var result = await _context.Articles.ToListAsync();
        return result;
    }
}