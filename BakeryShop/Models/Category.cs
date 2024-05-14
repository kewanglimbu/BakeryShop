namespace BakeryShop.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }= string.Empty;
        public string? Description { get; set; }
        public DateTime? DateAdded {  get; set; }
        public ICollection<BakeryItem>? BakeryItems { get; set;}
    }
}
