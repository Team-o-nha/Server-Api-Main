using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Domain.Interfaces;
using ColabSpace.Infrastructure.Persistence;
using IdentityModel.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using System;
using System.Net.Http;
using System.Security;
using System.Threading.Tasks;
using Moq;
using System.IO;
using Microsoft.AspNetCore.Http;
using Xunit;
using Microsoft.AspNetCore.SignalR.Client;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace ColabSpace.WebAPI.IntegrationTests2.Common
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        private static readonly string Authority = "https://colabspaceappb2c.b2clogin.com/tfp/colabspaceappb2c.onmicrosoft.com/B2C_1_ROPC_Auth";
        private static readonly string ClientId = "b45e1a0d-cd0f-4c58-98d8-44fa5ba507f1";
        private static readonly string[] ApiScope = new string[] { "https://colabspaceappb2c.onmicrosoft.com/api/colabspace.read", "https://colabspaceappb2c.onmicrosoft.com/api/colabspace.write" };
        private static readonly string UserName = "testuser";
        private static readonly string PassWord = "testuser";
        //emulator
        private static readonly string privateKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        private static readonly string databaseName = "ColabSpaceDb";
        private static string entpoint = "https://localhost:8081";

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            string endPointEnvironment = Environment.GetEnvironmentVariable("CosmosDBEndpoint", EnvironmentVariableTarget.Process);
            Console.WriteLine($"CosmosDBEndpoint from pipeline {endPointEnvironment}");

            if (!string.IsNullOrWhiteSpace(endPointEnvironment))
            {
                entpoint = endPointEnvironment;
            }
            builder
               .ConfigureServices(services =>
                {
                    string endPointEnvironment = Environment.GetEnvironmentVariable("CosmosDBEndpoint", EnvironmentVariableTarget.Process);
                    Console.WriteLine($"CosmosDBEndpoint from pipeline {endPointEnvironment}");

                    if (!string.IsNullOrWhiteSpace(endPointEnvironment))
                    {
                        entpoint = endPointEnvironment;
                    }
                    var serviceProvider = new ServiceCollection().AddEntityFrameworkCosmos().BuildServiceProvider();
                    //Add a database context using cosmos emulator
                    services.AddDbContext<ColabSpaceDbContext>(options =>
                    {
                        options.UseCosmos(entpoint, privateKey, databaseName);
                        options.UseApplicationServiceProvider(serviceProvider);
                    });
                    services.AddScoped<ICurrentUserService, TestCurrentUserService>();
                    services.AddScoped<IDateTime, TestDateTimeService>();
                    services.AddScoped<IColabSpaceDbContext>(provider => provider.GetService<ColabSpaceDbContext>());
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

        public ColabSpaceDbContext InitializeDbForTests()
        {
            var scope = this.Server.Host.Services.CreateScope();
            ColabSpaceDbContext context = scope.ServiceProvider.GetService<ColabSpaceDbContext>();
            context.Database.EnsureCreated();
            Utilities.InitializeDbForTests(context);
            return context;
        }

        public void DisposeDbForTests(ColabSpaceDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        public TService GetService<TService>()
    where TService : class
        {
            return Server?.Host?.Services?.GetService(typeof(TService)) as TService;
        }

        public MultipartFormDataContent MockUploadFileAction(string fileName, string data)
        {
            var fileMock = new Mock<IFormFile>();
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(data);
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);
            var file = fileMock.Object;
            var formData = new MultipartFormDataContent();
            // Add file (file, field name, file name)
            formData.Add(new StreamContent(ms), "files", fileName);

            return formData;
        }

        public async Task<string> GetAccessTokenByUserAsync(string userName, string password)
        {
            var app = PublicClientApplicationBuilder.Create(ClientId)
               .WithB2CAuthority(Authority)
               .Build();

            SecureString secure = new SecureString();
            foreach (char ch in password)
                secure.AppendChar(ch);

            var ar = await app.AcquireTokenByUsernamePassword(ApiScope, userName, secure).ExecuteAsync();
            return ar.AccessToken;
        }
    }
}
