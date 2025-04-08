﻿namespace SqlEventSourcing.Sales.ReadModel;

public class SalesReadModelConfiguration
{
    public string ConnectionString { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
}