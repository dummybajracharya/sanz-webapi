using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using SanzWebApplication.Models;

namespace SanzWebApplication.Tests;

public class ArticlesControllerTests : IClassFixture<MsSqlServerDockerWebApplicationFactoryFixture>
{
    private readonly MsSqlServerDockerWebApplicationFactoryFixture _factory;
    private readonly HttpClient _client;

    public ArticlesControllerTests(MsSqlServerDockerWebApplicationFactoryFixture factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetArticles_ShouldReturn_Articles()
    {
        var response = await _client.GetAsync("Articles");
        var result = await response.Content.ReadFromJsonAsync<List<Article>>();

        response.Should().HaveStatusCode(HttpStatusCode.OK);
        // !response.Should().HaveServerError();

    }
}