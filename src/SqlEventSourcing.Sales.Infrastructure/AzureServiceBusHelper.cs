using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Muflone;
using Muflone.Persistence;
using Muflone.Transport.Azure;
using Muflone.Transport.Azure.Models;
using SqlEventSourcing.Sales.Infrastructure.Commands;
using SqlEventSourcing.Sales.Infrastructure.Events;
using SqlEventSourcing.Sales.ReadModel.Services;

namespace SqlEventSourcing.Sales.Infrastructure;

public static class AzureServiceBusHelper
{
	public static IServiceCollection AddAzureServiceBusForSalesModule(this IServiceCollection services,
		AzureServiceBusSettings azureServiceBusSettings)
	{
		var serviceProvider = services.BuildServiceProvider();
		var repository = serviceProvider.GetRequiredService<IRepository>();
		var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

		AzureServiceBusConfiguration azureServiceBusConfiguration = new (azureServiceBusSettings.ConnectionString, 
			azureServiceBusSettings.TopicName,
			azureServiceBusSettings.ClientId);

		services.AddMufloneTransportAzure(azureServiceBusConfiguration);

		serviceProvider = services.BuildServiceProvider();
		var consumers = serviceProvider.GetRequiredService<IEnumerable<IConsumer>>();

		consumers = consumers.Concat(new List<IConsumer>
		{
			new CreateSalesOrderConsumer(repository,
				azureServiceBusConfiguration,
				loggerFactory),
			new SalesOrderCreatedConsumer(serviceProvider.GetRequiredService<ISalesOrderService>(),
				azureServiceBusConfiguration, loggerFactory)
		});
		services.AddMufloneAzureConsumers(consumers);

		return services;
	}
}