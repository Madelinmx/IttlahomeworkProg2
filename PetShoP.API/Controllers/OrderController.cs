using Microsoft.AspNetCore.Mvc;
using PetShop.Application.Contract;
using PetShop.Application.DtosOrder;
namespace PetShop.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService) => _orderService = orderService;
        [HttpPost("checkout")]
        public async Task<IActionResult> ProcessCheckout([FromBody] CheckoutDto dto)
        {
            var orderResponse = await _orderService.ProcessCheckoutAsync(dto);
            return orderResponse == null ? BadRequest("Fallo en el checkout: Stock insuficiente o pago rechazado.") : Ok(orderResponse);
        }
    }
}