using APItoMVC.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace APItoMVC.Services
{
    public class ProductService
    {
        private readonly HttpClient _httpClient;
        private readonly string _productApiUrl = "https://localhost:7291/api/Products";
        private readonly string _categoryApiUrl = "https://localhost:7291/api/Category";

        public ProductService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            var response = await _httpClient.GetStringAsync(_productApiUrl);
            var products = JsonConvert.DeserializeObject<List<Product>>(response) ?? new List<Product>();

            var categoryResponse = await _httpClient.GetStringAsync(_categoryApiUrl);
            var categories = JsonConvert.DeserializeObject<List<Category>>(categoryResponse) ?? new List<Category>();

            foreach (var product in products)
            {
                var category = categories.FirstOrDefault(c => c.Id == product.CategoryId);
                product.Category = category ?? new Category { Name = "Không xác định" };
            }

            return products;
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            var response = await _httpClient.GetStringAsync(_categoryApiUrl);
            return JsonConvert.DeserializeObject<List<Category>>(response) ?? new List<Category>();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            var response = await _httpClient.GetStringAsync($"{_productApiUrl}/{id}");
            return JsonConvert.DeserializeObject<Product>(response);
        }

        public async Task<bool> CreateProductAsync(Product product)
        {
            var json = JsonConvert.SerializeObject(product);
            Console.WriteLine("Dữ liệu gửi lên API: " + json);  // Kiểm tra JSON gửi đi
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_productApiUrl, content);

            Console.WriteLine("Trạng thái phản hồi: " + response.StatusCode); // Kiểm tra response API

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateProductAsync(int id, Product product)
        {
            try
            {
                var json = JsonConvert.SerializeObject(product);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"{_productApiUrl}/{id}", content);
                var responseText = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"❌ Lỗi khi cập nhật sản phẩm: {response.StatusCode} - {responseText}");
                }

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Lỗi ngoại lệ khi cập nhật sản phẩm: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_productApiUrl}/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
