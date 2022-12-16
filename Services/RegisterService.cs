using System.Security.Claims;
using LojaVirtual.Models;
using LojaVirtual.Requests;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LojaVirtual.Services;

public class RegisterService
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly TokenService _tokenService;

    public RegisterService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, TokenService tokenService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _tokenService = tokenService;
    }

    public async Task<dynamic> Register(RegisterRequest request)
    {

        var token = Guid.NewGuid().ToString();
        var task = await _userManager.CreateAsync(new()
        {
            UserName = request.UserName,
            Email = request.Email,
            CarrinhoComprasId = Guid.NewGuid().ToString(),
            APIKey = token,
            CreatedDate = DateTime.Now
        }, request.Password);
        if (task.Succeeded)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (!await _roleManager.RoleExistsAsync("Member"))
            {
                await _roleManager.CreateAsync(new()
                {
                    
                });
            }
            task = await _userManager.AddToRoleAsync(user, "Member");

            if (task.Succeeded)
            {
                task = await _userManager.AddClaimsAsync(user, new []
                {
                    new Claim("USER_ID", user.Id),
                    new Claim("USER_EMAIL", user.Email),
                    new Claim("USER_CARRINHO_ID", user.CarrinhoComprasId.ToString()),
                    new Claim("APIKEY", token)
                });
                return true;
            }
        }
        return false;
    }
}