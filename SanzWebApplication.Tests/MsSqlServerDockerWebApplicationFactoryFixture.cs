using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SanzWebApplication.DbContext;
using SanzWebApplication.Models;
using Testcontainers.MsSql;
namespace SanzWebApplication.Tests;

public class MsSqlServerDockerWebApplicationFactoryFixture : WebApplicationFactory<SanzWebApplication.Program>,
    IAsyncLifetime
{
    private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder().Build();


    // Start or Setup
    public async Task InitializeAsync()
    {
        try
        {
            await _msSqlContainer.StartAsync();

            using var scope = Services.CreateScope();
            var scopeService = scope.ServiceProvider;
            var dbContext = scopeService.GetRequiredService<ArticleDbContext>();
            await dbContext.Database.EnsureCreatedAsync();

            // Create some sample data
            var article = new Article
            {
                Title = "Test Title",
                Date = new DateTime(2024, 01, 24)
            };

            await dbContext.Articles.AddAsync(article);
            await dbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    // Dispose or End
    public async Task DisposeAsync()
        => await _msSqlContainer.DisposeAsync();


    // This is the main important section
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        builder.ConfigureServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<ArticleDbContext>));
            services.AddDbContext<ArticleDbContext>(options =>
            {
                options.UseSqlServer(_msSqlContainer.GetConnectionString());
            });
        });
    }
}