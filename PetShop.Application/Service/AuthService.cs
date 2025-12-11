using PetShop.Application.Contract;
using PetShop.Application.DtosUser;
using PetShop.Domain.Repositories;
using PetShop.Domain.Entities;
namespace PetShop.Application.Service
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        public AuthService(IUserRepository userRepository) => _userRepository = userRepository;

        public async Task<bool> RegisterAsync(UserRegisterDto userDto)
        {
            if (await _userRepository.GetByEmailAsync(userDto.Email) != null) return false;
            string passwordHash = $"HASH_OF_{userDto.Password}";
            var newUser = new User { Name = userDto.Name, Email = userDto.Email, PasswordHash = passwordHash, Role = "Client" };
            await _userRepository.AddAsync(newUser);
            return true;
        }
        public async Task<AuthResponseDto?> LoginAsync(UserLoginDto loginDto)
        {
            var user = await _userRepository.GetByEmailAsync(loginDto.Email);
            if (user == null || user.PasswordHash != $"HASH_OF_{loginDto.Password}") return null;

            // Simulación de JWT
            string token = $"FAKE_JWT_{user.Id}_{user.Role}";
            return new AuthResponseDto { Token = token, UserEmail = user.Email, Role = user.Role };
        }
    }
}