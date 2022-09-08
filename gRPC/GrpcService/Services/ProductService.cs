using Grpc.Core;

using GrpcService.Persistence;
using GrpcService.Persistence.Models;

using Microsoft.EntityFrameworkCore;

using static GrpcService.ProductsService;

namespace GrpcService.Services
{
    public class ProductService : ProductsServiceBase
    {
        private readonly ILogger<ProductService> _logger;
        private readonly BlazorDbContext db;

        public ProductService(ILogger<ProductService> logger, BlazorDbContext db)
        {
            _logger = logger;
            this.db = db;
        }

        public override async Task<ProductsMessage> GetAll(EmptyMessage request, ServerCallContext context)
        {
            ProductsMessage response = new ProductsMessage();

            List<ProductMessage> products = await this.db
                .Products
                .Select(p => new ProductMessage
                {
                    Id = p.Id,
                    ProductName = p.Name,
                    CategoryName = p.Category,
                    Manufacturer = p.Manufacturer,
                    Price = p.Price
                })
                .ToListAsync();

            response.Items.AddRange(products);

            return await Task.FromResult(response);
        }

        public override async Task<ProductMessage> GetById(ProductIdMessage request, ServerCallContext context)
        {
            Product? product = await this.db.Products.FindAsync(request.Id);

            ProductMessage searchedProduct = new ProductMessage()
            {
                Id = product.Id,
                ProductName = product.Name,
                CategoryName = product.Category,
                Manufacturer = product.Manufacturer,
                Price = product.Price
            };

            return await Task.FromResult(searchedProduct);
        }

        public override async Task<ProductMessage> Create(ProductMessage request, ServerCallContext context)
        {
            Product newProduct = new Product()
            {
                Id = request.Id,
                Name = request.ProductName,
                Category = request.CategoryName,
                Manufacturer = request.Manufacturer,
                Price = request.Price
            };

            Product res = (await this.db.Products.AddAsync(newProduct)).Entity;
            await this.db.SaveChangesAsync();

            ProductMessage response = new ProductMessage()
            {
                Id = res.Id,
                ProductName = res.Name,
                CategoryName = res.Category,
                Manufacturer = res.Manufacturer,
                Price = res.Price
            };

            return await Task.FromResult(response);
        }

        public override async Task<ProductMessage> Edit(ProductMessage request, ServerCallContext context)
        {
            Product? product = await this.db.Products.FindAsync(request.Id);

            if (product == null)
            {
                return await Task.FromResult<ProductMessage>(null);
            }

            product.Id = request.Id;
            product.Name = request.ProductName;
            product.Category = request.CategoryName;
            product.Manufacturer = request.Manufacturer;
            product.Price = request.Price;

            this.db.Products.Update(product);
            await this.db.SaveChangesAsync();

            return await Task.FromResult(new ProductMessage()
            {
                Id = product.Id,
                ProductName = product.Name,
                CategoryName = product.Category,
                Manufacturer = product.Manufacturer,
                Price = product.Price
            });
        }

        public override async Task<EmptyMessage> Delete(ProductIdMessage request, ServerCallContext context)
        {
            Product? product = await this.db.Products.FindAsync(request.Id);

            if (product == null)
            {
                return await Task.FromResult<EmptyMessage>(null);
            }

            this.db.Products.Remove(product);
            await this.db.SaveChangesAsync();

            return await Task.FromResult(new EmptyMessage());
        }
    }
}
