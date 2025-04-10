﻿using Muflone.Core;
using SqlEventSourcing.Shared.CustomTypes;
using SqlEventSourcing.Shared.DomainIds;

namespace SqlEventSourcing.Sales.Domain.Entities;

public class SalesOrderRow : Entity
{
	internal BeerId _beerId;
	internal BeerName _beerName;

	internal Quantity _quantity;
	internal Price _beerPrice;

	protected SalesOrderRow()
	{
	}

	internal static SalesOrderRow CreateSalesOrderRow(BeerId beerId, BeerName beerName, Quantity quantity,
		Price price)
	{
		return new SalesOrderRow(beerId, beerName, quantity, price);
	}

	private SalesOrderRow(BeerId beerId, BeerName beerName, Quantity quantity, Price price)
	{
		_beerId = beerId;
		_beerName = beerName;
		_quantity = quantity;
		_beerPrice = price;
	}
}