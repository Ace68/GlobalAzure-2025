﻿using FluentValidation;
using SqlEventSourcing.Shared.Contracts;

namespace SqlEventSourcing.Sales.Facade.Validators;

public class SalesOrderRowValidator : AbstractValidator<SalesOrderRowDto>
{
	public SalesOrderRowValidator()
	{
		RuleFor(v => v.BeerId).NotEmpty();
		RuleFor(v => v.BeerName).NotEmpty();
		RuleFor(v => v.Quantity.Value).GreaterThan(0);
		RuleFor(v => v.Quantity.UnitOfMeasure).NotEmpty();
		RuleFor(v => v.Price.Value).GreaterThan(0);
		RuleFor(v => v.Price.Currency).NotEmpty();
	}
}