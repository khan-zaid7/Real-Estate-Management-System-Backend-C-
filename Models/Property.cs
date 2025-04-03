using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstateManagement.Models
{
    public class Property
    {
        public int Id { get; set; }

        // Basic Information
        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(2000)]
        public string Content { get; set; }

        // Property Type and Status
        [Required]
        public string PropertyType { get; set; }  // e.g., Residential, Commercial

        [Required]
        public string SellingType { get; set; } // e.g., Sale, Rent

        [Required]
        public string Status { get; set; } // e.g., Available, Sold, Rented

        // Room Details
        [Range(1, 10)]
        public int? Bathroom { get; set; }

        [Range(1, 10)]
        public int? Kitchen { get; set; }

        [Required]
        public string BHK { get; set; } // e.g., 1BHK, 2BHK, 3BHK

        [Range(1, 10)]
        public int? Bedroom { get; set; }

        [Range(1, 10)]
        public int? Balcony { get; set; }

        [Range(1, 10)]
        public int? Hall { get; set; }

        // Price & Location
        [Required]
        public int Floor { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        [StringLength(100)]
        public string City { get; set; }

        [Required]
        [StringLength(100)]
        public string State { get; set; }

        [Required]
        public int TotalFloor { get; set; }

        [Required]
        public int AreaSize { get; set; } // in sqft

        [Required]
        [StringLength(200)]
        public string Address { get; set; }

        // Feature Information
        [Required]
        public string Feature { get; set; } // Yes/No or Details as per user input

        // Image & Status
        public string Image1 { get; set; } // Path or URL for Image 1
        public string Image2 { get; set; } // Path or URL for Image 2
        public string Image3 { get; set; } // Path or URL for Image 3
        public string Image4 { get; set; } // Path or URL for Image 4

        // Plan Images (Basement, Floor, Ground)
        public string FloorPlanImage { get; set; }

        // Is Featured property
        [Required]
        public bool IsFeatured { get; set; }

        // **User Relationship**
        [Required]
        public string UserId { get; set; } // Foreign Key

        [ForeignKey("UserId")]
        public User User { get; set; } // Navigation Property
    }
}
