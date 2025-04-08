using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using SqlEventSourcing.Sales.Facade.Validators;
using SqlEventSourcing.Sales.Infrastructure;
using SqlEventSourcing.Sales.ReadModel;
using SqlEventSourcing.Sales.ReadModel.Dtos;
using SqlEventSourcing.Sales.ReadModel.Queries;
using SqlEventSourcing.Sales.ReadModel.Services;
using SqlEventSourcing.Shared.ReadModel;

namespace SqlEventSourcing.Sales.Facade;

public static class SalesHelper
{
	public static IServiceCollection AddSales(this IServiceCollection services)
	{
		services.AddFluentValidationAutoValidation();
		services.AddValidatorsFromAssemblyContaining<SalesOrderContractValidator>();
		services.AddSingleton<ValidationHandler>();

		services.AddScoped<ISalesFacade, SalesFacade>();
		services.AddScoped<ISalesOrderService, SalesOrderService>();
		services.AddScoped<IAvailabilityService, AvailabilityService>();
		services.AddScoped<IQueries<SalesOrder>, SalesOrderQueries>();
		services.AddScoped<IQueries<Availability>, AvailabilityQueries>();

		return services;
	}

	public static IServiceCollection AddSalesInfrastructure(this IServiceCollection services, 
		AzureServiceBusSettings azureServiceBusSettings,
		SalesReadModelConfiguration readModelConfiguration)
	{
		services.AddSalesReadModel(readModelConfiguration);
		services.AddAzureServiceBusForSalesModule(azureServiceBusSettings);

		return services;
	}
}