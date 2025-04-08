using Muflone.Core;

namespace SqlEventSourcing.Shared.DomainIds;

public sealed class ProductionOrderId : DomainId
{
    public ProductionOrderId(Guid value) : base(value.ToString())
    {
    }
}