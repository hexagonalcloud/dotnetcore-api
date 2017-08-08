using System;
using System.Collections.Generic;
using System.Linq;
using Api.Data;
 using Api.Data.Sql;
using Api.Filters;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.Swagger.Model;

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
            // make configuration available for DI
            services.AddSingleton(_ => Configuration);

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

            services.AddResponseCaching();

            // Allow running without authorization if in a dev environment
            if (Configuration.GetValue<bool>("DisableAuthorization") && 
                Configuration.GetValue<string>("ASPNETCORE_ENVIRONMENT").Equals("Development", StringComparison.OrdinalIgnoreCase))
            {
                AddMvcWithoutAuthorization(services);
            }
            else
            {
                AddMvcWithAuthorization(services); 
            }
 
            services.AddRouting(options => options.LowercaseUrls = true);
          
	        services.AddSwaggerGen(options =>
	        {
		        options.AddSecurityDefinition("oauth2", new OAuth2Scheme
		        {
			        Type = "oauth2",
			        Flow = "implicit",
			        AuthorizationUrl = Configuration.GetValue<string>("IdentityServer") + "/connect/authorize",
			        TokenUrl = Configuration.GetValue<string>("IdentityServer") + "/connect/token",
			        Scopes = new Dictionary<string, string>
			        {
				        {"api1", "API Access"}
			        }
		        });	       
		        // Assign scope requirements to operations based on AuthorizeAttribute
	            options.OperationFilter<SecurityRequirementsOperationFilter>();
	        }); 
	        
            services.AddOptions();

            services.Configure<ConnectionStrings>(options =>
            {
                options.SqlAdventure =
                    Configuration.GetConnectionString("SqlAdventure");
            });

            var useInMemoryCache = Configuration.GetValue<bool>("UseInMemoryCache");

            if (useInMemoryCache)
            {
                services.AddDistributedMemoryCache();
            }
            else
            {
                services.AddDistributedRedisCache(option =>
                {
                    option.Configuration = Configuration.GetConnectionString("Redis");
                    option.InstanceName = Configuration.GetValue<string>("RedisInstanceName");
                });
            }

            // TODO:  verify how to best regster all services: only use autofac for non-framework, or register everything with autofac?

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddScoped<IUrlHelper, UrlHelper>(serviceProvider =>
            {
                var actionContext = serviceProvider.GetService<IActionContextAccessor>().ActionContext;
                return new UrlHelper(actionContext);
            });

            // Create the container builder.
            var builder = new ContainerBuilder();

            // Register dependencies, populate the services from
            // the collection, and build the container. If you want
            // to dispose of the container at the end of the app,
            // be sure to keep a reference to it as a property or field.
            builder.Populate(services);

            builder.RegisterType<ProductData>().As<IProductData>();

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
	        app.UseSwaggerUI(options =>
	        {
		        options.SwaggerEndpoint("v1/swagger.json", ".NET Core API v1");     
		        options.ConfigureOAuth2(Configuration.GetValue<string>("SwaggerClientId"), Configuration.GetValue<string>("SwaggerClientSecret"), 
			        "http://localhost:5001", "Swagger UI");
	        });
	         
            app.UseResponseCaching();

        }

        private static void AddMvcWithAuthorization(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvcCore(options =>
                {
                    //options.ReturnHttpNotAcceptable = true;
                    options.CacheProfiles.Add("Default",
                        new CacheProfile()
                        {
                            Duration = 60,
                            VaryByHeader = "Accept",
                            Location = ResponseCacheLocation.Any
                        });
                    options.CacheProfiles.Add("Never",
                        new CacheProfile()
                        {
                            Location = ResponseCacheLocation.None,
                            NoStore = true
                        });
                })
                .AddAuthorization()
                .AddJsonFormatters()
                .AddApiExplorer();
        }

        private static void AddMvcWithoutAuthorization(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvcCore(options =>
                {
                    //options.ReturnHttpNotAcceptable = true;
                    options.CacheProfiles.Add("Default",
                        new CacheProfile()
                        {
                            Duration = 60,
                            VaryByHeader = "Accept",
                            Location = ResponseCacheLocation.Any
                        });
                    options.CacheProfiles.Add("Never",
                        new CacheProfile()
                        {
                            Location = ResponseCacheLocation.None,
                            NoStore = true
                        });
                })
                .AddJsonFormatters()
                .AddApiExplorer();
        } 
	    
    }
}


