using AtlasAddressbook.Data;
using AtlasAddressbook.Enums;
using AtlasAddressbook.Models;
using AtlasAddressbook.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AtlasAddressbook.Services
{
    public class DataService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly ICategoryService _categoryService;

        public DataService(ApplicationDbContext context, UserManager<AppUser> userManager, ICategoryService categoryService)
        {
            _context = context;
            _userManager = userManager;
            _categoryService = categoryService;
        }

        public async Task ManageDataAsync()
        {
            await _context.Database.MigrateAsync();

            //Custom Address Book seeding methods
            await SeedDefaultUserAsync(_userManager);
            await SeedDefaultContacts(_context);
            await SeedDefaultCategoriesAsync(_context);
            await DefaultCategoryAssign(_categoryService, _context);
        }

        private async Task SeedDefaultUserAsync(UserManager<AppUser> userManagerSvc)
        {
            var defaultUser = new AppUser
            {
                UserName = "tonystark@mailinator.com",
                Email = "tonystark@mailinator.com",
                FirstName = "Tony",
                LastName = "Stark",
                EmailConfirmed = true,
            };
            try
            {
                var user = await userManagerSvc.FindByNameAsync(defaultUser.Email);
                if (user is null)
                {
                    await userManagerSvc.CreateAsync(defaultUser, "Abc&123!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("**** ERROR ****");
                Console.WriteLine("Error Seeding Default User");
                Console.WriteLine(ex.Message);
                Console.WriteLine("**** END OF ERROR ****");
            }
        }

        private async Task SeedDefaultContacts(ApplicationDbContext dbContextSvc)
        {
            var userId = dbContextSvc.Users.FirstOrDefault(u => u.Email == "tonystark@mailinator.com")!.Id;

            var defaultContact = new Contact
            {
                UserId = userId,
                Created = DateTime.UtcNow,
                FirstName = "Henry",
                LastName = "McCoy",
                Address1 = "1407 Graymalkin Ln.",
                City = "Salem Center",
                PhoneNumber = "555-555-0101",
                State = nameof(States.NY),
                Email = "hankmccoy@starktower.com"
            };
            try
            {
                var contact = await dbContextSvc.Contacts.AnyAsync(c => c.Email == "hankmccoy@starktower.com" && c.UserId == userId);
                if (!contact)
                {
                    await dbContextSvc.AddAsync(defaultContact);
                    await dbContextSvc.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("**** ERROR ****");
                Console.WriteLine("Error Seeding Default Contact");
                Console.WriteLine(ex.Message);
                Console.WriteLine("**** END OF ERROR ****");
            }

        }

        private async Task SeedDefaultCategoriesAsync(ApplicationDbContext dbContextSvc)
        {
            var userId = dbContextSvc.Users.FirstOrDefault(u => u.Email == "tonystark@mailinator.com")!.Id;

            var defaultCategory = new Category
            {
                UserId = userId,
                Name = "X-Men"
            };
            try
            {
                var category = await dbContextSvc.Categories.AnyAsync(c => c.Name == "X-Men" && c.UserId == userId);
                if (!category)
                {
                    await dbContextSvc.AddAsync(defaultCategory);
                    await dbContextSvc.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("**** ERROR ****");
                Console.WriteLine("Error Seeding Default Category");
                Console.WriteLine(ex.Message);
                Console.WriteLine("**** END OF ERROR ****");
            }

        }

        private async Task DefaultCategoryAssign(ICategoryService categorySvc, ApplicationDbContext dbContextSvc)
        {
            var user = dbContextSvc.Users
                .Include(c => c.Categories)
                .Include(c => c.Contacts)
                .FirstOrDefault(u => u.Email == "tonystark@mailinator.com");

            foreach (var contact in user!.Contacts)
            {
                foreach (var category in user.Categories)
                {
                    await categorySvc.AddContactToCategoryAsync(category.Id, contact.Id);
                }
            }

        }
    }
}