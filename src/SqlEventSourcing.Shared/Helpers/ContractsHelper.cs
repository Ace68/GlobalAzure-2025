using SqlEventSourcing.Shared.Contracts;
using SqlEventSourcing.Shared.CustomTypes;

namespace SqlEventSourcing.Shared.Helpers;

public static class ContractsHelper
{
	public static IEnumerable<BeerAvailabilityJson> ToBeerAvailabilities(this IEnumerable<SalesOrderRowDto> rows)
		=> rows.Select(row => new BeerAvailabilityJson(row.BeerId.ToString(), row.BeerName,
			new Availability(row.Quantity.Value, 0, row.Quantity.UnitOfMeasure)));
}