using BlazorDemoApp_AzureCosmosDB.Data;
using Microsoft.Azure.Cosmos;

namespace BlazorDemoApp_AzureCosmosDB.Services
{
    public class ContactService : IContactService
    {
        private readonly string CosmosDBConnectionString = "AccountEndpoint=https://az-cosmos-demo-db.documents.azure.com:443/;AccountKey=tH6tZkgTpZtd17qLL2DXgCjrXaEP3btemjRcMshtEFM11h6eMstOSmMvTqFGBtZ93jhaLJLOQvU3ACDbRdP9MQ==;";
        private readonly string CosmosDBName = "CosmosApp";
        private readonly string CosmosDBContainerName = "Contact";

        private Container GetContainerClient()
        {
            var cosmosDbClient = new CosmosClient(CosmosDBConnectionString);
            var container = cosmosDbClient.GetContainer(CosmosDBName, CosmosDBContainerName);
            return container;
        }

        public async Task UpsertContact(Contact contact)
        {
            try
            {
                if(contact.id == null)
                {
                    contact.id = Guid.NewGuid();
                }
                var container = GetContainerClient();
                var response = await container.UpsertItemAsync(contact,
                    new PartitionKey(contact.id.ToString()));
                Console.WriteLine(response.StatusCode);

            }
            catch (Exception ex)
            {
                throw new Exception("Exception", ex);
            }
        }

        // Note : partition key is the combination of partition key value and primary key (id) value
        // in this case partition key is also id itself - so we could also eliminate partition key input 
        public async Task DeleteContact(string? id, string? partitionkey)
        {
            try
            {
                var container = GetContainerClient();
                var response = await container.DeleteItemAsync<Contact>(id,
                    new PartitionKey(partitionkey));
                Console.WriteLine(response.StatusCode);

            }
            catch (Exception ex)
            {
                throw new Exception("Exception", ex);
            }
        }

        public async Task<List<Contact>> GetContactDetails()
        {
            List<Contact> contacts = new List<Contact>();
            try
            {

                var container = GetContainerClient();
                var sqlQuery = "SELECT * FROM c";
                QueryDefinition queryDefinition = new QueryDefinition(sqlQuery);
                FeedIterator<Contact> queryResultSetIterator = container.GetItemQueryIterator<Contact>(queryDefinition);

                while (queryResultSetIterator.HasMoreResults)
                {
                    FeedResponse<Contact> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                    foreach (Contact contact in currentResultSet)
                    {
                        contacts.Add(contact);
                    }
                }

            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            return contacts;
        }

        public async Task<Contact> GetContactDetailsById(string? id, string? partitionKey)
        {
            try
            {
                var container = GetContainerClient();
                ItemResponse<Contact> response = await container.ReadItemAsync<Contact>(id, new PartitionKey(partitionKey));
                return response.Resource;
            }
            catch (Exception ex)
            {

                throw new Exception("Exception ", ex);
            }
        }
    }
}
