using System.Collections.Generic;
using System.Linq;
using Api.Filters;
using Autofac;
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

            ConfigureMvc(services);

            ConfigureAuthorization(services);

            ConfigureSwagger(services);

            ConfigureOptions(services);

            ConfigureCache(services);
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new AutofacModule(_configuration));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
        {
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

            var origins = _configuration.GetSection("CorsOrigins").GetChildren().Select(o => o.Value).ToArray();
            app.UseCors(builder => builder.WithOrigins(origins)
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseResponseCaching();

            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("v1/swagger.json", "dotnetcore-api v1");
                options.ConfigureOAuth2(
                    _configuration.GetValue<string>("SwaggerClientId"),
                    _configuration.GetValue<string>("SwaggerClientSecret"),
                    "http://localhost:5001", // TODO: not the correct realm for deployed app...
                    "Swagger UI");
                options.ShowRequestHeaders();
            });

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
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info()
                {
                    Title = "dotnetcore-api v1",
                    Version = "v1",
                    Description = "Prototype .NET Core API"
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

                options.OperationFilter<AuthrorizationOperationFilter>();
                options.TagActionsBy(apidesc => apidesc.FormatForSwaggerActionGroup());
                options.DocumentFilter<LowercaseDocumentFilter>();
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
            })
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                })
                .AddAuthorization()
                .AddJsonFormatters()
                .AddApiExplorer();
        }

        private void ConfigureOptions(IServiceCollection services)
        {
            services.Configure<ConnectionStrings>(options =>
            {
                options.SqlAdventure =
                    _configuration.GetConnectionString("SqlAdventure");
            });
        }
    }
}
