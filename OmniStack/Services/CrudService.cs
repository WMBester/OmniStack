using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using WMB.Api.DbContext;
using WMB.Api.Models;

namespace WMB.Api.Services
{
    public class CrudService : ICrudService
    {
        private readonly ApplicationDbContext _context;

        public CrudService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ActionResult> CreateProductAsync(ProductDto productDto)
        {
            if (productDto == null)
            {
                return new BadRequestObjectResult("Product data is required.");
            }

            try
            {
                var product = new Product
                {
                    Name = productDto.Name,
                    Price = productDto.Price,
                    Description = productDto.Description
                };

                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();

                return new ObjectResult(new { message = "Product successfully created", product }) { StatusCode = 201 };
            }
            catch (ArgumentException ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                return new ObjectResult(value: $"{ex.Message} {ex.InnerException?.Message}") { StatusCode = 500 };
            }
            catch (Exception ex)
            {
                return new ObjectResult("An error occurred while creating the product." + ex.InnerException?.Message) { StatusCode = 500 };
            }
        }

        public async Task<ActionResult<List<Product>>> GetAllAsync()
        {
            try
            {
                var products = await _context.Products.ToListAsync();
                return new OkObjectResult(products);
            }
            catch (DbException ex)
            {
                return new ObjectResult(value: $"{ex.Message} {ex.InnerException?.Message}") { StatusCode = 500 };
            }
            catch (Exception ex)
            {
                return new ObjectResult("An error occurred while retrieving the products." + ex.InnerException?.Message) { StatusCode = 500 };
            }
        }

        public async Task<ActionResult<Product>> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                return new BadRequestObjectResult("Id must be greater than zero");
            }

            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    return new NotFoundObjectResult($"Product with id {id} not found");
                }
                return new OkObjectResult(product);
            }
            catch (DbException ex)
            {
                return new ObjectResult(value: $"{ex.Message} {ex.InnerException?.Message}") { StatusCode = 500 };
            }
            catch (Exception ex)
            {
                return new ObjectResult("An error occurred while retrieving the product." + ex.InnerException?.Message) { StatusCode = 500 };
            }
        }

        public async Task<ActionResult<Product>> UpdateAsync(int id, ProductDto productDto)
        {
            if (id <= 0)
            {
                return new BadRequestObjectResult("Id must be greater than zero");
            }

            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    return new NotFoundObjectResult($"Product with id {id} not found");
                }

                product.Name = productDto.Name;
                product.Price = productDto.Price;
                product.Description = productDto.Description;

                await _context.SaveChangesAsync();
                return new ObjectResult(new { message = "Update successful", product }) { StatusCode = 200 };
            }
            catch (DbException ex)
            {
                return new ObjectResult(value: $"{ex.Message} {ex.InnerException?.Message}") { StatusCode = 500 };
            }
            catch (Exception ex)
            {
                return new ObjectResult("An error occurred while updating the product." + ex.InnerException?.Message) { StatusCode = 500 };
            }
        }

        public async Task<ActionResult> DeleteAsync(int id)
        {
            if (id <= 0)
            {
                return new BadRequestObjectResult("Id must be greater than zero");
            }

            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    return new NotFoundObjectResult($"Product with id {id} not found");
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return new OkResult();
            }
            catch (DbException ex)
            {
                return new ObjectResult(value: $"{ex.Message} {ex.InnerException?.Message}") { StatusCode = 500 };
            }
            catch (Exception ex)
            {
                return new ObjectResult("An error occurred while deleting the product." + ex.InnerException?.Message) { StatusCode = 500 };
            }
        }
    }
}
