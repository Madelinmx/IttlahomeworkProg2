using PetShop.Domain.Entities;
using PetShop.Domain.Repositories;
namespace PetShop.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private static readonly List<Product> _products = new List<Product>
        {
            new Product { Id = 1, Name = "Pelota de Perro", Price = 8.50m, Stock = 10 },
            new Product { Id = 2, Name = "Comida para Gato", Price = 22.99m, Stock = 5 },
            new Product { Id = 3, Name = "Jaula de Hamster", Price = 50.00m, Stock = 0 }
        };

        public Task<Product?> GetByIdAsync(int id) =>
            Task.FromResult(_products.FirstOrDefault(p => p.Id == id));
        public Task<List<Product>> GetAllAsync() =>
            Task.FromResult(_products);
        public Task AddAsync(Product product)
        {
            product.Id = _products.Any() ? _products.Max(p => p.Id) + 1 : 1;
            _products.Add(product);
            return Task.CompletedTask;
        }
        public Task UpdateStockAsync(int productId, int quantity)
        {
            var product = _products.FirstOrDefault(p => p.Id == productId);
            if (product != null) product.Stock -= quantity;
            return Task.CompletedTask;
        }
    }
}