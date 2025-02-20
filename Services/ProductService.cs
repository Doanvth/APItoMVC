using APItoMVC.Models;
using Newtonsoft.Json;
using System.Text;

namespace APItoMVC.Services
{
    public class ProductService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = "https://localhost:7291/api/Products"; 
        public ProductService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            var response = await _httpClient.GetStringAsync(_apiUrl);
            return JsonConvert.DeserializeObject<List<Product>>(response);
            // Lớp cung cấp các phương thức để chuyển đổi giữa JSON và các đối tượng .NET.
            //Phương thức để chuyển đổi chuỗi JSON thành đối tượng kiểu T.
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            var response = await _httpClient.GetStringAsync($"{_apiUrl}/{id}");
            return JsonConvert.DeserializeObject<Product>(response);
        }

        public async Task<bool> CreateProductAsync(Product product)
        {
            var json = JsonConvert.SerializeObject(product);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_apiUrl, content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateProductAsync(int id, Product product)
        {
            var json = JsonConvert.SerializeObject(product);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{_apiUrl}/{id}", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_apiUrl}/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
