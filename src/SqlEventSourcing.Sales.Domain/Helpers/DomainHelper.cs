using SqlEventSourcing.Sales.Domain.Entities;
using SqlEventSourcing.Sales.SharedKernel.CustomTypes;
using SqlEventSourcing.Shared.Contracts;
using SqlEventSourcing.Shared.CustomTypes;
using SqlEventSourcing.Shared.DomainIds;

namespace SqlEventSourcing.Sales.Domain.Helpers;

public static class DomainHelper
{
	internal static SalesOrderRow MapToDomainRow(this SalesOrderRowDto json)
	{
		return SalesOrderRow.CreateSalesOrderRow(new BeerId(json.BeerId), new BeerName(json.BeerName), json.Quantity, json.Price);
	}

	internal static IEnumerable<SalesOrderRow> MapToDomainRows(this IEnumerable<SalesOrderRowDto> json)
	{
		return json.Select(r =>
			SalesOrderRow.CreateSalesOrderRow(new BeerId(r.BeerId), new BeerName(r.BeerName), r.Quantity, r.Price));
	}

	internal static ReadModel.Dtos.SalesOrder MapToReadModel(this SalesOrder salesOrder)
	{
		return ReadModel.Dtos.SalesOrder.CreateSalesOrder((SalesOrderId)salesOrder.Id, salesOrder._salesOrderNumber,
						salesOrder._customerId, salesOrder._customerName, salesOrder._orderDate,
									salesOrder._rows.Select(r => new SalesOrderRowDto
									{
										BeerId = Guid.Parse(r._beerId.Value),
										BeerName = r._beerName.Value,
										Quantity = r._quantity,
										Price = r._beerPrice
									}));
	}
}