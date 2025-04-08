﻿using Muflone.Core;
using SqlEventSourcing.Sales.Domain.Helpers;
using SqlEventSourcing.Sales.SharedKernel.CustomTypes;
using SqlEventSourcing.Sales.SharedKernel.Events;
using SqlEventSourcing.Shared.Contracts;
using SqlEventSourcing.Shared.CustomTypes;
using SqlEventSourcing.Shared.DomainIds;

namespace SqlEventSourcing.Sales.Domain.Entities;

public class SalesOrder : AggregateRoot
{
	internal SalesOrderNumber _salesOrderNumber;
	internal OrderDate _orderDate;

	internal CustomerId _customerId;
	internal CustomerName _customerName;

	internal IEnumerable<SalesOrderRow> _rows;
	
	internal DeliveryDate _deliveryDate;
	// internal OrderState _orderState;

	protected SalesOrder()
	{
	}

	internal static SalesOrder CreateSalesOrder(SalesOrderId salesOrderId, Guid correlationId, SalesOrderNumber salesOrderNumber,
		OrderDate orderDate, CustomerId customerId, CustomerName customerName, IEnumerable<SalesOrderRowDto> rows)
	{
		// Check SalesOrder invariants

		return new SalesOrder(salesOrderId, correlationId, salesOrderNumber, orderDate, customerId, customerName, rows);
	}

	private SalesOrder(SalesOrderId salesOrderId, Guid correlationId, SalesOrderNumber salesOrderNumber, OrderDate orderDate,
		CustomerId customerId, CustomerName customerName, IEnumerable<SalesOrderRowDto> rows)
	{		
		RaiseEvent(new SalesOrderCreated(salesOrderId, correlationId, salesOrderNumber, orderDate, customerId, customerName, rows));
		//RaiseEvent(new SalesOrderCreatedV2(salesOrderId, correlationId, salesOrderNumber, orderDate, customerId, customerName, new OrderState("Open"), rows));
	}

	private void Apply(SalesOrderCreated @event)
	{
		Id = @event.SalesOrderId;
		_salesOrderNumber = @event.SalesOrderNumber;
		_orderDate = @event.OrderDate;
		_customerId = @event.CustomerId;
		_customerName = @event.CustomerName;
		_rows = @event.Rows.MapToDomainRows();
	
		_deliveryDate = new DeliveryDate(DateTime.MaxValue);
	}
	
	// private void Apply(SalesOrderCreated @event)
	// {
	// 	SalesOrderCreatedV2 upgradeEvent = new(@event.SalesOrderId, @event.MessageId, @event.SalesOrderNumber,
	// 		@event.OrderDate, @event.CustomerId, @event.CustomerName, new OrderState("Open"), @event.Rows);
	// 	Apply(upgradeEvent);
	// }
	
	// private void Apply(SalesOrderCreatedV2 @event)
	// {
	// 	Id = @event.SalesOrderId;
	// 	_salesOrderNumber = @event.SalesOrderNumber;
	// 	_orderDate = @event.OrderDate;
	// 	_customerId = @event.CustomerId;
	// 	_customerName = @event.CustomerName;
	// 	_rows = @event.Rows.MapToDomainRows();
	//
	// 	_deliveryDate = new DeliveryDate(DateTime.MaxValue);
	// 	_orderState = @event.OrderState;
	// }
	
	internal void SetDeliveryDate(DeliveryDate deliveryDate, Guid correlationId)
	{
		if (deliveryDate.Value < _orderDate.Value)
		{
			RaiseEvent(new SalesOrderDomainException(new SalesOrderId(new Guid(Id.Value)), correlationId,
				new Exception("Delivery date cannot be earlier than order date.")));
			return;
		}
		
		RaiseEvent(new SalesOrderDeliveryDateSet(new SalesOrderId(new Guid(Id.Value)), correlationId, deliveryDate));
	}
	
	private void Apply(SalesOrderDeliveryDateSet @event)
	{
		//_deliveryDate = @event.DeliveryDate;
	}
	
	private void Apply(SalesOrderDomainException @event)
	{
		// do nothing
	}
}