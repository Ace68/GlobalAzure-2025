using SqlEventSourcing.Shared.CustomTypes;

namespace SqlEventSourcing.Shared.Contracts;

public record BeerAvailabilityJson(string BeerId, string BeerName, Availability Availability);