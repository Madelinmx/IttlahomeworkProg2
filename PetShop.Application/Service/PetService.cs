using PetShop.Application.Contract;
using PetShop.Application.DtosPet;
using PetShop.Domain.Repositories;
using PetShop.Domain.Entities;

namespace PetShop.Application.Service
{
    public class PetService : IPetService
    {
        private readonly IPetRepository _petRepository;

        public PetService(IPetRepository petRepository)
        {
            _petRepository = petRepository;
        }

        public async Task<List<PetDto>> GetAvailablePetsAsync()
        {
            var pets = await _petRepository.GetAllAvailableAsync();

            // Mapeo de Entidad a DTO
            return pets.Select(p => new PetDto
            {
                Id = p.Id,
                Name = p.Name,
                Type = p.Type,
                Age = p.Age,
                Description = p.Description
            }).ToList();
        }

        public async Task<PetDto?> GetPetDetailsAsync(int id)
        {
            var pet = await _petRepository.GetByIdAsync(id);
            if (pet == null || pet.IsAdopted)
            {
                return null;
            }

            // Mapeo
            return new PetDto
            {
                Id = pet.Id,
                Name = pet.Name,
                Type = pet.Type,
                Age = pet.Age,
                Description = pet.Description
            };
        }

        public async Task<bool> MarkAsAdoptedAsync(int petId)
        {
            // Lógica de negocio para cambiar el estado
            var pet = await _petRepository.GetByIdAsync(petId);
            if (pet == null || pet.IsAdopted)
            {
                return false; // Ya adoptado o no existe
            }

            // Llama al repositorio para la actualización
            await _petRepository.UpdateStatusAsync(petId, true);
            return true;
        }

        // Método para el Admin
        public async Task<PetDto?> AddNewPetAsync(PetCreateDto petDto)
        {
            var petEntity = new Pet
            {
                Name = petDto.Name,
                Type = petDto.Type,
                Age = petDto.Age,
                Description = petDto.Description,
                IsAdopted = false
            };

            await _petRepository.AddAsync(petEntity);

            // Se asume que el repositorio asigna el ID y lo actualiza en la entidad.
            return new PetDto
            {
                Id = petEntity.Id,
                Name = petEntity.Name,
                Type = petEntity.Type,
                Age = petEntity.Age,
                Description = petEntity.Description
            };
        }
    }
}