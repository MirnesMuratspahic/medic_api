using MedicLab.Models;
using MedicLab.Models.DTO;
using Microsoft.Win32;
using System.Text.RegularExpressions;

namespace MedicLab.Services.Interfaces
{
    public interface IUserService
    {
        ///login: Admin login authentication endpoint.
        Task<(ErrorProvider, string)> Login(dtoUserLogin userLogin);
        ///users: Fetch all users endpoint.
        Task<(ErrorProvider, List<dtoUserInformation>)> GetAllUsers();
        ///users/details/(id): Fetch details of a specific user endpoint.
        Task<(ErrorProvider, dtoUserInformation)> GetUserById(int id);
        ///users/block/(id): Block a user by ID endpoint.
        Task<ErrorProvider> BlockUserById(int id);
        ///logout: Admin logout endpoint.
        Task<ErrorProvider> LogOut(HttpRequest request);
        ///register: Register/add user endpoint.
        Task<ErrorProvider> Register(dtoUserRegistration userRegistration);
    }
}
