using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LojaVirtual.Data;
using LojaVirtual.Models;
using LojaVirtual.Requests;
using LojaVirtual.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;

namespace LojaVirtual.Controllers
{
    public class AccountController : Controller
    {
        // Properties
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly RegisterService _registerService;
        private readonly LoginService _loginService;
        private readonly MailService _mail;

        // Constructor
        public AccountController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager, RegisterService registerService, LoginService loginService, MailService mail)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _registerService = registerService;
            _loginService = loginService;
            _mail = mail;
        }
        
        [HttpGet("/login")]
        public async Task<IActionResult> Login()
        {
            try
            {

                /*if (await _roleManager.RoleExistsAsync("Admin"))
                {
                    var result = await _userManager.CreateAsync(new()
                    {
                        Email = "admin@admin.com",
                        UserName = "Administrator",
                        CarrinhoComprasId = Guid.NewGuid().ToString()
                    }, "Admin@2022");
                    if (result.Succeeded)
                    {
                        var user = await _userManager.FindByEmailAsync("admin@admin.com");
                        await _userManager.AddToRoleAsync(user, "Admin");
                    }
                }
                else
                {
                    var result = await _userManager.CreateAsync(new()
                    {
                        Email = "admin@admin.com",
                        UserName = "Administrator",
                        CarrinhoComprasId = Guid.NewGuid().ToString()
                    }, "admin@2022");
                    if (result.Succeeded)
                    {
                        var user = await _userManager.FindByEmailAsync("admin@admin.com");
                        await _roleManager.CreateAsync(new()
                        {
                            Name = "Admin"
                        });
                        await _userManager.AddToRoleAsync(user, "Admin");
                    }
                }*/

                return View();

            }
            catch (Exception e)
            {
                return View("Login");
            }
        }

        [HttpPost("/LoginService")]
        public async Task<IActionResult> LoginService(LoginRequest request)
        {
            try
            {
                var task = await _loginService.Login(request);
                if (task) return RedirectToAction("Index", "Home");
                return RedirectToAction("Login", "Account");
            }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Account");
            }
        }

        
        [HttpGet("/register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("/RegisterService")]
        public async Task<IActionResult> RegisterService(RegisterRequest request)
        {
            try
            {

                var result = await _registerService.Register(request);
                await _mail.SendMail(new ()
                {
                    Subject = "Created account =)",
                    EmailTo = request.Email,
                    NameTo = request.UserName,
                    plainText = "Your account has been created with success",
                    htmlContent = "Your account has been created with success"
                });
                if (result) return RedirectToAction("Index", "Home");
                
                
                return RedirectToAction("Login", "Account");
            }
            catch (Exception e)
            {
                return RedirectToAction("Register", "Account");
            }
        }

        
        
        [HttpGet("/logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Home");
            }
        } 
    }
}