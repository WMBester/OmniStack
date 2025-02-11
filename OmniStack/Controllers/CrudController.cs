using Microsoft.AspNetCore.Mvc;
using WMB.Api.Models;
using WMB.Api.Services;
using System.Threading.Tasks;

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
    }
}
