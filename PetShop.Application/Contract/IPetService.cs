using PetShop.Application.DtosPet;

namespace PetShop.Application.Contract
{
    public interface IPetService
    {
        Task<List<PetDto>> GetAvailablePetsAsync();
        Task<PetDto?> GetPetDetailsAsync(int id);
        Task<bool> MarkAsAdoptedAsync(int petId); // Lógica para cambiar el estado
        Task<PetDto?> AddNewPetAsync(PetCreateDto petDto); // Para el Admin
    }
}