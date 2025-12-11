using PetShop.Domain.Entities;
using PetShop.Domain.Repositories;
using System;
namespace PetShop.Infrastructure.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private static readonly List<Appointment> _appointments = new List<Appointment>
        {
            new Appointment {
                Id = 1, UserId = 101, ServiceId = 1,
                StartTime = new DateTime(2025, 12, 11, 10, 0, 0), EndTime = new DateTime(2025, 12, 11, 11, 0, 0),
                Status = "Confirmed"
            }
        };

        public Task AddAsync(Appointment appointment)
        {
            appointment.Id = _appointments.Any() ? _appointments.Max(a => a.Id) + 1 : 1;
            _appointments.Add(appointment);
            return Task.CompletedTask;
        }
        public Task<List<Appointment>> GetByDateRangeAsync(DateTime start, DateTime end)
        {
            var results = _appointments
                .Where(a => a.StartTime >= start && a.StartTime < end)
                .ToList();
            return Task.FromResult(results);
        }
        public Task<List<Appointment>> GetByUserIdAsync(int userId) =>
            Task.FromResult(_appointments.Where(a => a.UserId == userId).ToList());
    }
}