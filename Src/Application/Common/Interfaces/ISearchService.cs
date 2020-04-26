using ColabSpace.Application.MessageChats.Models;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using System.Collections.Generic;

namespace ColabSpace.Application.Common.Interfaces
{
    public interface ISearchService
    {
        ISearchIndexClient MessagesClient { get; }

        ISearchIndexClient ConversationsClient { get; }

        List<MessageChatModel> SearchMessages(string userId, string searchText);
    }
}
