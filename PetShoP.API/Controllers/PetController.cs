using Microsoft.AspNetCore.Mvc;
using PetShop.Application.Contract;
using PetShop.Application.DtosPet;

namespace PetShop.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PetController : ControllerBase
    {
        private readonly IPetService _petService;

        public PetController(IPetService petService)
        {
            _petService = petService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAvailablePets()
        {
            var pets = await _petService.GetAvailablePetsAsync();
            return Ok(pets); 
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPetDetails(int id)
        {
            var pet = await _petService.GetPetDetailsAsync(id);
            if (pet == null)
            {
                return NotFound($"Mascota con ID {id} no disponible para adopción.");
            }
            return Ok(pet);
        }

       
        [HttpPost("adopt/{petId}")]
       
        public async Task<IActionResult> MarkAsAdopted(int petId)
        {
            var success = await _petService.MarkAsAdoptedAsync(petId);
            if (!success)
            {
                return BadRequest("La mascota ya está adoptada o no existe.");
            }
            return Ok($"Mascota con ID {petId} marcada como adoptada con éxito.");
        }

       
        [HttpPost]
       
        public async Task<IActionResult> AddNewPet([FromBody] PetCreateDto petDto)
        {
            var newPet = await _petService.AddNewPetAsync(petDto);
            return StatusCode(201, newPet);
        }
    }
}