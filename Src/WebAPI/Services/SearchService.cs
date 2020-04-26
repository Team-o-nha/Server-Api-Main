using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Application.MessageChats.Models;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ColabSpace.WebAPI.Services
{
    public class SearchService : ISearchService
    {
        private static ISearchServiceClient _searchClient;
        private static ISearchIndexClient _indexMessagesClient;
        private static ISearchIndexClient _indexConversationsClient;
        private static ISearchIndexClient _indexAzureBlobClient;

        public SearchService(IConfiguration config)
        {
            _searchClient = new SearchServiceClient(config["Azure:Search:SearchServiceName"], 
                new SearchCredentials(config["Azure:Search:SearchServiceQueryApiKey"]));
            _indexMessagesClient = _searchClient.Indexes.GetClient(config["Azure:Search:SearchServiceMessagesIndexName"]);
            _indexConversationsClient = _searchClient.Indexes.GetClient(config["Azure:Search:SearchServiceConversationsIndexName"]);
            _indexAzureBlobClient = _searchClient.Indexes.GetClient(config["Azure:Search:SearchServiceAzureblobIndexName"]);
        }

        public ISearchIndexClient MessagesClient => _indexMessagesClient;

        public ISearchIndexClient ConversationsClient => _indexConversationsClient;

        public ISearchIndexClient AzureBlobClient => _indexAzureBlobClient;

        public List<MessageChatModel> SearchMessages(string userId, string searchText)
        {
            try
            {
                var lstMessages = new List<MessageChatModel>();
                var conversationIds = GetConversationIds(userId);

                // If blank search, assume they want to search everything
                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    searchText += SearchBolbStorageFiles(searchText);
                } else
                {
                    searchText = "*";
                }

                var parameters = new SearchParameters()
                {
                    Filter = $"search.in(ConversationId, '{conversationIds}', ',')",
                    SearchMode = SearchMode.All
                };

                var results = _indexMessagesClient.Documents.Search(searchText, parameters).Results;
                foreach (SearchResult<Document> result in results)
                {
                    var message = new MessageChatModel()
                    {
                        Id = Guid.Parse((string)result.Document.GetValueOrDefault("Id")),
                        Content = (string)result.Document.GetValueOrDefault("Content"),
                        ConversationId = Guid.Parse((string)result.Document.GetValueOrDefault("ConversationId")),
                        RegUserName = (string)result.Document.GetValueOrDefault("RegUserName"),
                        Created = ((DateTimeOffset)result.Document.GetValueOrDefault("Created")).UtcDateTime
                    };
                    lstMessages.Add(message);
                }

                return lstMessages;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error querying index: {0}\r\n", ex.Message.ToString());
            }

            return null;
        }

        private string GetConversationIds(string userId)
        {
            string conversationIds = "";

            var parameters = new SearchParameters()
            {
                Filter = $"Members/any(member: member/UserOid eq '{userId}')",
                Top = 100, //Default is 50
                Select = new[] { "Id" }
            };

            var results = _indexConversationsClient.Documents.Search("*", parameters).Results;

            foreach (SearchResult<Document> result in results)
            {
                conversationIds = conversationIds + (string)result.Document.GetValueOrDefault("Id") + ",";
            }
            
            if (conversationIds.Length > 0)
            {
                conversationIds = conversationIds.Substring(0, conversationIds.Length - 1);
            }

            return conversationIds;
        }

        private string SearchBolbStorageFiles(string searchText)
        {
            string fileNames = "";

            var parameters = new SearchParameters()
            {
                Select = new[] { "metadata_storage_name" }
            };

            var results = _indexAzureBlobClient.Documents.Search(searchText, parameters).Results;

            foreach (SearchResult<Document> result in results)
            {
                fileNames = fileNames + "|" + (string)result.Document.GetValueOrDefault("metadata_storage_name");
            }

            return fileNames;
        }
    }
}
