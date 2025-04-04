namespace RealEstateManagement.ViewModel
{
    public class InquiryListItemVm
    {
        public int Id { get; set; }
        public string PropertyTitle { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public DateTime SentAt { get; set; }
        public string MessagePreview { get; set; }
        public bool IsRead { get; set; }
        public int ReplyCount { get; set; }
        public DateTime LastMessageDate { get; set; }
        public string ReceiverId { get; set; }
        public string CurrentUserId { get; set; } // Add this property

        public bool IsReceived => ReceiverId == CurrentUserId;
    }
}