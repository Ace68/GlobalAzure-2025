﻿using Serilog;

namespace SqlEventSourcing.Modules
{
    public class SerilogModule : IModule
    {
        public bool IsEnabled => true;
        public int Order => 0;

        public IServiceCollection Register(WebApplicationBuilder builder)
        {
            var logger = new LoggerConfiguration()
              .ReadFrom.Configuration(builder.Configuration)
              .Enrich.FromLogContext()
              .CreateLogger();

            builder.Logging.AddSerilog(logger);

            return builder.Services;
        }

        WebApplication IModule.Configure(WebApplication app) => app;
    }
}