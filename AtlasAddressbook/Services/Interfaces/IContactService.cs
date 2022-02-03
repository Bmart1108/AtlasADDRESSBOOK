using AtlasAddressbook.Models;

namespace AtlasAddressbook.Services.Interfaces
{
    public interface IContactService
    {
        public Task<Contact> GetContactByIdAsync(int contactId);
    }
}
