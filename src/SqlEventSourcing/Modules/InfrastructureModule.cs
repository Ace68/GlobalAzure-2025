using Muflone.Persistence.Sql;
using SqlEventSourcing.Sales.Facade;
using SqlEventSourcing.Sales.Infrastructure;
using SqlEventSourcing.Sales.ReadModel;

namespace SqlEventSourcing.Modules;

public sealed class InfrastructureModule : IModule
{
    public bool IsEnabled => true;
    public int Order => 90;

    public IServiceCollection Register(WebApplicationBuilder builder)
    {
        builder.Services.AddSqlStore(builder.Configuration);
        var azureServiceBusSettings = builder.Configuration.GetSection("BrewUp:AzureServiceBus")
            .Get<AzureServiceBusSettings>()!;
        var salesReadModelConfiguration = builder.Configuration.GetSection("BrewUp:SqlServer")
            .Get<SalesReadModelConfiguration>()!;
       builder.Services.AddSalesInfrastructure(azureServiceBusSettings, salesReadModelConfiguration);
       
        
        return builder.Services;
    }

    public WebApplication Configure(WebApplication app) => app;
}