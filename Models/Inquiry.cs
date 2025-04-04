using System.ComponentModel.DataAnnotations;

namespace RealEstateManagement.Models
{
    public class Inquiry
    {
        public int Id { get; set; }

        [Required]
        public string SenderId { get; set; }
        public User Sender { get; set; }

        [Required]
        public string ReceiverId { get; set; }
        public User Receiver { get; set; }

        [Required]
        public int PropertyId { get; set; }
        public Property Property { get; set; }

        [Required]
        public string Message { get; set; }

        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false;

        // For replies
        public int? ParentInquiryId { get; set; }
        public Inquiry ParentInquiry { get; set; }
        public ICollection<Inquiry> Replies { get; set; }
    }
}
