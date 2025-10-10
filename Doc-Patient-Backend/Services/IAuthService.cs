using Doc_Patient_Backend.Models.DTOs;
using System.Threading.Tasks;

namespace Doc_Patient_Backend.Services
{
    public interface IAuthService
    {
        Task<(bool Succeeded, string ErrorMessage)> RegisterAsync(RegisterDto registerDto);
        Task<(string Token, string ErrorMessage)> LoginAsync(LoginDto loginDto);
    }
}
