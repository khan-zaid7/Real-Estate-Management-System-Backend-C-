using RealEstateManagement.Models;

namespace RealEstateManagement.Extensions
{
    public static class InquiryExtensions
    {
        public static IEnumerable<Inquiry> GetThread(this Inquiry inquiry)
        {
            var thread = new List<Inquiry>();

            // Add parent if exists
            if (inquiry.ParentInquiry != null)
            {
                thread.AddRange(inquiry.ParentInquiry.GetThread());
            }

            // Add current message
            thread.Add(inquiry);

            // Add replies
            if (inquiry.Replies != null)
            {
                foreach (var reply in inquiry.Replies.OrderBy(r => r.SentAt))
                {
                    thread.Add(reply);
                }
            }

            return thread;
        }

        public static string Truncate(this string value, int length)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= length ? value : value.Substring(0, length) + "...";
        }
    }
}
