using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Data;
using RealEstateManagement.Models;
using RealEstateManagement.ViewModel;

namespace RealEstateManagement.Controllers
{
    [Authorize]
    public class InquiryController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public InquiryController(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Inquiry
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            var inquiries = await _context.Inquiries
                .Where(i => (i.ReceiverId == user.Id || i.SenderId == user.Id) && i.ParentInquiryId == null)
                .Select(i => new InquiryListItemVm
                {
                    Id = i.Id,
                    PropertyTitle = i.Property.Title,
                    SenderName = i.Sender.UserName,
                    SenderEmail = i.Sender.Email,
                    SentAt = i.SentAt,
                    MessagePreview = i.Message.Length > 50 ? i.Message.Substring(0, 50) + "..." : i.Message,
                    IsRead = i.IsRead,
                    ReplyCount = i.Replies.Count,
                    LastMessageDate = i.Replies.Any() ? i.Replies.Max(r => r.SentAt) : i.SentAt,
                    ReceiverId = i.ReceiverId,
                    CurrentUserId = user.Id // Set the current user ID here
                })
                .OrderByDescending(i => i.LastMessageDate)
                .AsNoTracking()
                .ToListAsync();

            return View(inquiries);
        }

        // GET: Inquiry/Create/{propertyId}
        public IActionResult Create(int propertyId)
        {
            var inquiry = new Inquiry { PropertyId = propertyId };
            return View(inquiry);
        }

        // POST: Inquiry/Create
        [HttpPost]
        public async Task<IActionResult> Create(Inquiry inquiry)
        {
            var user = await _userManager.GetUserAsync(User);
            var property = await _context.Properties
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == inquiry.PropertyId);

            if (property == null)
            {
                return NotFound();
            }

            inquiry.SenderId = user.Id;
            inquiry.ReceiverId = property.UserId;
            inquiry.SentAt = DateTime.UtcNow;

            _context.Add(inquiry);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = inquiry.Id });
        }

        // GET: Inquiry/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var inquiry = await _context.Inquiries
                .Include(i => i.Property)
                .Include(i => i.Sender)
                .Include(i => i.Receiver)
                .Include(i => i.Replies)
                    .ThenInclude(r => r.Sender)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (inquiry == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            ViewBag.CurrentUserId = user?.Id;

            // Mark messages as read
            if (user != null)
            {
                // Mark main inquiry if current user is receiver
                if (inquiry.ReceiverId == user.Id && !inquiry.IsRead)
                {
                    inquiry.IsRead = true;
                }

                // Mark all unread replies where current user is receiver
                foreach (var reply in inquiry.Replies.Where(r =>
                    r.ReceiverId == user.Id && !r.IsRead))
                {
                    reply.IsRead = true;
                }

                // Only save if we made changes
                if (_context.ChangeTracker.HasChanges())
                {
                    await _context.SaveChangesAsync();
                }
            }

            return View(inquiry);
        }
        // POST: Inquiry/Reply
        [HttpPost]
        public async Task<IActionResult> Reply(int parentId, string message)
        {
            var parentInquiry = await _context.Inquiries
                .Include(i => i.Sender)
                .Include(i => i.Receiver)
                .FirstOrDefaultAsync(i => i.Id == parentId);

            if (parentInquiry == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);

            var reply = new Inquiry
            {
                ParentInquiryId = parentId,
                PropertyId = parentInquiry.PropertyId,
                SenderId = user.Id,
                ReceiverId = user.Id == parentInquiry.SenderId ? parentInquiry.ReceiverId : parentInquiry.SenderId,
                Message = message,
                SentAt = DateTime.UtcNow
            };

            _context.Add(reply);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = parentId });
        }
    }
}
