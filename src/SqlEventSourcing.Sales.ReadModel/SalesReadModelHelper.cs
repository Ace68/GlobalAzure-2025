using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Muflone.Persistence.Sql.Dispatcher;
using SqlEventSourcing.Sales.ReadModel.Facade;
using SqlEventSourcing.Sales.ReadModel.Services;

namespace SqlEventSourcing.Sales.ReadModel;

public static class SalesReadModelHelper
{
    public static IServiceCollection AddSalesReadModel(this IServiceCollection services,
        SalesReadModelConfiguration readModelConfiguration)
    {
        services.AddDbContext<SalesDbContext>(options =>
            options.UseSqlServer(readModelConfiguration.ConnectionString));
 
        services.AddSingleton(readModelConfiguration);
        services.AddSingleton<IEventStorePositionRepository, EventStorePositionRepository>();
        
        return services;
    }
}