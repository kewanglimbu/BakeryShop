using BakeryShop.Models;

namespace BakeryShop.Repositories.Interfaces
{
    public interface IBakeryItemRepository
    {
        Task<IEnumerable<BakeryItem>> GetAllBakeryItemsAsync();
        Task<BakeryItem?> GetBakeryItemByIdAsync(int? id);
        Task<int> AddBakeryItemAsync(BakeryItem bakeryItem);
        Task<int> UpdateBakeryItemAsync(BakeryItem bakeryItem);
        Task<int> DeleteBakeryItemAsync(int? id);
    }
}
