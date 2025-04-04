using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using RealEstateManagement.Models;
using RealEstateManagement.ViewModel;
using RealEstateManagement.Data;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace RealEstateManagement.Controllers
{

    public class PropertyController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _context;
        private readonly ILogger<PropertyController> _logger;

        public PropertyController(UserManager<User> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        [Authorize(Roles = "Agent")]
        public async Task<IActionResult> SellerDashboard()
        {
            var user = await _userManager.GetUserAsync(User); 

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var properties = await _context.Properties
                .Where(p => p.UserId == user.Id) 
                .ToListAsync(); 
            return View(properties); 
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Agent")]
        [HttpGet]
        public IActionResult AddProperty()
        {
            return View();
        }


        [Authorize(Roles = "Agent")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProperty(PropertyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Collect all validation errors
                var errorMessages = new StringBuilder();
                foreach (var state in ModelState)
                {
                    if (state.Value.Errors.Count > 0)
                    {
                        foreach (var error in state.Value.Errors)
                        {
                            errorMessages.AppendLine($"Error in field '{state.Key}': {error.ErrorMessage}");
                        }
                    }
                }

                // Return validation error response
                return Content($"Validation failed! Here are the errors: {errorMessages.ToString()}");
            }

            try
            {
                // Ensure the user is authenticated
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Unauthorized(); // Return Unauthorized if no user is found
                }

                // Validate required files (ensure Image1 is uploaded)
                if (model.Image1 == null || model.Image1.Length == 0)
                {
                    ModelState.AddModelError("Image1", "Main image is required.");
                    return View(model); // Return the view with the error if Image1 is missing
                }

                // Save image files
                string image1Path = await SaveFile(model.Image1);
                string image2Path = model.Image2 != null && model.Image2.Length > 0 ? await SaveFile(model.Image2) : null;
                string image3Path = model.Image3 != null && model.Image3.Length > 0 ? await SaveFile(model.Image3) : null;
                string image4Path = model.Image4 != null && model.Image4.Length > 0 ? await SaveFile(model.Image4) : null;
                string floorPlanPath = model.FloorPlanImage != null && model.FloorPlanImage.Length > 0 ? await SaveFile(model.FloorPlanImage) : null;

                // Create the property object
                var property = new Property
                {
                    Title = model.Title,
                    Content = model.Content,
                    PropertyType = model.PropertyType,
                    SellingType = model.SellingType,
                    Status = model.Status,
                    Bathroom = model.Bathroom,
                    Kitchen = model.Kitchen,
                    BHK = model.BHK,
                    Bedroom = model.Bedroom,
                    Balcony = model.Balcony,
                    Hall = model.Hall,
                    Floor = model.Floor,
                    Price = model.Price,
                    City = model.City,
                    State = model.State,
                    TotalFloor = model.TotalFloor,
                    AreaSize = model.AreaSize,
                    Address = model.Address,
                    Feature = model.Feature,
                    Image1 = image1Path,
                    Image2 = image2Path,
                    Image3 = image3Path,
                    Image4 = image4Path,
                    FloorPlanImage = floorPlanPath,
                    IsFeatured = model.IsFeatured,
                    UserId = user.Id
                };

                // Save the property to the database
                _context.Properties.Add(property);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Property added successfully!";
                return RedirectToAction("Index", "Home"); // Redirect to home page
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.InnerException?.Message ?? "An error occurred while adding the property.");
                return View(model); // Return the view with the error message
            }
        }

        [Authorize(Roles = "Agent")]
        [HttpGet]
        public async Task<IActionResult> EditProperty(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var property = await _context.Properties.FirstOrDefaultAsync(p => p.Id == id);

            if (property == null)
            {
                return NotFound();
            }

            // Map existing property to the view model
            var model = new EditPropertyViewModel
            {
                Id = property.Id,
                Title = property.Title,
                Content = property.Content,
                PropertyType = property.PropertyType,
                SellingType = property.SellingType,
                Status = property.Status,
                Bathroom = (int)property.Bathroom,
                Kitchen = (int)property.Kitchen,
                BHK = property.BHK,
                Bedroom = (int)property.Bedroom,
                Balcony = (int)property.Balcony,
                Hall = (int)property.Hall,
                Floor = property.Floor,
                Price = property.Price,
                City = property.City,
                State = property.State,
                TotalFloor = property.TotalFloor,
                AreaSize = property.AreaSize,
                Address = property.Address,
                Feature = property.Feature,
                IsFeatured = property.IsFeatured,

                // Pass existing image URLs
                Image1Url = property.Image1,
                Image2Url = property.Image2,
                Image3Url = property.Image3,
                Image4Url = property.Image4,
                FloorPlanImageUrl = property.FloorPlanImage
            };

            return View(model);
        }

        [Authorize(Roles = "Agent")]
        [HttpPost]
        public async Task<IActionResult> EditProperty(int id, EditPropertyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // Fetch the existing property by its ID
                var property = await _context.Properties.FindAsync(id);

                if (property == null)
                {
                    return NotFound(); // Return an error if the property is not found
                }

                // Update the property details
                property.Title = model.Title;
                property.Content = model.Content;
                property.PropertyType = model.PropertyType;
                property.SellingType = model.SellingType;
                property.Status = model.Status;
                property.Bathroom = model.Bathroom;
                property.Kitchen = model.Kitchen;
                property.BHK = model.BHK;
                property.Bedroom = model.Bedroom;
                property.Balcony = model.Balcony;
                property.Hall = model.Hall;
                property.Floor = model.Floor;
                property.Price = model.Price;
                property.City = model.City;
                property.State = model.State;
                property.TotalFloor = model.TotalFloor;
                property.AreaSize = model.AreaSize;
                property.Address = model.Address;
                property.IsFeatured = model.IsFeatured;

                // Handle file uploads, if any
                if (model.Image1 != null && model.Image1.Length > 0)
                {
                    property.Image1 = await SaveFile(model.Image1);
                }

                if (model.Image2 != null && model.Image2.Length > 0)
                {
                    property.Image2 = await SaveFile(model.Image2);
                }

                if (model.Image3 != null && model.Image3.Length > 0)
                {
                    property.Image3 = await SaveFile(model.Image3);
                }

                if (model.Image4 != null && model.Image4.Length > 0)
                {
                    property.Image4 = await SaveFile(model.Image4);
                }

                if (model.FloorPlanImage != null && model.FloorPlanImage.Length > 0)
                {
                    property.FloorPlanImage = await SaveFile(model.FloorPlanImage);
                }

                // Update the property in the database
                _context.Properties.Update(property);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Property updated successfully!";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Error updating property");
                ModelState.AddModelError("", ex.InnerException?.Message ?? "An error occurred while updating the property.");
                return View(model);
            }
        }


        [Authorize(Roles = "Agent")]
        private async Task<string> SaveFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return null; // Return null if no file is provided
            }

            try
            {
                // Generate a unique filename
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);

                // Ensure the directory exists
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                // Save the file to the specified location
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream); // Copy the file to the server
                }

                // Return the file path to be saved in the database
                return $"/uploads/{fileName}";
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during file save
                throw new Exception("Error saving file: " + ex.Message);
            }
        }

        public async Task<IActionResult> PropertyDetail(int id)
        {
            // Get current property with user details
            var property = await _context.Properties
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (property == null)
            {
                return NotFound();
            }

            // Get a featured property (excluding current property)
            var featuredProperty = await _context.Properties
                .Where(p => p.IsFeatured && p.Id != id)
                .OrderByDescending(p => p.Id)
                .FirstOrDefaultAsync();

            // Get most recent property (excluding current and featured properties)
            var recentPropertiesQuery = _context.Properties
                .Where(p => p.Id != id && (featuredProperty == null || p.Id != featuredProperty.Id))
                .OrderByDescending(p => p.Id);

            var recentProperty = await recentPropertiesQuery.FirstOrDefaultAsync();

            var viewModel = new PropertyDetailViewModel
            {
                CurrentProperty = property,
                FeaturedProperty = featuredProperty,
                RecentProperty = recentProperty
            };

            return View(viewModel);
        }

        public async Task<IActionResult> AllProperties(
             string type,
             string stype,
             string city,
             int page = 1,
             int pageSize = 2)
                {
                    // Start with base query
                    var query = _context.Properties
                        .Include(p => p.User)
                        .Where(p => p.Status == "Available")
                        .AsQueryable();

                    // Apply filters
                    if (!string.IsNullOrEmpty(type))
                    {
                        query = query.Where(p => p.PropertyType.ToLower() == type.ToLower());
                    }

                    if (!string.IsNullOrEmpty(stype))
                    {
                        query = query.Where(p => p.SellingType.ToLower() == stype.ToLower());
                    }

                    if (!string.IsNullOrEmpty(city))
                    {
                        query = query.Where(p => p.City.ToLower().Contains(city.ToLower()));
                    }

                    // Get total count for pagination
                    var totalItems = await query.CountAsync();

                    // Apply pagination
                    var properties = await query
                        .OrderByDescending(p => p.Id)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToListAsync();

                    // Get featured property (excluding current filtered results)
                    var featuredProperty = await _context.Properties
                        .FirstOrDefaultAsync(p => p.IsFeatured && p.Status == "Available");

                    var viewModel = new AllPropertiesViewModel
                    {
                        Properties = properties,
                        FeaturedProperty = featuredProperty,
                        CurrentType = type,
                        CurrentStype = stype,
                        CurrentCity = city,
                        Pager = new Pager(totalItems, page, pageSize)
                    };

                    return View(viewModel);
        }
    }
}