using System.Text.Json.Serialization;
using DataSqlite;

//var builder = WebApplication.CreateBuilder(args);
var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddHealthChecks();
builder.Services.AddLogging();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

var app = builder.Build();

app.MapHealthChecks("/healthz");

app.MapGet("/", () =>
{
    var version = System.Environment.GetEnvironmentVariable("VERSION") ?? builder.Configuration["VERSION"];
    return new Metadata(System.Net.Dns.GetHostName(), version);
}).WithName("GetMetadata");

app.MapGet("/users", () =>
{
    var users = SqliteGenerator.GenerateUsers();
    return users;
}).WithName("GetUsers");

app.Run();

public record Metadata(string Hostname, string Version);
public record User(string Name);

// https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/source-generation?pivots=dotnet-8-0
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.KebabCaseLower)]
[JsonSerializable(typeof(Metadata))]
[JsonSerializable(typeof(User))]
[JsonSerializable(typeof(User[]))]
[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(IEnumerable<User>))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}
