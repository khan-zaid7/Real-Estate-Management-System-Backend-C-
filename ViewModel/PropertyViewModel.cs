﻿using System.ComponentModel.DataAnnotations;

public class PropertyViewModel
{
    public int Id { get; set; }

    // Basic Information
    [Required]
    [MinLength(5)]
    [MaxLength(100)]
    public string Title { get; set; }

    [Required]
    [StringLength(2000)]
    public string Content { get; set; } // Description

    // Property Type and Status
    [Required]
    public string PropertyType { get; set; }

    [Required]
    public string SellingType { get; set; }

    [Required]
    public string Status { get; set; }

    // Room Details
    [Required]
    [Range(1, 10)]
    public int Bathroom { get; set; }

    [Required]
    [Range(1, 10)]
    public int Kitchen { get; set; }

    [Required]
    public string BHK { get; set; }

    [Required]
    [Range(1, 10)]
    public int Bedroom { get; set; }

    [Required]
    [Range(1, 10)]
    public int Balcony { get; set; }

    [Required]
    [Range(1, 10)]
    public int Hall { get; set; }

    // Price & Location
    [Required]
    public int Floor { get; set; }

    [Required]
    public decimal Price { get; set; }

    [Required]
    [MaxLength(100)]
    public string City { get; set; }

    [Required]
    [MaxLength(100)]
    public string State { get; set; }

    [Required]
    public int TotalFloor { get; set; }

    [Required]
    public int AreaSize { get; set; }

    [Required]
    [MaxLength(200)]
    public string Address { get; set; }

    // Feature Information
    public string Feature { get; set; } = string.Empty;

    // Images - Updated to store URLs as strings
    public string Image1Url { get; set; }
    public string Image2Url { get; set; }
    public string Image3Url { get; set; }
    public string Image4Url { get; set; }
    public string FloorPlanImageUrl { get; set; }

    // File Inputs for new image uploads
    public IFormFile Image1 { get; set; }
    public IFormFile Image2 { get; set; }
    public IFormFile Image3 { get; set; }
    public IFormFile Image4 { get; set; }
    public IFormFile FloorPlanImage { get; set; }

    // Is Featured property
    [Required]
    public bool IsFeatured { get; set; }
}
