using Microsoft.AspNetCore.Identity;

namespace RealEstateManagement.Models
{
    public class User : IdentityUser
    {
        public string? FullName { get; set; }
    }
}
