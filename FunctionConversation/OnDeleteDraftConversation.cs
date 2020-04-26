using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace FunctionConversation
{
    public static class OnDeleteDraftConversation
    {
        [FunctionName("OnDeleteDraftConversation")]
        public static async Task RunAsync([TimerTrigger("0 0 0 * * *")]TimerInfo myTimer,
            [CosmosDB(
                databaseName: "ColabSpaceDb",
                collectionName: "Conversations",
                ConnectionStringSetting = "AzureWebJobsCosmosDBConnectionString"),]
            IEnumerable<Document> conversations,
            [CosmosDB(
                databaseName: "ColabSpaceDb",
                collectionName: "MessageChats",
                ConnectionStringSetting = "AzureWebJobsCosmosDBConnectionString"),]
            IEnumerable<Document> messages,
            [CosmosDB(
                databaseName: "ColabSpaceDb",
                collectionName: "Conversations",
                ConnectionStringSetting = "AzureWebJobsCosmosDBConnectionString"),]
            DocumentClient conversationsClient,
            ILogger log)
        {
            foreach (var conversation in conversations)
            {
                var id = conversation.GetPropertyValue<string>("Id");
                var type = conversation.GetPropertyValue<string>("Type");
                if (type != "channel")
                {
                    var messagesOfConv = messages.Where(x =>
                        x.GetPropertyValue<string>("ConversationId") == id).ToList();
                    if (messagesOfConv == null || messagesOfConv.Count() == 0)
                    {
                        RequestOptions requestOptions = new RequestOptions();
                        requestOptions.PartitionKey = new PartitionKey(Undefined.Value);
                        await conversationsClient.DeleteDocumentAsync(conversation.SelfLink, requestOptions);
                    }
                }
            }
        }
    }
}
