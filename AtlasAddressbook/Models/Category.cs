using System.ComponentModel.DataAnnotations;

namespace AtlasAddressbook.Models
{
    public class Category
    {
        //Primary Key
        public int Id { get; set; }

        //Foreign Key
        public string? UserId { get; set; }

        [Required]
        [Display(Name = "Category Name")]
        public string? Name { get; set; }

        //Navigation Properties
        public virtual AppUser? User { get; set; }

        public virtual ICollection<Contact> Contacts { get; set; } = new HashSet<Contact>();


    }
}
