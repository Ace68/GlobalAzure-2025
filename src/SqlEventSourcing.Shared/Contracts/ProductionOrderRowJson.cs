﻿using SqlEventSourcing.Shared.CustomTypes;

namespace SqlEventSourcing.Shared.Contracts;

public class ProductionOrderRowJson
{
	public Guid BeerId { get; set; } = Guid.Empty;
	public string BeerName { get; set; } = string.Empty;
	public Quantity Quantity { get; set; } = new(0, string.Empty);
}