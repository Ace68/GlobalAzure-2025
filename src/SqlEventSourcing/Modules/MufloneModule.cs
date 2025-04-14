using Muflone.Persistence.Sql;
using Muflone.Persistence.Sql.Services;

namespace SqlEventSourcing.Modules;

public class MufloneModule : IModule
{
    public bool IsEnabled => true;
    public int Order => 80;
    
    public IServiceCollection Register(WebApplicationBuilder builder)
    {
        builder.Services.AddSqlStore(builder.Configuration);
        
        return builder.Services;
    }

    public WebApplication Configure(WebApplication app)
    {
        var group = app.MapGroup("/v1/muflone/")
            .WithTags("Muflone");
        
        group.MapGet("/aggregate/{aggregateId}", HandleGetAggregateStream)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status200OK)
            .WithName("GetAggregateStream");
        
        group.MapGet("/aggregate", HandleGetAggregates)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status200OK)
            .WithName("GetAggregates");

        return app;
    }

    private static async Task<IResult> HandleGetAggregates(IMufloneSqlPersistenceService mufloneSqlPersistenceService,
        CancellationToken cancellationToken)
    {
        var aggregates = await mufloneSqlPersistenceService.GetAggregatesAsync(cancellationToken);
        
        return Results.Ok(aggregates);
    }

    private static async Task<IResult> HandleGetAggregateStream(
        IMufloneSqlPersistenceService mufloneSqlPersistenceService,
        string aggregateId,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(aggregateId))
            return Results.BadRequest("AggregateId is required");

        var stream = await mufloneSqlPersistenceService.GetAggregateStreamByIdAsync(aggregateId, 0, cancellationToken);
        
        return Results.Ok(stream);
    }
}