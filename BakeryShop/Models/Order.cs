using System.ComponentModel.DataAnnotations;

namespace BakeryShop.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public decimal OrderTotal { get; set; }
        public DateTime RequiredDeliveryDate { get; set; }
        public string? Comment { get; set; }
        public List<BakeryItem> BakeryItems { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
