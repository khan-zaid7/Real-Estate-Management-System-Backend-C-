﻿using System.ComponentModel.DataAnnotations;

namespace RealEstateManagement.ViewModel
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Name is required.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string? Email { get; set; }

        [Required(ErrorMessage = "PhoneNumber is required.")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "The {0} must be at {2} and at max {1} characters long.")]
        [DataType(DataType.Password)]
        [Compare("ConfirmPassword", ErrorMessage = "Password does not match.")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string? ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        [Display(Name = "Role")]
        public string? Role { get; set; }
    }
}
