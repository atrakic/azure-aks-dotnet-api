using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHealthChecks();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () =>
{
    var version = Environment.GetEnvironmentVariable("VERSION") ?? "0.0.1";
    return new ProgramMetadata(System.Net.Dns.GetHostName(), version);
}).WithName("GetProgramMetadata");

app.MapGet("/users", () =>
{
    var users = new User[] {
        new User("Guest"),
        new User("Admin"),
        new User("Manager")};
    return users;
}).WithName("GetUsers");

app.MapHealthChecks("/healthz");
app.UseMetricServer();
app.UseHttpLogging();
app.UseRouting();
app.UseHttpMetrics();

app.Run();

record ProgramMetadata(string Hostname, string Version){}
record User(string name){}
