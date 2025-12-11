using PetShop.Application.DtosUser;
namespace PetShop.Application.Contract
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(UserRegisterDto userDto);
        Task<AuthResponseDto?> LoginAsync(UserLoginDto loginDto);
    }
}