using SqlEventSourcing.Sales.Facade;
using SqlEventSourcing.Sales.Facade.Endpoints;

namespace SqlEventSourcing.Modules;

public class SalesModule : IModule
{
    public bool IsEnabled => true;
    public int Order => 0;
    
    public IServiceCollection Register(WebApplicationBuilder builder)
    {
        builder.Services.AddSales();
        
        return builder.Services;
    }

    public WebApplication Configure(WebApplication app)
    {
        app.MapSalesEndpoints();

        return app;
    }
}