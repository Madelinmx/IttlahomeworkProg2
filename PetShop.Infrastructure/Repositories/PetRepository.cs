using PetShop.Domain.Entities;
using PetShop.Domain.Repositories;

namespace PetShop.Infrastructure.Repositories
{
    public class PetRepository : IPetRepository
    {
        // SIMULACIÓN: Lista en memoria.
        private static readonly List<Pet> _pets = new List<Pet>
        {
            new Pet { Id = 10, Name = "Max", Type = "Perro", Age = 3, Description = "Muy juguetón.", IsAdopted = false },
            new Pet { Id = 11, Name = "Miso", Type = "Gato", Age = 1, Description = "Tímido pero cariñoso.", IsAdopted = false },
            new Pet { Id = 12, Name = "Rocky", Type = "Perro", Age = 7, Description = "Ya fue adoptado.", IsAdopted = true }
        };

        public Task AddAsync(Pet pet)
        {
            pet.Id = _pets.Any() ? _pets.Max(p => p.Id) + 1 : 1;
            _pets.Add(pet);
            return Task.CompletedTask;
        }

        public Task<Pet?> GetByIdAsync(int id)
        {
            return Task.FromResult(_pets.FirstOrDefault(p => p.Id == id));
        }

        public Task<List<Pet>> GetAllAvailableAsync()
        {
            // Filtra solo los que NO están adoptados
            return Task.FromResult(_pets.Where(p => !p.IsAdopted).ToList());
        }

        public Task UpdateStatusAsync(int petId, bool isAdopted)
        {
            var petToUpdate = _pets.FirstOrDefault(p => p.Id == petId);
            if (petToUpdate != null)
            {
                petToUpdate.IsAdopted = isAdopted;
            }
            return Task.CompletedTask;
        }
    }
}