using ElevateJobPortal.Models;
using ElevateJobPortal.Repositories;
using ElevateJobPortal.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace ElevateJobPortal.Services;

public class AccountService : IAccountService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ICompanyRepository _companyRepository;

    public AccountService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ICompanyRepository companyRepository)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _companyRepository = companyRepository;
    }

    public async Task<IdentityResult> RegisterAsync(RegisterViewModel model)
    {
        if (model.Role != "JobSeeker" && model.Role != "Company")
            return IdentityResult.Failed(new IdentityError { Description = "Invalid role selected." });

        if (model.Role == "Company" && string.IsNullOrWhiteSpace(model.CompanyName))
            return IdentityResult.Failed(new IdentityError { Description = "Company name is required for companies." });

        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            FullName = model.FullName
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded) return result;

        await _userManager.AddToRoleAsync(user, model.Role);

        if (model.Role == "Company")
        {
            await _companyRepository.AddAsync(new Company
            {
                UserId = user.Id,
                Name = model.CompanyName!.Trim()
            });
        }

        await _signInManager.SignInAsync(user, isPersistent: false);
        return result;
    }

    public Task<SignInResult> LoginAsync(LoginViewModel model)
    {
        return _signInManager.PasswordSignInAsync(
            model.Email, model.Password, model.RememberMe, lockoutOnFailure: true);
    }

    public Task LogoutAsync()
    {
        return _signInManager.SignOutAsync();
    }
}