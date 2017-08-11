using System.Collections.Generic;
using System.Linq;
using Api.Filters;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
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
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
            Environment = env;
        }

        private IConfigurationRoot Configuration { get; }

        private IHostingEnvironment Environment { get; }

        // ConfigureServices is where you register dependencies. This gets
        // called by the runtime before the ConfigureContainer method, below.
        public void ConfigureServices(IServiceCollection services)
        {
            // make configuration available for DI
            services.AddSingleton(_ => Configuration);

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
            if (Configuration.GetValue<bool>("DisableAuthorization") && Environment.IsDevelopment())
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
                        { "api1", "API Access" }
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

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddScoped<IUrlHelper, UrlHelper>(serviceProvider =>
            {
                var actionContext = serviceProvider.GetService<IActionContextAccessor>().ActionContext;
                return new UrlHelper(actionContext);
            });
        }

        // ConfigureContainer is where you can register things directly
        // with Autofac. This runs after ConfigureServices so the things
        // here will override registrations made in ConfigureServices.
        // Don't build the container; that gets done for you. If you
        // need a reference to the container, you need to use the
        // "Without ConfigureContainer" mechanism shown later.
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new AutofacModule(Environment, Configuration));
        }

        // Configure is where you add middleware. This is called after
        // ConfigureContainer. You can use IApplicationBuilder.ApplicationServices
        // here if you need to resolve things from the container.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
        {
//            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
//            loggerFactory.AddDebug();

            loggerFactory.AddSerilog();

            if (!env.IsDevelopment())
            {
                var options = new RewriteOptions().AddRedirectToHttps();
                app.UseRewriter(options);
                app.UseExceptionHandler();
            }
            else
            {
                app.UseDeveloperExceptionPage();
            }

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
                options.ConfigureOAuth2(
                    Configuration.GetValue<string>("SwaggerClientId"),
                    Configuration.GetValue<string>("SwaggerClientSecret"),
                    "http://localhost:5001",
                    "Swagger UI");
            });

            app.UseResponseCaching();

            appLifetime.ApplicationStopped.Register(Log.CloseAndFlush);
        }

        private static void AddMvcWithAuthorization(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvcCore(options =>
                {
                    // options.ReturnHttpNotAcceptable = true;
                    options.CacheProfiles.Add(
                        "Default",
                        new CacheProfile()
                        {
                            Duration = 60,
                            VaryByHeader = "Accept",
                            Location = ResponseCacheLocation.Any
                        });
                    options.CacheProfiles.Add(
                        "Never",
                        new CacheProfile()
                        {
                            Location = ResponseCacheLocation.None,
                            NoStore = true
                        });
                    options.Filters.Add(typeof(ExceptionLogFilter));
                    options.Filters.Add(typeof(RequestLogFilter));
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
                    // options.ReturnHttpNotAcceptable = true;
                    options.CacheProfiles.Add(
                        "Default",
                        new CacheProfile()
                        {
                            Duration = 60,
                            VaryByHeader = "Accept",
                            Location = ResponseCacheLocation.Any
                        });
                    options.CacheProfiles.Add(
                        "Never",
                        new CacheProfile()
                        {
                            Location = ResponseCacheLocation.None,
                            NoStore = true
                        });
                    options.Filters.Add(typeof(ExceptionLogFilter));
                    options.Filters.Add(typeof(RequestLogFilter));
                })
                .AddJsonFormatters()
                .AddApiExplorer();
        }
    }
}
