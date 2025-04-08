using Muflone.Core;

namespace SqlEventSourcing.Shared.DomainIds;

public sealed class CustomerId : DomainId
{
	public CustomerId(Guid value) : base(value.ToString())
	{
	}
}