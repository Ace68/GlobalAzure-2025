using SqlEventSourcing.Shared.CustomTypes;

namespace SqlEventSourcing.Sales.SharedKernel.Contracts;

public record BeerAvailabilityJson(string BeerId, string BeerName, Quantity Quantity);