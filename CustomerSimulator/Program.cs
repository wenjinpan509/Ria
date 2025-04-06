using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;

var firstNames = new[] { "Leia", "Sadie", "Jose", "Sara", "Frank", "Dewey", "Tomas", "Joel", "Lukas", "Carlos" };
var lastNames = new[] { "Liberty", "Ray", "Harrison", "Ronan", "Drew", "Powell", "Larsen", "Chan", "Anderson", "Lane" };

var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";

var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
    .Build();

var baseUrl = config["Api:BaseAddress"];
var client = new HttpClient { BaseAddress = new Uri(baseUrl) };

var rand = new Random();
int globalId = 1;

async Task SendPost()
{
    var count = rand.Next(2, 5);
    var list = new List<Customer>();

    for (int i = 0; i < count; i++)
    {
        list.Add(new Customer
        {
            FirstName = firstNames[rand.Next(firstNames.Length)],
            LastName = lastNames[rand.Next(lastNames.Length)],
            Age = rand.Next(10, 90),
            Id = Interlocked.Increment(ref globalId)
        });
    }

    var response = await client.PostAsJsonAsync("customer/add", list);
    var resultText = await response.Content.ReadAsStringAsync();

    if (response.IsSuccessStatusCode)
    {
        Console.WriteLine($"[POST] Success ({response.StatusCode}) - Added {list.Count} customers.");
    }
    else
    {
        Console.WriteLine($"[POST] Failed ({response.StatusCode})");
        try
        {
            var errorObject = System.Text.Json.JsonDocument.Parse(resultText);
            if (errorObject.RootElement.TryGetProperty("errors", out var errorsProp) && errorsProp.ValueKind == System.Text.Json.JsonValueKind.Array)
            {
                foreach (var error in errorsProp.EnumerateArray())
                {
                    Console.WriteLine($"    {error.GetString()}");
                }
            }
            else
            {
                Console.WriteLine($"    Raw error: {resultText}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"    Could not parse error response: {resultText}");
            Console.WriteLine($"{ex.Message}");
        }
    }
}

async Task SendGet()
{
    var res = await client.GetAsync("customer/get");
    var content = await res.Content.ReadAsStringAsync();
    Console.WriteLine($"GET: {content}");
}

List<Task> tasks = [];

for (int i = 0; i < 10; i++)
{
    tasks.Add(SendPost());
    tasks.Add(SendGet());
}

await Task.WhenAll(tasks);

record Customer
{
    public string FirstName { get; init; } = "";
    public string LastName { get; init; } = "";
    public int Age { get; init; }
    public int Id { get; init; }
}