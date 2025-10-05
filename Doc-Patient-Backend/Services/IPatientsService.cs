using Doc_Patient_Backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Doc_Patient_Backend.Services
{
    public interface IPatientsService
    {
        Task<(IEnumerable<ApplicationUser> Patients, int TotalCount)> GetPatientsAsync(int page, int pageSize);
        Task<ApplicationUser> ToggleBlockPatientAsync(string id);
    }
}