using PetShop.Application.Contract;
using PetShop.Application.DtosAppointment;
using PetShop.Domain.Repositories;
using PetShop.Domain.Entities;
using System;
namespace PetShop.Application.Service
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IUserRepository _userRepository;

        // SIMULACIÓN de ServiceRepository
        private static readonly List<Service> _availableServices = new List<Service>
        {
            new Service { Id = 1, Name = "Baño y Secado", Price = 25.00m, DurationMinutes = 60 },
            new Service { Id = 2, Name = "Corte de Pelo", Price = 40.00m, DurationMinutes = 90 }
        };
        private Service? GetServiceDuration(int serviceId) =>
            _availableServices.FirstOrDefault(s => s.Id == serviceId);

        public AppointmentService(IAppointmentRepository appointmentRepository, IUserRepository userRepository)
        {
            _appointmentRepository = appointmentRepository;
            _userRepository = userRepository;
        }

        public async Task<bool> IsSlotAvailableAsync(int serviceId, DateTime requestedTime)
        {
            var service = GetServiceDuration(serviceId);
            if (service == null) return false;
            DateTime endTime = requestedTime.AddMinutes(service.DurationMinutes);
            var existingAppointments = await _appointmentRepository.GetByDateRangeAsync(requestedTime.Date, requestedTime.Date.AddDays(1));

            // Verifica solapamiento de tiempo
            return !existingAppointments.Any(existing => (requestedTime < existing.EndTime && endTime > existing.StartTime));
        }

        public async Task<AppointmentDto?> CreateAppointmentAsync(AppointmentCreateDto dto)
        {
            var user = await _userRepository.GetByIdAsync(dto.UserId);
            var service = GetServiceDuration(dto.ServiceId);
            if (user == null || service == null || !await IsSlotAvailableAsync(dto.ServiceId, dto.RequestedStartTime)) return null;

            var appointment = new Appointment
            {
                UserId = dto.UserId,
                ServiceId = dto.ServiceId,
                StartTime = dto.RequestedStartTime,
                EndTime = dto.RequestedStartTime.AddMinutes(service.DurationMinutes),
                Status = "Confirmed"
            };
            await _appointmentRepository.AddAsync(appointment);

            return new AppointmentDto
            {
                Id = appointment.Id,
                ServiceName = service.Name,
                StartTime = appointment.StartTime,
                EndTime = appointment.EndTime,
                Status = appointment.Status
            };
        }
        public Task<List<AppointmentDto>> GetUserAppointmentsAsync(int userId)
        {
            // Implementación simplificada
            throw new NotImplementedException();
        }
    }
}