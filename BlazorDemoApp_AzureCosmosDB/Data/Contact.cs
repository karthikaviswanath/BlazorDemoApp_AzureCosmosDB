namespace BlazorDemoApp_AzureCosmosDB.Data
{
    public class Contact
    {
        //Note : this model is used for binding with a NoSQL(json) DB - Azure Cosmos DB - so 'id' in smaller case
        public Guid? id { get; set; }
        public string? Name { get; set; }
        public string? Country { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }
}
