using incedentAPI_RimaBouazra.models;
using System.Net.Http.Json;
using Xunit;
[Trait("Category", "Integration")]
public class IncidentsIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public IncidentsIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetIncidents_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/IncedentsDB");

        response.EnsureSuccessStatusCode(); // 200 OK
    }

    [Fact]
    public async Task PostIncident_CreatesIncident()
    {
        var incident = new
        {
            Title = "Test Incident",
            Description = "Test Description",
            Severity = "HIGH"
        };

        var response = await _client.PostAsJsonAsync("/api/IncedentsDB", incident);

        response.EnsureSuccessStatusCode();

        var createdIncident = await response.Content.ReadFromJsonAsync<Incident>();

        Assert.NotNull(createdIncident);
        Assert.Equal("Test Incident", createdIncident.Title);
        Assert.Equal("HIGH", createdIncident.Severity);
    }

    [Fact]
    public async Task PostThenGet_ReturnsInsertedIncident()
    {
        var incident = new
        {
            Title = "Integration Test",
            Description = "Test Description",
            Severity = "MEDIUM"
        };

        await _client.PostAsJsonAsync("/api/IncedentsDB", incident);

        var response = await _client.GetAsync("/api/IncedentsDB");

        var data = await response.Content.ReadFromJsonAsync<List<Incident>>();
        Assert.NotNull(data);
        Assert.Contains(data, i => i.Title == "Integration Test");
    }
}