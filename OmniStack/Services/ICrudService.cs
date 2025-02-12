using WMB.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace WMB.Api.Services
{
    public interface ICrudService
    {
        Task<ActionResult> CreateProductAsync(ProductDto productDto);
        Task<ActionResult<List<Product>>> GetAllAsync();
        Task<ActionResult<Product>> GetByIdAsync(int id);
        Task<ActionResult<Product>> UpdateAsync(int id, ProductDto productDto);
        Task<ActionResult> DeleteAsync(int id);
    }
}
