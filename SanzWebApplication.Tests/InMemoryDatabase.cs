using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SanzWebApplication.DbContext;
using SanzWebApplication.Models;

namespace SanzWebApplication.Tests;

public class InMemoryDatabase
{
    [Fact]
    public async Task ArticleController_GetArticlesById_ReturnsCorrectArticle()
    {
        var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<ArticleDbContext>));
                services.AddDbContext<ArticleDbContext>(options =>
                {
                    options.UseInMemoryDatabase("test");
                });
            });
        });

        using var scope = factory.Services.CreateScope();
        var scopeService = scope.ServiceProvider;
        var dbContext = scopeService.GetRequiredService<ArticleDbContext>();

        dbContext.Database.EnsureCreated();
        dbContext.Articles.Add(new Article()
        {
            Id = 1,
            Date = new DateTime(2023, 1, 1),
            Title = "Test Title"
        });
        dbContext.SaveChanges();

        var client = factory.CreateClient();
        
        // Act
        var response = await client.GetAsync("Articles/GetArticles/1");
        var result = await response.Content.ReadFromJsonAsync<Article>();
        
        // Assert
        result?.Title.Should().Be("Test Title");
        result?.Date.Should().Be(new DateTime(2023, 1, 1));
    }
}