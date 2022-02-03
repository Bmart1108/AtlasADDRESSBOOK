using AtlasAddressbook.Models;




namespace AtlasAddressbook.Services.Interfaces
{
    public interface ICategoryService
    {
        Task AddContactToCategoryAsync(int categoryId, int contactId);

        Task AddContactToCategoriesAsync(List<int> categoryList, int contactId);


        Task RemoveContactFromCategoryAsync(int categoryId, int contactId);

        Task<ICollection<Category>> GetContactCategoriesAsync(int contactId);

        Task<ICollection<int>> GetContactCategoryIdsAsync(int contactId);

        Task<bool> IsContactInCategory(int categoryId, int contactId);

        Task<IEnumerable<Category>> GetUserCategoriesAsync(string userId);
        
    }
}