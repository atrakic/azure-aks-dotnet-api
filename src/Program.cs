using System.Text.Json.Serialization;
using DotnetApi.Data;
using DotnetApi.Services;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddHealthChecks();
builder.Services.AddLogging();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
    options.SerializerOptions.WriteIndented = true;
});

builder.Services.AddSingleton<IQuoteService, QuoteService>();

var app = builder.Build();

app.MapHealthChecks("/healthz");

app.MapGet("/", () =>
{
    var version = System.Environment.GetEnvironmentVariable("VERSION")
        ?? builder.Configuration["VERSION"]
        ?? "beta";

    return new Metadata(System.Net.Dns.GetHostName(), version);
});

app.MapGet("/users", () =>
{
    var users = UserGenerator.Generate();
    return users;
});

app.MapGet("/quote", async (IQuoteService quoteService) =>
{
    var quote = await quoteService.RandomQuote();
    return Results.Json(quote);
});

app.Run();

public record Metadata(string Hostname, string Version);
public record User(string Name);

[JsonSerializable(typeof(Metadata))]
[JsonSerializable(typeof(User))]
[JsonSerializable(typeof(User[]))]
[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(IEnumerable<User>))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}

public partial class Program {}
