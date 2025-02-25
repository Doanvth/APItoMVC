using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using APItoMVC.Models;

namespace APItoMVC.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly CategoryService _categoryService;

        public CategoriesController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetCategoriesAsync();
            return View(categories);
        }

        public async Task<IActionResult> Details(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null) return NotFound();
            return View(category);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                await _categoryService.CreateCategoryAsync(category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null) return NotFound();
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (ModelState.IsValid)
            {
                await _categoryService.UpdateCategoryAsync(id, category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        [HttpGet]
        [Route("Categories/Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null) return NotFound();

            await _categoryService.DeleteCategoryAsync(id);
            TempData["SuccessMessage"] = $"Danh mục '{category.Name}' đã được xoá thành công.";
            return RedirectToAction(nameof(Index));
        }
    }

}