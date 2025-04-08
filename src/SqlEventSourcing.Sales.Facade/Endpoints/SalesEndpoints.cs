﻿using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SqlEventSourcing.Sales.Facade.Validators;
using SqlEventSourcing.Shared.Contracts;

namespace SqlEventSourcing.Sales.Facade.Endpoints;

public static class SalesEndpoints
{
	public static IEndpointRouteBuilder MapSalesEndpoints(this IEndpointRouteBuilder endpoints)
	{
		var group = endpoints.MapGroup("/v1/sales/")
			.WithTags("Sales");

		group.MapPost("/", HandleCreateOrder)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status201Created)
			.WithName("CreateSalesOrder");
		
		group.MapPut("/{salesOrderId}", HandleSetDeliveryDate)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status201Created)
			.WithName("SetDeliveryDate");

		group.MapGet("/", HandleGetOrders)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status200OK)
			.WithName("GetSalesOrders");

		return endpoints;
	}

	private static async Task<IResult> HandleCreateOrder(
		ISalesFacade salesUpFacade,
		IValidator<SalesOrderJson> validator,
		ValidationHandler validationHandler,
		SalesOrderJson body,
		CancellationToken cancellationToken)
	{
		await validationHandler.ValidateAsync(validator, body);
		if (!validationHandler.IsValid)
			return Results.BadRequest(validationHandler.Errors);

		var salesOrderId = await salesUpFacade.CreateOrderAsync(body, cancellationToken);

		return Results.Created(new Uri($"/v1/sales/{salesOrderId}", UriKind.Relative), salesOrderId);
	}
	
	private static async Task<IResult> HandleSetDeliveryDate(
		ISalesFacade salesUpFacade,
		IValidator<SalesOrderJson> validator,
		ValidationHandler validationHandler,
		string salesOrderId,
		CancellationToken cancellationToken)
	{
		if (string.IsNullOrWhiteSpace(salesOrderId))
			return Results.BadRequest(validationHandler.Errors);
		
		await salesUpFacade.SetDeliveryDateAsync(salesOrderId, cancellationToken);

		return Results.NoContent();
	}

	private static async Task<IResult> HandleGetOrders(
		ISalesFacade salesUpFacade,
		CancellationToken cancellationToken)
	{
		var orders = await salesUpFacade.GetOrdersAsync(cancellationToken);

		return Results.Ok(orders);
	}
}