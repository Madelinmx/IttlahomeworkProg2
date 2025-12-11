using PetShop.Application.DtosAppointment;
using System;
using System.Collections.Generic;
namespace PetShop.Application.Contract
{
    public interface IAppointmentService
    {
        Task<AppointmentDto?> CreateAppointmentAsync(AppointmentCreateDto appointmentDto);
        Task<List<AppointmentDto>> GetUserAppointmentsAsync(int userId);
        Task<bool> IsSlotAvailableAsync(int serviceId, DateTime requestedTime);
    }
}