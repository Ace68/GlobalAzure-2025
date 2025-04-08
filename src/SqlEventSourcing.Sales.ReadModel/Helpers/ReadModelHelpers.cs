using SqlEventSourcing.Sales.ReadModel.Dtos;
using SqlEventSourcing.Shared.Contracts;

namespace SqlEventSourcing.Sales.ReadModel.Helpers;

public static class ReadModelHelpers
{
	public static IEnumerable<SalesOrderRow> ToReadModelEntities(this IEnumerable<SalesOrderRowDto> dtos)
	{
		return dtos.Select(dto =>
			SalesOrderRow.Create(Guid.NewGuid().ToString(), dto.BeerId.ToString(), dto.BeerName, dto.Quantity, dto.Price)
			);
	}
}