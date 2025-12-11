using Microsoft.AspNetCore.Mvc;
using PetShop.Application.Contract;
using PetShop.Application.DtosAppointment;
namespace PetShop.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        public AppointmentController(IAppointmentService appointmentService) => _appointmentService = appointmentService;
        [HttpPost("book")]
        public async Task<IActionResult> BookAppointment([FromBody] AppointmentCreateDto dto)
        {
            var newAppointment = await _appointmentService.CreateAppointmentAsync(dto);
            return newAppointment == null ? BadRequest("La hora solicitada no está disponible.") : StatusCode(201, newAppointment);
        }
    }
}