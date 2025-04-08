using SqlEventSourcing.Sales.ReadModel.Helpers;
using SqlEventSourcing.Sales.SharedKernel.CustomTypes;
using SqlEventSourcing.Shared.Contracts;
using SqlEventSourcing.Shared.CustomTypes;
using SqlEventSourcing.Shared.DomainIds;
using SqlEventSourcing.Shared.Entities;
using SqlEventSourcing.Shared.ReadModel;

namespace SqlEventSourcing.Sales.ReadModel.Dtos;

public class SalesOrder : EntityBase
{
	public string OrderNumber { get; private set; } = string.Empty;

	public string CustomerId { get; private set; } = string.Empty;
	public string CustomerName { get; private set; } = string.Empty;

	public DateTime OrderDate { get; private set; } = DateTime.MinValue;

	public DateTime DeliveryDate { get; private set; } = DateTime.MaxValue;

	public string Status { get; private set; } = string.Empty;
	

	// public ICollection<SalesOrderRow> SalesOrderRows { get; private set; } = Array.Empty<SalesOrderRow>();

	protected SalesOrder()
	{ }

	public static SalesOrder CreateSalesOrder(SalesOrderId salesOrderId, SalesOrderNumber salesOrderNumber, CustomerId customerId,
		CustomerName customerName, OrderDate orderDate, IEnumerable<SalesOrderRowDto> rows) => new(salesOrderId.Value,
		salesOrderNumber.Value, customerId.Value, customerName.Value, orderDate.Value, rows.ToReadModelEntities());

	private SalesOrder(string salesOrderId, string salesOrderNumber, string customerId, string customerName,
		DateTime orderDate, IEnumerable<SalesOrderRow> rows)
	{
		Id = salesOrderId;
		OrderNumber = salesOrderNumber;
		
		CustomerId = customerId;
		CustomerName = customerName;
		
		OrderDate = orderDate;
		DeliveryDate = orderDate.AddDays(60);
		Status = Shared.Helpers.Status.Created.Name;
		
		// SalesOrderRows = rows.ToList();
	}

	public void CompleteOrder() => Status = Shared.Helpers.Status.Completed.Name;
	
	public void SetDeliveryDate(DeliveryDate deliveryDate) => DeliveryDate = deliveryDate.Value;

	// public SalesOrderJson ToJson() => new(Id, OrderNumber, Guid.Parse(CustomerId), CustomerName, OrderDate, SalesOrderRows.Select(r => r.ToJson));
	
	public SalesOrderJson ToJson() => new(Id, OrderNumber, Guid.Parse(CustomerId), CustomerName, OrderDate, []);
}