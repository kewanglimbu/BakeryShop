using BakeryShop.Models;
using BakeryShop.Repositories.Interfaces;
using BakeryShop.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BakeryShop.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            CategoryListViewModel categoryListViewModel = new()
            {
                Categories = (await _categoryRepository.GetAllCategoriesAsync()).ToList()
            };
            return View(categoryListViewModel);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(CategoryAddViewModel categoryVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Category category = new Category
                    {
                        Name = categoryVM.Name,
                        Description = categoryVM.Description,
                        DateAdded = DateTime.Now
                    };
                    await _categoryRepository.AddCategoryAsync(category);
                    return Ok(new { Success = true, Message = "Category added successfully!", redirectUrl = Url.Action("Index", "Category") });
                }
            }
            catch (Exception ex)
            {
                //ModelState.AddModelError("", $"Adding the category failed, please try again! Error: {ex.Message}");
                return BadRequest(new { Success = false });
            }
            return View(categoryVM);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
                return NotFound();

            var selectedCategory = await _categoryRepository.GetCategoryByIdAsync(id.Value);

            if (selectedCategory == null)
                return NotFound();

            CategoryEditViewModel catgoryEditVM = new CategoryEditViewModel
            {
                Name = selectedCategory.Name,
                Description = selectedCategory.Description
            };

            return View(catgoryEditVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryEditViewModel categoryEditVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var category = await _categoryRepository.GetCategoryByIdAsync(categoryEditVM.Id);
                    if (category == null)
                        return NotFound();

                    category.Name = categoryEditVM.Name;
                    category.Description = categoryEditVM.Description;

                    await _categoryRepository.UpdateCategoryAsync(category);
                    return Ok(new { Success = true, Message = "Category Updated successfully!", redirectUrl = Url.Action("Index", "Category") });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                //ModelState.AddModelError("", $"Updating the category failed, please try again! Error: {ex.Message}");
                string errorMessage = $"Updating the category failed, please try again! Error: {ex.Message}";
                return StatusCode(500, new { Success = false, Message = "A category with the same name already exists" });
            }
            //return View(categoryEditVM);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var category = await _categoryRepository.GetCategoryByIdAsync(id.Value);

            if (category == null)
                return NotFound();

            CategoryDetailsViewModel categoryDetailVM = new CategoryDetailsViewModel
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                DateAdded = category.DateAdded
            };

            return View(categoryDetailVM);
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? categoryId)
        {
            if (categoryId == null)
            {
                return BadRequest("Invalid category ID");
            }

            try
            {
                await _categoryRepository.DeleteCategoryAsync(categoryId.Value);
                return Ok(new { Success = true, Message = "Category deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = $"An error occurred: {ex.Message}" });
            }
        }
    }
}
