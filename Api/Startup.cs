﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public IContainer ApplicationContainer { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Configure Cors Policy
            var cors = Configuration.GetSection("CorsOrigins").GetChildren().Select(o => o.Value).ToArray();

            services.AddCors(options =>
           {
               options.AddPolicy("default", policy =>
              {
                  policy.WithOrigins(cors)
                      .AllowAnyHeader()
                      .AllowAnyMethod();
              });
           });

            // Add framework services.
            services.AddMvcCore(setupAction =>
            {
                setupAction.ReturnHttpNotAcceptable = true;
            })
                    .AddJsonFormatters()
                    .AddApiExplorer();

            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddSwaggerGen();

            // Create the container builder.
            var builder = new ContainerBuilder();

            // Register dependencies, populate the services from
            // the collection, and build the container. If you want
            // to dispose of the container at the end of the app,
            // be sure to keep a reference to it as a property or field.
            builder.Populate(services);
            this.ApplicationContainer = builder.Build();

            // Create the IServiceProvider based on the container.
            return new AutofacServiceProvider(this.ApplicationContainer);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (!env.IsDevelopment())
            {
                var options = new RewriteOptions().AddRedirectToHttps();
                app.UseRewriter(options);
                app.UseExceptionHandler(); // TODO: custom exception handling middleware with logging?
            }
            else
            {
                app.UseDeveloperExceptionPage();
            }

            // this uses the policy called "default"
            app.UseCors("default");

            var authority = Configuration.GetValue<string>("IdentityServer");

            app.UseIdentityServerAuthentication(new IdentityServerAuthenticationOptions
            {
                Authority = authority,
                RequireHttpsMetadata = false,

                ApiName = "api1"
            });

            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUi(); //default, available at /swagger/ui
        }
    }
}


