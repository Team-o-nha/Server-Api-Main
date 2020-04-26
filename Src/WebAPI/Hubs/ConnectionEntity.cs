using Microsoft.WindowsAzure.Storage.Table;

namespace ColabSpace.WebAPI.Hubs
{
    public class ConnectionEntity : TableEntity
    {
        public ConnectionEntity() { }

        public ConnectionEntity(string userID, string connectionID)
        {
            this.PartitionKey = userID;
            this.RowKey = connectionID;
        }
    }
}
