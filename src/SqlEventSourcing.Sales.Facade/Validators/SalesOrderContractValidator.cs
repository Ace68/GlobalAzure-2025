using FluentValidation;
using SqlEventSourcing.Shared.Contracts;

namespace SqlEventSourcing.Sales.Facade.Validators;

public class SalesOrderContractValidator : AbstractValidator<SalesOrderJson>
{
	public SalesOrderContractValidator()
	{
		RuleFor(v => v.SalesOrderNumber).NotEmpty();

		RuleFor(v => v.CustomerId).NotEmpty();
		RuleFor(v => v.CustomerName).NotEmpty();
		RuleFor(v => v.OrderDate).GreaterThan(DateTime.MinValue);

		RuleForEach(v => v.Rows).SetValidator(new SalesOrderRowValidator());
	}
}