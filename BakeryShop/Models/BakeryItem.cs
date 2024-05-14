using System.ComponentModel.DataAnnotations;

namespace BakeryShop.Models
{
    public class BakeryItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [StringLength(100)]
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsAvailable { get; set; }
        public int CategoryId {  get; set; }
        public Category? Category { get; set; }
    }
}
