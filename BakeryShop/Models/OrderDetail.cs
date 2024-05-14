namespace BakeryShop.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int BakeryItemId { get; set; }
        public int Quantity { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
        public BakeryItem BakeryItem { get; set; } = default!;
        public Order Order { get; set; } = default!;
    }
}
