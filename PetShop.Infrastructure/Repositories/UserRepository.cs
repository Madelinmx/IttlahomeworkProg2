using PetShop.Domain.Entities;
using PetShop.Domain.Repositories;
namespace PetShop.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private static readonly List<User> _users = new List<User>
        {
            new User { Id = 101, Email = "admin@petshop.com", Name = "Admin", PasswordHash = "HASH_OF_123456", Role = "Admin" }
        };

        public Task AddAsync(User user)
        {
            user.Id = _users.Any() ? _users.Max(u => u.Id) + 1 : 1;
            _users.Add(user);
            return Task.CompletedTask;
        }
        public Task<User?> GetByEmailAsync(string email) =>
            Task.FromResult(_users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)));
        public Task<User?> GetByIdAsync(int id) =>
            Task.FromResult(_users.FirstOrDefault(u => u.Id == id));
    }
}