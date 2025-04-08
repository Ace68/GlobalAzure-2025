using Muflone.Core;

namespace SqlEventSourcing.Shared.DomainIds;

public sealed class OrderId : DomainId
{
    public OrderId(Guid value) : base(value.ToString())
    {
    }
}