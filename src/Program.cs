using Prometheus;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHealthChecks();
builder.Services.AddLogging();

var app = builder.Build();
app.MapHealthChecks("/healthz");
app.UseMetricServer();
app.UseHttpLogging();
app.UseRouting();
app.UseHttpMetrics();

app.MapGet("/", () =>
{
    var version = Environment.GetEnvironmentVariable("VERSION") ?? builder.Configuration["VERSION"];
    return new Metadata(System.Net.Dns.GetHostName(), version);
}).WithName("GetMetadata");

app.MapGet("/users", () =>
{
    var users = new User[] {
        new User("Guest"),
        new User("Admin"),
        new User("Manager")};
    return users;
}).WithName("GetUsers");

app.Run();

record Metadata(string Hostname, string Version){}
record User(string name){}
