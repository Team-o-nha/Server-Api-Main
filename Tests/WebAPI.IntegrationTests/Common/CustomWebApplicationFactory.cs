using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Domain.Interfaces;
using ColabSpace.Infrastructure.Persistence;
using IdentityModel.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using System;
using System.Net.Http;
using System.Security;
using System.Threading.Tasks;

namespace ColabSpace.WebAPI.IntegrationTests.Common
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        private static readonly string Authority = "https://colabspaceappb2c.b2clogin.com/tfp/colabspaceappb2c.onmicrosoft.com/B2C_1_ROPC_Auth";
        private static readonly string ClientId = "b45e1a0d-cd0f-4c58-98d8-44fa5ba507f1";
        private static readonly string[] ApiScope = new string[] { "https://colabspaceappb2c.onmicrosoft.com/api/colabspace.read", "https://colabspaceappb2c.onmicrosoft.com/api/colabspace.write" };
        private static readonly string UserName = "testuser";
        private static readonly string PassWord = "testuser";
        //emulator
        //private static readonly string privateKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        //private static readonly string databaseName = "ColabSpaceDb";
        //private static readonly string entpoint = "https://localhost:8081";

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder
               .ConfigureServices(services =>
                {
                    // Create a new service provider -> use InMemoryDatabase
                    var serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                    // Add a database context using an in-memory 
                    // database for testing.
                    services.AddDbContext<ColabSpaceDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("InMemoryDbForTesting");
                        options.UseInternalServiceProvider(serviceProvider);
                    });

                    // Create a new service provider -> use ICosmosDB Emulator
                    //var serviceProvider = new ServiceCollection().BuildServiceProvider();
                    // Add a database context using cosmos emulator
                    //services.AddDbContext<ColabSpaceDbContext>(options =>
                    //{
                    //    options.UseCosmos(entpoint, privateKey, databaseName);
                    //    options.UseApplicationServiceProvider(serviceProvider);
                    //});

                    services.AddScoped<IColabSpaceDbContext>(provider => provider.GetService<ColabSpaceDbContext>());

                    services.AddScoped<ICurrentUserService, TestCurrentUserService>();
                    services.AddScoped<IDateTime, TestDateTimeService>();

                    var sp = services.BuildServiceProvider();

                    // Create a scope to obtain a reference to the database
                    using var scope = sp.CreateScope();
                    var scopedServices = scope.ServiceProvider;
                    var context = scopedServices.GetRequiredService<ColabSpaceDbContext>();
                    var logger = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                    // Ensure the database is created.
                    context.Database.EnsureCreated();

                    try
                    {
                        // Seed the database with test data.
                        Utilities.InitializeDbForTests(context);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred seeding the " +
                                            $"database with test messages. Error: {ex.Message}");
                    }
                })
                .UseEnvironment("Test");
        }

        public HttpClient GetAnonymousClient()
        {
            return CreateClient();
        }

        public async Task<HttpClient> GetAuthenticatedClientAsync()
        {
            var client = CreateClient();

            var token = await GetAccessTokenAsync();

            client.SetBearerToken(token);

            return client;
        }

        private async Task<string> GetAccessTokenAsync()
        {
            var app = PublicClientApplicationBuilder.Create(ClientId)
               .WithB2CAuthority(Authority)
               .Build();

            SecureString secure = new SecureString();
            foreach (char ch in PassWord)
                secure.AppendChar(ch);

            var ar = await app.AcquireTokenByUsernamePassword(ApiScope, UserName, secure).ExecuteAsync();
            return ar.AccessToken;
        }
    }
}
