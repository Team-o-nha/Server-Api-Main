using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;

namespace FunctionNotification
{
    public static class OnNotificationsChanged
    {
        public class ConnectionEntity : TableEntity
        {
            public ConnectionEntity() { }
        }

        [FunctionName("OnNotificationsChanged")]
        public static async Task Run(
            [CosmosDBTrigger(
                databaseName: "ColabSpaceDb",
                collectionName: "Notifications",
                ConnectionStringSetting = "AzureWebJobsCosmosDBConnectionString",
                CreateLeaseCollectionIfNotExists = true),]
                IEnumerable<object> updatedNotifications,
            [Table("connection")] CloudTable cloudTable,
            [SignalR(HubName = "chathub")] IAsyncCollector<SignalRMessage> signalRMessages,
            ILogger log)
        {
            foreach (var notification in updatedNotifications)
            {
                var doc = (Document)notification;
                var toUser = doc.GetPropertyValue<Document>("ToUser");
                var userId = toUser.GetPropertyValue<string>("UserOid");

                foreach (var entity in GetConnectionEntitys(cloudTable, userId))
                {
                    await signalRMessages.AddAsync(new SignalRMessage
                    {
                        Target = "OnNotificationsChanged",
                        ConnectionId = entity.RowKey,
                        Arguments = new[] { notification }
                    });
                }
            }
        }

        private static TableQuerySegment<ConnectionEntity> GetConnectionEntitys(CloudTable cloudTable, string key)
        {
            var query = new TableQuery<ConnectionEntity>()
                .Where(TableQuery.GenerateFilterCondition(
                "PartitionKey",
                QueryComparisons.Equal,
                key));

            return cloudTable.ExecuteQuerySegmentedAsync(query, null).Result;
        }
    }
}
