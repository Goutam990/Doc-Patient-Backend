using Doc_Patient_Backend.Models;
using Doc_Patient_Backend.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Doc_Patient_Backend.Services
{
    public interface IAppointmentService
    {
        Task<IEnumerable<object>> GetAllAppointmentsAsync();
        Task<IEnumerable<object>> GetDoneAppointmentsAsync();
        Task<bool> ChangeAppointmentStatusAsync(int appointmentId, bool isDone);
        Task<Appointment> CreateNewAppointmentAsync(NewAppointment newAppointment);
    }
}