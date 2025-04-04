using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Data;
using RealEstateManagement.Models;

namespace RealEstateManagement.Controllers;

public class HomeController : Controller
{
    private readonly SignInManager<User> signInManager;
    private readonly UserManager<User> userManager;
    private readonly RoleManager<IdentityRole> roleManager;
    private readonly AppDbContext _context;

    public HomeController(SignInManager<User> signInManager, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, AppDbContext _context)
    {
        this.signInManager = signInManager;
        this.userManager = userManager;
        this.roleManager = roleManager;
        this._context = _context;
    }

    private readonly ILogger<HomeController> _logger;

    public async Task<IActionResult> Index(string type, string stype, string city)
    {
        var query = _context.Properties
                            .Include(p => p.User)
                            .AsQueryable(); 

        // Combine all input fields into one search query (ignoring null values)
        List<string> searchTerms = new List<string>();

        // Check user roles
        // Check if the user is authenticated
        var user = await signInManager.UserManager.GetUserAsync(User);

        if (user != null) 
        {
            // Get the roles of the currently authenticated user
            var roles = await userManager.GetRolesAsync(user);

            // If the user has the "Agent" role, redirect to the SellerDashboard
            if (roles.Contains("Agent"))
            {
                return RedirectToAction("SellerDashboard", "Property");
            }
        }

        if (!string.IsNullOrEmpty(type)) searchTerms.Add(type.ToLower());
        if (!string.IsNullOrEmpty(stype)) searchTerms.Add(stype.ToLower());
        if (!string.IsNullOrEmpty(city)) searchTerms.Add(city.ToLower());

        if (searchTerms.Any()) // Only filter if at least one search term is provided
        {
            query = query.Where(p =>
                searchTerms.Any(term =>
                    (p.PropertyType != null && p.PropertyType.ToLower().Contains(term)) ||
                    (p.SellingType != null && p.SellingType.ToLower().Contains(term)) ||
                    (p.City != null && p.City.ToLower().Contains(term))));
        }

        // Fetch the filtered list from the database
        var filteredProperties = await query.ToListAsync();

        // Provide a message if no results found
        if (!filteredProperties.Any())
        {
            ViewBag.Message = "No properties found matching your search criteria.";
        }

        return View(filteredProperties);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    public IActionResult About()
    {
        return View();
    }

    public async Task<IActionResult> Agent()
    {
        // First get all users
        var users = await userManager.Users.ToListAsync();

        // Then filter by role on the client side
        var agentUsers = new List<User>();

        foreach (var user in users)
        {
            if (await userManager.IsInRoleAsync(user, "Agent"))
            {
                agentUsers.Add(user);
            }
        }

        return View(agentUsers);
    }

}
