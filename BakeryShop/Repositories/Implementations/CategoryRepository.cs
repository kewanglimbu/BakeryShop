using BakeryShop.Data;
using BakeryShop.Models;
using BakeryShop.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BakeryShop.Repository.Implementations
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly BakeryShopDbContext _bakeryShopDbContext;

        public CategoryRepository(BakeryShopDbContext bakeryShopDbContext)
        {
            _bakeryShopDbContext = bakeryShopDbContext;   
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _bakeryShopDbContext.Categories.AsNoTracking().OrderBy(c => c.Id).ToListAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(int? id)
        {
            return await _bakeryShopDbContext.Categories.AsNoTracking().Include(b => b.BakeryItems).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<int> AddCategoryAsync(Category category)
        {
            bool categoryWithSameNameExist = await _bakeryShopDbContext.Categories.AnyAsync(c => c.Name == category.Name);

            if (categoryWithSameNameExist)
                throw new Exception("A category with the same name already exists");
            
            _bakeryShopDbContext.Categories.Add(category);

            return await _bakeryShopDbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateCategoryAsync(Category category)
        {
            bool categoryWithSameNameExist = await _bakeryShopDbContext.Categories.AnyAsync(c => c.Name == category.Name && c.Id != category.Id);

            if (categoryWithSameNameExist)
                throw new Exception("A category with the same name already exists");

            var categoryToUpdate = await _bakeryShopDbContext.Categories.FirstOrDefaultAsync(c => c.Id == category.Id);

            if (categoryToUpdate != null)
            {
                categoryToUpdate.Name = category.Name;
                categoryToUpdate.Description = category.Description;

                _bakeryShopDbContext.Categories.Update(categoryToUpdate);
                return await _bakeryShopDbContext.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException($"Category not found to update.");
            }
        }

        public async Task<int> DeleteCategoryAsync(int? id)
        {
            var bakeryItemsInCategory = _bakeryShopDbContext.BakeryItems.Any(p => p.CategoryId == id);

            if (bakeryItemsInCategory)
                throw new Exception("Unable to delete category. Please delete all associated bakery items first");

            var categoryToDelete = await _bakeryShopDbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (categoryToDelete != null)
            {
                _bakeryShopDbContext.Categories.Remove(categoryToDelete);
                return await _bakeryShopDbContext.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException($"Category not found to delete.");
            }
        }
    }
}
