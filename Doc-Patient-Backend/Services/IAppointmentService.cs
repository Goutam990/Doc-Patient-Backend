using Doc_Patient_Backend.Models;
using Doc_Patient_Backend.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Doc_Patient_Backend.Services
{
    public interface IAppointmentService
    {
        Task<IEnumerable<AppointmentDto>> GetAllAppointmentsAsync();
        Task<IEnumerable<AppointmentDto>> GetUpcomingAppointmentsForPatientAsync(string patientId);
        Task<bool> ChangeAppointmentStatusAsync(int appointmentId, string status);
        Task<(Appointment, string Error)> CreateNewAppointmentAsync(CreateAppointmentDto createAppointmentDto, string patientId);
        Task<(bool, string Error)> CancelAppointmentAsync(int appointmentId, string patientId);
    }
}