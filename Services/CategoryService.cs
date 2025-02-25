using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using APItoMVC.Models;

public class CategoryService
{
    private readonly HttpClient _httpClient;
    private readonly string apiUrl = "https://localhost:7291/api/Category";

    public CategoryService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Category>> GetCategoriesAsync()
    {
        var response = await _httpClient.GetStringAsync(apiUrl);
        return JsonConvert.DeserializeObject<List<Category>>(response);
    }

    public async Task<Category> GetCategoryByIdAsync(int id)
    {
        var response = await _httpClient.GetStringAsync($"{apiUrl}/{id}");
        return JsonConvert.DeserializeObject<Category>(response);
    }

    public async Task CreateCategoryAsync(Category category)
    {
        var content = new StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8, "application/json");
        await _httpClient.PostAsync(apiUrl, content);
    }

    public async Task UpdateCategoryAsync(int id, Category category)
    {
        var content = new StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8, "application/json");
        await _httpClient.PutAsync($"{apiUrl}/{id}", content);
    }

    public async Task DeleteCategoryAsync(int id)
    {
        await _httpClient.DeleteAsync($"{apiUrl}/{id}");
    }
}
