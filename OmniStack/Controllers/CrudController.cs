using Microsoft.AspNetCore.Mvc;
using WMB.Api.Models;
using WMB.Api.Services;

namespace WMB.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CrudController : ControllerBase
    {
        private readonly ICrudService _crudService;

        public CrudController(ICrudService crudService)
        {
            _crudService = crudService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync(ProductDto productDto)
        {
            return await _crudService.CreateProductAsync(productDto);
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetAllAsync()
        {
            return await _crudService.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetByIdAsync(int id)
        {
            return await _crudService.GetByIdAsync(id);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Product>> UpdateAsync(int id, ProductDto productDto)
        {
            return await _crudService.UpdateAsync(id, productDto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            return await _crudService.DeleteAsync(id);
        }
    }
}
