using WMB.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace WMB.Api.Services
{
    public interface ICrudService
    {
        Task<ActionResult> CreateProductAsync(ProductDto productDto);
        Task<List<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task<Product> UpdateAsync(int id, ProductDto productDto);
        Task DeleteAsync(int id);
    }
}
