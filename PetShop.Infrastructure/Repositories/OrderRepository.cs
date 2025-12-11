using PetShop.Domain.Entities;
using PetShop.Domain.Repositories;
namespace PetShop.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private static readonly List<Order> _orders = new List<Order>();

        public Task AddAsync(Order order)
        {
            order.Id = _orders.Any() ? _orders.Max(o => o.Id) + 1 : 1;
            foreach (var detail in order.Details) detail.OrderId = order.Id;
            _orders.Add(order);
            return Task.CompletedTask;
        }
        public Task<Order?> GetByIdAsync(int orderId) =>
            Task.FromResult(_orders.FirstOrDefault(o => o.Id == orderId));
        public Task UpdateStatusAsync(int orderId, string newStatus)
        {
            var orderToUpdate = _orders.FirstOrDefault(o => o.Id == orderId);
            if (orderToUpdate != null) orderToUpdate.Status = newStatus;
            return Task.CompletedTask;
        }
    }
}