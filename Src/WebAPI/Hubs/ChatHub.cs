using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage;
using Microsoft.Extensions.Configuration;

namespace ColabSpace.WebAPI.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ChatHub : Hub
    {
        string LoginUserId => Context?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        private readonly IConfiguration _config;

        public ChatHub(IConfiguration config)
        {
            _config = config;
        }

        [HubMethodName("AddToGroup")]
        public async Task AddToGroup(string conversationId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, conversationId);
        }

        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

        public override async Task OnConnectedAsync()
        {
            var table = GetConnectionTable();
            await table.CreateIfNotExistsAsync();
            var entity = new ConnectionEntity(
                LoginUserId,
                Context.ConnectionId);
            var insertOperation = TableOperation.InsertOrReplace(entity);
            await table.ExecuteAsync(insertOperation);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var table = GetConnectionTable();
            var deleteOperation = TableOperation.Delete(
                new ConnectionEntity(LoginUserId, Context.ConnectionId) { ETag = "*" });
            await table.ExecuteAsync(deleteOperation);

            await base.OnDisconnectedAsync(exception);
        }

        private CloudTable GetConnectionTable()
        {
            var storageAccount =
                CloudStorageAccount.Parse(_config["BlobStorage:ConnectionString"]);
            var tableClient = storageAccount.CreateCloudTableClient();

            return tableClient.GetTableReference("connection");
        }
    }
}
