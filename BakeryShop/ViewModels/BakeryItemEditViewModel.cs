using BakeryShop.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BakeryShop.ViewModels
{
    public class BakeryItemEditViewModel
    {
        public IEnumerable<SelectListItem>? Categories { get; set; } = default!;
        public BakeryItem BakeryItem { get; set; }
    }
}
