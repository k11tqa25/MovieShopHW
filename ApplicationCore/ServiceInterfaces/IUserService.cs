using ApplicationCore.Models;
using System;
using System.Threading.Tasks;

namespace ApplicationCore.ServiceInterfaces
{
    public interface IUserService
    {
        Task<UserRegisterResponseModel> RegisterUserAsync(UserRegisterRequestModel userRegisterResponseModel);

        Task<UserLoginResponseModel> LoginAsync(string email, string password);
    }
}
