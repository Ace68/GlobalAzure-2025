namespace SqlEventSourcing.Modules;

public class AspireModule : IModule
{
    public bool IsEnabled => true;
    public int Order => 0;
    
    public IServiceCollection Register(WebApplicationBuilder builder)
    {
        builder.AddServiceDefaults();
        
        return builder.Services;
    }

    public WebApplication Configure(WebApplication app)
    {
        app.MapDefaultEndpoints();
        
        return app;
    }
}