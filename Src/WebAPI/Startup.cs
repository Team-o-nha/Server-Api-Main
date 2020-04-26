using ColabSpace.Application;
using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Infrastructure.Persistence;
using ColabSpace.WebAPI.Common;
using ColabSpace.WebAPI.Services;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSwag;
using NSwag.Generation.Processors.Security;
using System.Threading.Tasks;
using System.Linq;
using ColabSpace.Infrastructure;
using ColabSpace.WebAPI.Hubs;

namespace ColabSpace.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            configuration = builder.Build();

            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddInfrastructure(Configuration, Environment);
            services.AddApplication();

            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IBlobStorageService, BlobStorageService>();
            services.AddScoped<ISearchService, SearchService>();

            services.AddHealthChecks()
                .AddDbContextCheck<ColabSpaceDbContext>();

            services.AddControllersWithViews()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<IColabSpaceDbContext>())
                .AddNewtonsoftJson();

            services.AddCors(o => o.AddPolicy("AllowOrigin", builder => {
                builder.AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithOrigins("https://colabspacepocfrontend.z11.web.core.windows.net", 
                                "https://colabspacepocstaging.z11.web.core.windows.net",
                                "https://colabspacesmartphone.z31.web.core.windows.net",
                                "https://app.collab-space.com",
                                "http://localhost:4200")
                    .AllowCredentials();
            }));

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwtOptions =>
            {
                jwtOptions.Authority = $"https://{Configuration["AzureAdB2C:B2CName"]}.b2clogin.com/{Configuration["AzureAdB2C:Tenant"]}/{Configuration["AzureAdB2C:Policy"]}/v2.0";
                jwtOptions.Audience = Configuration["AzureAdB2C:ClientId"];
                jwtOptions.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var isChat = context.Request.Path.Value.StartsWith("/chathub");

                        if (!string.IsNullOrEmpty(accessToken) && isChat)
                        {
                            context.Token = context.Request.Query["access_token"];
                        }
                        return Task.CompletedTask;
                    }
                };
            });
            // Add framework services.
            services
                .AddMvc(options =>
                {
                    options.EnableEndpointRouting = false;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            // Customise default API behaviour
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            //SignalR
            if (string.IsNullOrEmpty(Configuration["Azure:SignalR:ConnectionString"]))
            {
                services.AddSignalR();
            } 
            else
            {
                services.AddSignalR(options => options.EnableDetailedErrors = true)
                   .AddAzureSignalR(options =>
                   {
                       options.ConnectionString = Configuration["Azure:SignalR:ConnectionString"];
                   });
            }
           

            // Register the Swagger services
            services.AddOpenApiDocument(config =>
            {
                config.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = "ColabSpace API";
                    document.Info.Description = "ColabSpace .NET Core 3.0 web API";
                };
                config.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Description = "Type into the textbox: Bearer {your JWT token}."
                });

                config.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCustomExceptionHandler();
            app.UseHealthChecks("/health");
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // Register the Swagger generator and the Swagger UI middlewares
            app.UseOpenApi();
            app.UseSwaggerUi3(settings =>
            {
                settings.Path = "/api";
                settings.DocumentPath = "/api/specification.json";
            });  // Serves the Swagger UI 3 web ui to view the OpenAPI/Swagger documents by default on `/swagger`

            // Enabling CORS Globally
            app.UseCors("AllowOrigin");

            app.UseAuthentication();

            app.UseMvc();
            app.UseFileServer();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(routes =>
            {
                routes.MapHub<ChatHub>("/chathub");
            });
        }
    }
}
