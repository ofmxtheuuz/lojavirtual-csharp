using LojaVirtual.Models;
using LojaVirtual.Requests;
using Microsoft.AspNetCore.Identity;

namespace LojaVirtual.Services;

public class LoginService
{
    private SignInManager<User> _signInManager;

    public LoginService(SignInManager<User> signInManager)
    {
        _signInManager = signInManager;
    }

    public async Task<bool> Login(LoginRequest request)
    {
        var result = await _signInManager.PasswordSignInAsync(request.UserName, request.Password, false, false);
        if (result.Succeeded) return true;
        return false;
    }
}