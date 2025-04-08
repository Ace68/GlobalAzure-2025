using SqlEventSourcing.Shared.Contracts;
using SqlEventSourcing.Shared.CustomTypes;
using SqlEventSourcing.Shared.DomainIds;
using SqlEventSourcing.Shared.ReadModel;

namespace SqlEventSourcing.Sales.ReadModel.Dtos;

public class Availability : EntityBase
{
	public string BeerId { get; private set; } = string.Empty;
	public string BeerName { get; private set; } = string.Empty;

	public decimal Quantity { get; private set; }
	public string UnitOfMeasure { get; private set; } = string.Empty;

	protected Availability()
	{
	}

	public static Availability Create(BeerId beerId, BeerName beerName, Quantity quantity)
	{
		return new Availability(beerId.Value, beerName.Value, quantity);
	}
	
	public void UpdateAvailability(Quantity quantity) => Quantity = quantity.Value;

	private Availability(string beerId, string beerName, Quantity quantity)
	{
		Id = beerId;

		BeerId = beerId;
		BeerName = beerName;
		Quantity = quantity.Value;
		UnitOfMeasure = quantity.UnitOfMeasure;
	}

	public BeerAvailabilityJson ToJson() => new(Id, BeerName,
		new Shared.CustomTypes.Availability(0, Quantity, UnitOfMeasure));
}