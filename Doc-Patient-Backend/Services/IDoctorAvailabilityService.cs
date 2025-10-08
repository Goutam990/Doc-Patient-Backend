using Doc_Patient_Backend.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Doc_Patient_Backend.Services
{
    public interface IDoctorAvailabilityService
    {
        Task<DoctorAvailability> AddVacationDayAsync(string doctorId, DateTime vacationDate, string reason);
        Task<IEnumerable<DoctorAvailability>> GetVacationDaysAsync(string doctorId);
        Task<bool> DeleteVacationDayAsync(int availabilityId, string doctorId);
    }
}