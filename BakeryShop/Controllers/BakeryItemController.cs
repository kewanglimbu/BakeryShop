using BakeryShop.Models;
using BakeryShop.Repositories.Interfaces;
using BakeryShop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BakeryShop.Controllers
{
    public class BakeryItemController : Controller
    {
        private readonly IBakeryItemRepository _bakeryItemRepository;
        private readonly ICategoryRepository _categoryRepository;


        public BakeryItemController(IBakeryItemRepository bakeryItemRepository, ICategoryRepository categoryRepository)
        {
            _bakeryItemRepository = bakeryItemRepository;
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            BakeryItemListViewModel bakeryItemListViewModel = new()
            {
                BakeryItems = (await _bakeryItemRepository.GetAllBakeryItemsAsync()).ToList()
            };
            return View(bakeryItemListViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            try
            {
                IEnumerable<Category>? allCategories = await _categoryRepository.GetAllCategoriesAsync();
                IEnumerable<SelectListItem> selectListItems = allCategories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList();

                BakeryItemAddViewModel bakeryItemAddViewModel = new BakeryItemAddViewModel
                {
                    Categories = selectListItems
                };
                return View(bakeryItemAddViewModel);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = $"There was an error: {ex.Message}";
            }
            return View(new BakeryItemAddViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(BakeryItemAddViewModel bakeryItemAddVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    BakeryItem bakeryItem = new BakeryItem()
                    {
                        Name = bakeryItemAddVM.BakeryItem.Name,
                        Description = bakeryItemAddVM.BakeryItem.Description,
                        Price = bakeryItemAddVM.BakeryItem.Price,
                        ImageUrl = bakeryItemAddVM.BakeryItem.ImageUrl,
                        IsAvailable = bakeryItemAddVM.BakeryItem.IsAvailable,
                        CategoryId = bakeryItemAddVM.BakeryItem.CategoryId
                    };

                    await _bakeryItemRepository.AddBakeryItemAsync(bakeryItem);
                    return Ok(new { Success = true, Message = "Bakery Product added successfully!", redirectUrl = Url.Action("Index", "BakeryItem") });
                }
            }
            catch (Exception)
            {
                return BadRequest(new { Success = false });
            }
            return View(bakeryItemAddVM);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var allCategories = await _categoryRepository.GetAllCategoriesAsync();

            IEnumerable<SelectListItem> selectListItems = allCategories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();

            var selectedBakeryItem = await _bakeryItemRepository.GetBakeryItemByIdAsync(id.Value);

            BakeryItemEditViewModel bakeryItemEditViewModel = new() { Categories = selectListItems, BakeryItem = selectedBakeryItem };
            return View(bakeryItemEditViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BakeryItemEditViewModel bakeryItemEditVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var bakeryItem = await _bakeryItemRepository.GetBakeryItemByIdAsync(bakeryItemEditVM.BakeryItem.Id);
                    if (bakeryItem == null)
                        return NotFound();

                    bakeryItem.Name = bakeryItemEditVM.BakeryItem.Name;
                    bakeryItem.Description = bakeryItemEditVM.BakeryItem.Description;
                    bakeryItem.ImageUrl = bakeryItemEditVM.BakeryItem.ImageUrl;
                    bakeryItem.Price = bakeryItemEditVM.BakeryItem.Price;
                    bakeryItem.IsAvailable = bakeryItemEditVM.BakeryItem.IsAvailable;
                    bakeryItem.CategoryId = bakeryItemEditVM.BakeryItem.CategoryId;

                    await _bakeryItemRepository.UpdateBakeryItemAsync(bakeryItem);
                    return Ok(new { Success = true, Message = "Bakery Product Updated successfully!", redirectUrl = Url.Action("Index", "BakeryItem") });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                string errorMessage = $"Updating the Bakery Product failed, please try again! Error: {ex.Message}";
                return StatusCode(500, new { Success = false, Message = "A Bakery Product with the same name already exists" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var bakeryItem = await _bakeryItemRepository.GetBakeryItemByIdAsync(id);
                if (bakeryItem == null)
                {
                    return NotFound();
                }

                var bakeryItemDetailsViewModel = new BakeryItemDetailsViewModel
                {
                    Id = bakeryItem.Id,
                    Name = bakeryItem.Name,
                    Description = bakeryItem.Description,
                    Price = bakeryItem.Price,
                    ImageUrl = bakeryItem.ImageUrl,
                    IsAvailable = bakeryItem.IsAvailable,
                    CategoryId = bakeryItem.CategoryId,
                    Category = bakeryItem.Category
                };

                return View(bakeryItemDetailsViewModel);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = $"There was an error: {ex.Message}";
                return View("Error");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int? bakeryItemId)
        {
            if (bakeryItemId == null)
            {
                return BadRequest("Invalid Bakery Product ID");
            }

            try
            {
                await _bakeryItemRepository.DeleteBakeryItemAsync(bakeryItemId.Value);
                return Ok(new { Success = true, Message = "Bakery Product deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = $"An error occurred: {ex.Message}" });
            }
        }
    }
}
