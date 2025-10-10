using Doc_Patient_Backend.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Doc_Patient_Backend.Services
{
    public interface IDoctorService
    {
        Task<IEnumerable<PatientDetailsDto>> GetAllPatientsAsync();
        Task<PatientDetailsDto> GetPatientDetailsAsync(int patientInfoId);
        Task<(bool Succeeded, string ErrorMessage)> UpdateAvailabilityAsync(string doctorId, UpdateAvailabilityDto updateAvailabilityDto);
        Task<(bool Succeeded, string ErrorMessage)> ApproveAppointmentAsync(int appointmentId);
        Task<(bool Succeeded, string ErrorMessage)> RejectAppointmentAsync(int appointmentId);
        Task<(AppointmentDto CreatedRevisit, string ErrorMessage)> ScheduleRevisitAsync(int originalAppointmentId);
        Task<IEnumerable<AppointmentDto>> GetUpcomingAppointmentsAsync(string doctorId);
    }
}
