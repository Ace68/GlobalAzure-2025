using Muflone.Core;

namespace SqlEventSourcing.Shared.DomainIds;

public sealed class ProductId : DomainId
{
    public ProductId(Guid value) : base(value.ToString())
    {
    }
}