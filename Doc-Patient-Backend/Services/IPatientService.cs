using Doc_Patient_Backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Doc_Patient_Backend.Services
{
    public interface IPatientService
    {
        Task<IEnumerable<Patient>> GetAllPatientsAsync();
        Task<Patient> GetPatientByIdAsync(int id);
        Task<Patient> GetPatientByMobileNoAsync(string mobile);
        Task<Patient> CreatePatientAsync(Patient patient);
        Task<bool> UpdatePatientAsync(int id, Patient patient);
        Task<bool> DeletePatientAsync(int id);
    }
}