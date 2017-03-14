﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using NovaApp.API.DataProvider;

namespace NovaApp.API
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            services.AddSingleton<IDataProvider, MockDataProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            UseCustomCors(app);

            app.UseMvc();
        }

        public static IApplicationBuilder UseCustomCors(IApplicationBuilder app)
        {
            app.Use(async (httpContext, next) =>
            {
                if (httpContext.Request.Path.Value.Contains("rest/"))
                {
                    httpContext.Response.Headers.Add("Access-Control-Allow-Origin", new StringValues(httpContext.Request.Headers["Origin"].ToArray()));
                    httpContext.Response.Headers.Add("Access-Control-Allow-Headers", new[] { "Origin, X-Requested-With, Content-Type, Accept, Authorization, SC-Service-Link" });
                    httpContext.Response.Headers.Add("Access-Control-Allow-Methods", new[] { "GET, POST, PUT, DELETE, OPTIONS" });
                    httpContext.Response.Headers.Add("Access-Control-Allow-Credentials", new[] { "true" });

                    if (httpContext.Request.Method == "OPTIONS") return;
                }
                
                await next();
            });

            app.UseCors(builder =>
            {
                builder.AllowAnyHeader();
                builder.WithMethods("GET, POST, PUT, DELETE, OPTIONS");
                builder.AllowAnyOrigin();
                builder.AllowCredentials();
            });

            return app;
        }

    }
}
