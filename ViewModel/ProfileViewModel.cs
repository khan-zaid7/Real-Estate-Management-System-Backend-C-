using System.ComponentModel.DataAnnotations;

namespace RealEstateManagement.ViewModel
{
    public class ProfileViewModel
    {
        [Required(ErrorMessage = "Full Name is required.")]
        [StringLength(100, ErrorMessage = "Full Name cannot be longer than 100 characters.")]
        public string? FullName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Phone Number is required.")]
        [Phone(ErrorMessage = "Invalid Phone Number format.")]
        [StringLength(15, ErrorMessage = "Phone Number cannot be longer than 15 characters.")]
        public string? PhoneNumber { get; set; }

        [Required]
        public string? Role { get; set; } // Role remains read-only
    }
}
