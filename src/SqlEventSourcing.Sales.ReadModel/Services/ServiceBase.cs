using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Muflone.Persistence.Sql.Persistence;
using SqlEventSourcing.Sales.ReadModel.Facade;

namespace SqlEventSourcing.Sales.ReadModel.Services;

public abstract class ServiceBase
{
	protected readonly SqlOptions SqlOptions;
	protected readonly SalesDbContext SalesDbContext;
	protected readonly ILogger Logger;

	protected ServiceBase(ILoggerFactory loggerFactory, 
		SalesDbContext salesDbContext, 
		SqlOptions sqlOptions)
	{
		SalesDbContext = salesDbContext ?? throw new ArgumentNullException(nameof(salesDbContext));
		SqlOptions = sqlOptions ?? throw new ArgumentNullException(nameof(sqlOptions));
		Logger = loggerFactory.CreateLogger(GetType()) ?? throw new ArgumentNullException(nameof(loggerFactory));
	}
}