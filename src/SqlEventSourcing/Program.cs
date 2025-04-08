using SqlEventSourcing.Modules;

var builder = WebApplication.CreateBuilder(args);

// Register Modules
builder.RegisterModules();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.ConfigureModules();

await app.RunAsync();