using Microsoft.AspNetCore.Identity;

namespace RealEstateManagement.Models
{
    public class User : IdentityUser
    {
        public string? FullName { get; set; }
        public ICollection<Property> Properties { get; set; } = new List<Property>();
    }
}
