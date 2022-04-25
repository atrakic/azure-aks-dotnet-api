using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () =>
{
    const string version = "0.0.1";
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

app.UseMetricServer();
app.UseHttpLogging();
app.UseRouting();
app.UseHttpMetrics();

app.Run();

record ProgramMetadata(string Hostname, string Version){}
record User(string name){}
