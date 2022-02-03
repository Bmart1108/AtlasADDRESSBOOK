using AtlasAddressbook.Data;
using AtlasAddressbook.Models;
using Microsoft.EntityFrameworkCore;

namespace AtlasAddressbook.Services
{
    public class SearchService
    {
        private readonly ApplicationDbContext _context;

        public SearchService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Contact> SearchContacts(string searchString, string userId)
        {
            var result = _context.Contacts.Include(c => c.Categories).Where(c => c.UserId == userId).AsQueryable();

            if (searchString is not null)
            {
                result = result.Where(c => c.Address1!.ToLower().Contains(searchString.ToLower())
                || c.Address2!.ToLower().Contains(searchString.ToLower())
                || c.City!.ToLower().Contains(searchString.ToLower())
                || c.Email!.ToLower().Contains(searchString.ToLower())
                || c.FirstName!.ToLower().Contains(searchString.ToLower())
                || c.LastName!.ToLower().Contains(searchString.ToLower())
                || c.Categories.Select(t => t.Name).Contains(searchString));
            }
            return result.OrderByDescending(c => c.FirstName);
        }

    }
}