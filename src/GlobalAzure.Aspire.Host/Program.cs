var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<Projects.SqlEventSourcing>("api");

builder.AddProject<Projects.GlobalAzure>("blazor")
    .WithReference(api);

builder.Build().Run();