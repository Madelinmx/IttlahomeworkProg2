using PetShop.Application.DtosOrder;
using System.Threading.Tasks;
namespace PetShop.Application.Contract
{
    public interface IOrderService
    {
        Task<OrderResponseDto?> ProcessCheckoutAsync(CheckoutDto checkout);
    }
}