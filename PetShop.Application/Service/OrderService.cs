using PetShop.Application.Contract;
using PetShop.Application.DtosOrder;
using PetShop.Domain.Repositories;
using PetShop.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace PetShop.Application.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;

        public OrderService(IOrderRepository orderRepository, IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        public async Task<OrderResponseDto?> ProcessCheckoutAsync(CheckoutDto checkout)
        {
            decimal totalAmount = 0;
            var orderDetails = new List<OrderDetail>();

            foreach (var item in checkout.CartItems)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product == null || product.Stock < item.Quantity) return null; // Fallo de Stock

                totalAmount += product.Price * item.Quantity;
                orderDetails.Add(new OrderDetail { ProductId = product.Id, Quantity = item.Quantity, UnitPrice = product.Price });
            }

            // SIMULACIÓN DE PAGO
            if (checkout.CardNumber == "0000" || totalAmount <= 0) return null; // Pago Fallido

            var newOrder = new Order { UserId = checkout.UserId, TotalAmount = totalAmount, Status = "Paid", Details = orderDetails };
            await _orderRepository.AddAsync(newOrder);

            // ACTUALIZACIÓN DE STOCK (CRÍTICO)
            foreach (var item in checkout.CartItems)
            {
                await _productRepository.UpdateStockAsync(item.ProductId, item.Quantity);
            }

            return new OrderResponseDto
            {
                OrderId = newOrder.Id,
                TotalPaid = newOrder.TotalAmount,
                OrderDate = newOrder.OrderDate,
                Status = newOrder.Status
            };
        }
    }
}