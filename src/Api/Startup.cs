using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Api.Conventions;
using Api.Filters;
using AspNetCoreRateLimit;
using Autofac;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using SqlAdventure;
using Swashbuckle.AspNetCore.Swagger;

namespace Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;

        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(_configuration);
            services.AddCors();
            services.AddResponseCaching();
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddOptions();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper, UrlHelper>(serviceProvider =>
            {
                var actionContext = serviceProvider.GetService<IActionContextAccessor>().ActionContext;
                return new UrlHelper(actionContext);
            });

            ConfigureCache(services);

            ConfigureRateLimiting(services);

            ConfigureMvc(services);

            ConfigureAuthorization(services);

            ConfigureSwagger(services);

            ConfigureOptions(services);

            services.AddAutoMapper(Assembly.GetAssembly(typeof(SqlAdventureModule)), Assembly.GetAssembly(typeof(ApiModule)));
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new ApiModule());
            builder.RegisterModule(new SqlAdventureModule());
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
        {
            app.UseCustomtExceptionHandler();

            if (!env.IsDevelopment())
            {
                var options = new RewriteOptions().AddRedirectToHttps();
                app.UseRewriter(options);
            }

            var origins = _configuration.GetSection("CorsOrigins").GetChildren().Select(o => o.Value).ToArray();
            app.UseCors(builder => builder.WithOrigins(origins)
                .AllowAnyHeader());

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("public/swagger.json", "ASP.NET Core API: Public");
                options.SwaggerEndpoint("admin/swagger.json", "ASP.NET Core API: Admin");
                options.ConfigureOAuth2(
                    _configuration.GetValue<string>("SwaggerClientId"),
                    _configuration.GetValue<string>("SwaggerClientSecret"),
                    "http://localhost:8081", // TODO: not the correct realm for deployed app...
                    "Swagger UI");
                options.ShowRequestHeaders();
            });

            app.UseIpRateLimiting();

            app.UseAuthentication();
            app.UseResponseCaching();

            app.UseETags();

            app.UseMvc();
        }

        private void ConfigureCache(IServiceCollection services)
        {
            var useInMemoryCache = _configuration.GetValue<bool>("UseInMemoryCache");

            if (useInMemoryCache)
            {
                services.AddDistributedMemoryCache();
            }
            else
            {
                services.AddDistributedRedisCache(option =>
                {
                    option.Configuration = _configuration.GetConnectionString("Redis");
                    option.InstanceName = _configuration.GetValue<string>("RedisInstanceName");
                });
            }
        }

        private void ConfigureSwagger(IServiceCollection services)
        {
            var apiDocs = Path.Combine(System.AppContext.BaseDirectory, "Api.xml");
            var coreDocs = Path.Combine(System.AppContext.BaseDirectory, "Core.xml");

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("public", new Info()
                {
                    Title = "Public Adventure API",
                    Version = "v1",
                    Description = "Prototype ASP.NET Core API using the AdventureWorks database. Public section. No authorization required.",
                });

                options.SwaggerDoc("admin", new Info()
                {
                    Title = "Admin Adventure API",
                    Version = "v1",
                    Description = "Prototype ASP.NET Core API using the AdventureWorks database. Admin section. Authorization required."
                });

                options.AddSecurityDefinition("oauth2", new OAuth2Scheme
                {
                    Type = "oauth2",
                    Flow = "implicit",
                    AuthorizationUrl = _configuration.GetValue<string>("IdentityServer") + "/connect/authorize",
                    TokenUrl = _configuration.GetValue<string>("IdentityServer") + "/connect/token",
                    Scopes = new Dictionary<string, string>
                    {
                        { "api1", "API Access" }
                    }
                });

                options.OperationFilter<ResponseOperationFilter>();
                options.DocumentFilter<LowercaseDocumentFilter>();
                options.IncludeXmlComments(coreDocs);
                options.IncludeXmlComments(apiDocs);
            });
        }

        private void ConfigureAuthorization(IServiceCollection services)
        {
            var authority = _configuration.GetValue<string>("IdentityServer");
            var requireHttpsMetaData = !_hostingEnvironment.IsDevelopment();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Audience = "api1";
                    options.Authority = authority;
                    options.RequireHttpsMetadata = requireHttpsMetaData;
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    "Default", policy => policy.RequireAuthenticatedUser());
            });
        }

        private void ConfigureMvc(IServiceCollection services)
        {
            services.AddMvcCore(options =>
                {
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
                    options.Filters.Add(typeof(RequestLogFilter));
                    options.Filters.Add(typeof(ExceptionLogFilter));
                    options.Filters.Add(new ProducesAttribute("application/json"));
                    options.ReturnHttpNotAcceptable = true;
                    options.Conventions.Add(new ApiExplorerGroupPerNamespaceConvention());
                })
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                })
                .AddAuthorization()
                .AddJsonFormatters()
                .AddApiExplorer()
                .AddDataAnnotations();
        }

        private void ConfigureOptions(IServiceCollection services)
        {
            services.Configure<ConnectionStrings>(options =>
            {
                options.SqlAdventure =
                    _configuration.GetConnectionString("SqlAdventure");
            });
        }

        private void ConfigureRateLimiting(IServiceCollection services)
        {
            // More configuration information at https://github.com/stefanprodan/AspNetCoreRateLimit
            services.Configure<IpRateLimitOptions>(_configuration.GetSection("IpRateLimiting"));
            services.Configure<IpRateLimitPolicies>(_configuration.GetSection("IpRateLimitPolicies"));

            services.AddSingleton<IIpPolicyStore, DistributedCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, DistributedCacheRateLimitCounterStore>();
        }
    }
}
