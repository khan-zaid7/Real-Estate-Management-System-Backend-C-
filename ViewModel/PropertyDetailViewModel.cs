using RealEstateManagement.Models;

namespace RealEstateManagement.ViewModel
{
    public class PropertyDetailViewModel
    {
        public Property CurrentProperty { get; set; }
        public Property FeaturedProperty { get; set; }
        public Property RecentProperty { get; set; }
    }
}
