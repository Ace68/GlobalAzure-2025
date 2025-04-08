using SqlEventSourcing.Shared.Contracts;
using SqlEventSourcing.Shared.CustomTypes;
using SqlEventSourcing.Shared.ReadModel;

namespace SqlEventSourcing.Sales.ReadModel.Dtos;

public class SalesOrderRow : EntityBase
{
	public string OrderId { get; private set; } = string.Empty;
	public string BeerId { get; private set; } = string.Empty;
	public string BeerName { get; private set; } = string.Empty;
	public decimal Quantity { get; private set; }
	public string UnitOfMeasure { get; private set; } = string.Empty;
	public decimal Price { get; private set; }
	public string Currency { get; private set; } = string.Empty;
	
	public virtual SalesOrder SalesOrder { get; private set; } = null!;
	
	protected SalesOrderRow() { }

	public static SalesOrderRow Create(string saleOrderId, string beerId, string beerName, Quantity quantity, Price price)
	{
		return new SalesOrderRow(saleOrderId, beerId, beerName, quantity, price);
	}

	private SalesOrderRow(string saleOrderId, string beerId, string beerName, Quantity quantity, Price price)
	{
		Id = Guid.NewGuid().ToString();
		OrderId = saleOrderId;
		BeerId = beerId;
		BeerName = beerName;
		Quantity = quantity.Value;
		UnitOfMeasure = quantity.UnitOfMeasure;
		Price = price.Value;
		Currency = price.Currency;
	}

	internal SalesOrderRowDto ToJson => new()
	{
		BeerId = new Guid(BeerId),
		BeerName = BeerName,
		Quantity = new Quantity(Quantity, UnitOfMeasure),
		Price = new Price(Price, Currency)
	};
}