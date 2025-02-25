using APItoMVC.Models;
using APItoMVC.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace APItoMVC.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductService _productService;

        public ProductsController()
        {
            _productService = new ProductService();
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetProductsAsync();
            return View(products);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _productService.GetCategoriesAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _productService.GetCategoriesAsync();
                return View(product);
            }

            bool success = await _productService.CreateProductAsync(product);
            if (!success)
            {
                ModelState.AddModelError("", "Không thể thêm sản phẩm. Kiểm tra lại API!");
                ViewBag.Categories = await _productService.GetCategoriesAsync();
                return View(product);
            }

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound();

            ViewBag.Categories = await _productService.GetCategoriesAsync();
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _productService.GetCategoriesAsync();
                return View(product);
            }

            bool success = await _productService.UpdateProductAsync(id, product);
            if (!success)
            {
                ModelState.AddModelError("", "Không thể cập nhật sản phẩm. Kiểm tra lại API!");
                ViewBag.Categories = await _productService.GetCategoriesAsync();
                return View(product);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var success = await _productService.DeleteProductAsync(id);

            if (success)
            {
                TempData["SuccessMessage"] = "Xóa sản phẩm thành công!";
            }
            else
            {
                TempData["ErrorMessage"] = "Không thể xóa sản phẩm!";
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
