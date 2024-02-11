using Microsoft.AspNetCore.Routing;
using Xunit.Abstractions;

namespace SanzWebApplication.Tests;

// IClassFixture is used to shared the resources among all other tests
// You can setup common code in the MyTestFixture and Dispose them once all the unit tests are executed
// If you check the Id of MyTestFixture on other tests, it will remain same but if you check MyId you will notice it will be new all the time 
// it is because for each test, we will be creating new instance but using IClassFixture, we can share same instance on all other tests.
public class ClassFixtureTest : IDisposable
{
    public string Id { get; set; }
    
    public ClassFixtureTest()
    {
        // Setup code
        Console.WriteLine("Fixture setup");
        Id = Guid.NewGuid().ToString();
    }

    public void Dispose()
    {
        // Teardown code
        Console.WriteLine("Fixture teardown");
    }

    
}


// Use the fixture class in tests
public class MyTestClass : IClassFixture<ClassFixtureTest>
{
    private readonly ClassFixtureTest _fixture;
    private readonly ITestOutputHelper _testOutputHelper;
    public string MyId { get; set; }

    public MyTestClass(ClassFixtureTest fixture, ITestOutputHelper testOutputHelper)
    {
        _fixture = fixture;
        _testOutputHelper = testOutputHelper;
        MyId = Guid.NewGuid().ToString();
    }

    [Fact]
    public void Test1()
    {
        // Use _fixture in your test
        _testOutputHelper.WriteLine("Test 1 executed");
        _testOutputHelper.WriteLine($"MY Id is {MyId}");
        _testOutputHelper.WriteLine(_fixture.Id);
        Assert.True(true); // Example assertion
    }

    [Fact]
    public void Test2()
    {
        // Use _fixture in your test
        _testOutputHelper.WriteLine("Test 2 executed");
        _testOutputHelper.WriteLine($"MY Id is {MyId}");
        _testOutputHelper.WriteLine(_fixture.Id);
        Assert.True(true); // Example assertion
    }
}