using ApisJwt.Helpers;
using ApisJwt.ViewModels;

namespace ApisJwt.Services.Auth
{
    public interface IAuthService
    {
        Task<ApiResponse<AuthViewModel>> Register(RegisterViewModel model);
    }
}
