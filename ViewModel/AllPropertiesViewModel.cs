using RealEstateManagement.Models;

namespace RealEstateManagement.ViewModel
{
    public class AllPropertiesViewModel
    {
        public List<Property> Properties { get; set; }
        public Property FeaturedProperty { get; set; }
        public string CurrentType { get; set; }
        public string CurrentStype { get; set; }
        public string CurrentCity { get; set; }
        public Pager Pager { get; set; }
    }

    // Pager class for pagination
    public class Pager
    {
        public Pager(int totalItems, int currentPage = 1, int pageSize = 6)
        {
            TotalItems = totalItems;
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        }

        public int TotalItems { get; }
        public int CurrentPage { get; }
        public int PageSize { get; }
        public int TotalPages { get; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
    }
}
