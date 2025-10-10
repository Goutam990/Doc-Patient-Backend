using Doc_Patient_Backend.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Doc_Patient_Backend.Services
{
    public interface IPatientService
    {
        Task<IEnumerable<AppointmentDto>> GetUpcomingAppointmentsAsync(string patientUserId);
        Task<IEnumerable<AppointmentDto>> GetCompletedAppointmentsAsync(string patientUserId);
        Task<IEnumerable<AppointmentDto>> GetAllAppointmentsAsync(string patientUserId);
        Task<(AppointmentDto CreatedAppointment, string ErrorMessage)> BookAppointmentAsync(string patientUserId, CreateAppointmentDto createAppointmentDto);
        Task<(bool Succeeded, string ErrorMessage)> UpdateAppointmentAsync(string patientUserId, int appointmentId, UpdateAppointmentDto updateAppointmentDto);
        Task<(bool Succeeded, string ErrorMessage)> CancelAppointmentAsync(string patientUserId, int appointmentId);
    }
}
