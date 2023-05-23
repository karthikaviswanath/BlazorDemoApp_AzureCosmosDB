using BlazorDemoApp_AzureCosmosDB.Data;

namespace BlazorDemoApp_AzureCosmosDB.Services
{
    public interface IContactService
    {
        Task UpsertContact(Contact contact); //Method for both adding and updating contact
        Task DeleteContact(string? id, string? partitionkey);
        Task<List<Contact>> GetContactDetails();
        Task<Contact> GetContactDetailsById(string? id, string? partitionKey);
    }
}