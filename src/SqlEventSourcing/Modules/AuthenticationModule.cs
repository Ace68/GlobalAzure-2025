﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace SqlEventSourcing.Modules
{
    public class AuthenticationModule : IModule
    {
        public bool IsEnabled => true;
        public int Order => 0;

        public IServiceCollection Register(WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                sharedOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(jwtBearerOptions =>
            {
                jwtBearerOptions.Authority = builder.Configuration["BrewUp:TokenAuthentication:Issuer"];
                jwtBearerOptions.Audience = builder.Configuration["BrewUp:TokenAuthentication:Audience"];
                jwtBearerOptions.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = authenticationContext =>
                    {
                        if (authenticationContext.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            authenticationContext.Response.Headers.Add("Is-Token-Expired", "true");

                        authenticationContext.NoResult();

                        authenticationContext.Response.StatusCode = 500;
                        authenticationContext.Response.ContentType = "text/plain";

                        return authenticationContext.Response.WriteAsync(
                            $"An error occurred processing your authentication. Details: {authenticationContext.Exception}");
                    }
                };
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Administrators", policy => policy.RequireClaim("user_roles", "[Administrator]"));
            });

            return builder.Services;
        }

        WebApplication IModule.Configure(WebApplication app) => app;
    }
}