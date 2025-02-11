using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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
            catch (Exception ex)
            {
                return new ObjectResult("An error occurred while creating the product.") { StatusCode = 500 };
            }
        }

        public async Task<List<Product>> GetAllAsync()
        {
            try
            {
                return await _context.Products.ToListAsync();
            }
            catch (DbException ex)
            {
                throw new Exception("A database error occurred", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred", ex);
            }
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Id must be greater than zero", nameof(id));
            }

            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    throw new KeyNotFoundException($"Product with id {id} not found");
                }
                return product;
            }
            catch (DbException ex)
            {
                throw new Exception("A database error occurred", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred", ex);
            }
        }

        public async Task<Product> UpdateAsync(int id, ProductDto ProductDto)
        {
            if (id < 0)
            {
                throw new ArgumentException("Id must be greater than zero", nameof(id));
            }

            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    throw new KeyNotFoundException($"Product with id {id} not found");
                }

                product.Name = ProductDto.Name;
                product.Price = ProductDto.Price;
                product.Description = ProductDto.Description;

                await _context.SaveChangesAsync();
                return product;
            }
            catch (DbException ex)
            {
                throw new Exception("A database error occurred", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            if (id < 0)
            {
                throw new ArgumentException("Id must be greater than zero", nameof(id));
            }

            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    throw new KeyNotFoundException($"Product with id {id} not found");
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            catch (DbException ex)
            {
                throw new Exception("A database error occurred", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred", ex);
            }
        }
    }
}
