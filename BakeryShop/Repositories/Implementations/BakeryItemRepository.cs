using BakeryShop.Data;
using BakeryShop.Models;
using BakeryShop.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BakeryShop.Repositories.Implementations
{
    public class BakeryItemRepository : IBakeryItemRepository
    {
        private readonly BakeryShopDbContext _bakeryShopDbContext;

        public BakeryItemRepository(BakeryShopDbContext bakeryShopDbContext)
        {
            _bakeryShopDbContext = bakeryShopDbContext;
        }

        public async Task<IEnumerable<BakeryItem>> GetAllBakeryItemsAsync()
        {
            return await _bakeryShopDbContext.BakeryItems.ToListAsync();
        }

        public async Task<BakeryItem?> GetBakeryItemByIdAsync(int? id)
        {
            return await _bakeryShopDbContext.BakeryItems.AsNoTracking().Include(c => c.Category).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<int> AddBakeryItemAsync(BakeryItem bakeryItem)
        {
            bool bakeryItemWithSameNameExist = await _bakeryShopDbContext.BakeryItems.AnyAsync(c => c.Name == bakeryItem.Name);

            if (bakeryItemWithSameNameExist)
                throw new Exception("A bakery Item with the same name already exists");

            _bakeryShopDbContext.BakeryItems.Add(bakeryItem);

            return await _bakeryShopDbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateBakeryItemAsync(BakeryItem bakeryItem)
        {
            bool bakeryItemWithSameNameExist = await _bakeryShopDbContext.BakeryItems.AnyAsync(c => c.Name == bakeryItem.Name && c.Id != bakeryItem.Id);

            if (bakeryItemWithSameNameExist)
                throw new Exception("A bakery Item with the same name already exists");

            var bakeryItemToUpdate = await _bakeryShopDbContext.BakeryItems.FirstOrDefaultAsync(c => c.Id == bakeryItem.Id);

            if (bakeryItemToUpdate != null)
            {
                bakeryItemToUpdate.Name = bakeryItem.Name;
                bakeryItemToUpdate.Description = bakeryItem.Description;
                bakeryItemToUpdate.Price = bakeryItem.Price;
                bakeryItemToUpdate.ImageUrl = bakeryItem.ImageUrl;
                bakeryItemToUpdate.IsAvailable = bakeryItem.IsAvailable;
                bakeryItemToUpdate.Description = bakeryItem.Description;
                bakeryItemToUpdate.CategoryId = bakeryItem.CategoryId;

                _bakeryShopDbContext.BakeryItems.Update(bakeryItemToUpdate);
                return await _bakeryShopDbContext.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException($"Bakery Item not found to update.");
            }
        }

        public async Task<int> DeleteBakeryItemAsync(int? id)
        {
            var bakeryItemToDelete = await _bakeryShopDbContext.BakeryItems.FirstOrDefaultAsync(c => c.Id == id);

            if (bakeryItemToDelete != null)
            {
                _bakeryShopDbContext.BakeryItems.Remove(bakeryItemToDelete);
                return await _bakeryShopDbContext.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException($"Bakery Item to delete can't be found.");
            }
        }
    }
}
