using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Doc_Patient_Backend.Services
{
    public interface ISettingsService
    {
        Task<List<string>> GetAvailableSlotsAsync(DateTime date, string doctorId);
    }
}