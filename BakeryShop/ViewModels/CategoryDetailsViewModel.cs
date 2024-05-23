using BakeryShop.Models;

namespace BakeryShop.ViewModels
{
    public class CategoryDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime? DateAdded { get; set; }
        public ICollection<BakeryItem> BakeryItems { get; set; } = new List<BakeryItem>();
    }
}
