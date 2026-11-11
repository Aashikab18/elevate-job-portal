using ElevateJobPortal.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace ElevateJobPortal.Services;

public interface IAccountService
{
    Task<IdentityResult> RegisterAsync(RegisterViewModel model);
    Task<SignInResult> LoginAsync(LoginViewModel model);
    Task LogoutAsync();
}