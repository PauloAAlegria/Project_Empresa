using Empresa.Data;
using Empresa.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NuGet.Protocol.Plugins;
using System.Collections.Generic;
using System.Data;

namespace Empresa.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        private readonly EmpresaContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<IdentityUser> usermanager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager, EmpresaContext context)
        {
            _userManager = usermanager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
        }       


        //Carregar a pagina de login
        public IActionResult Login()
        {
            return View();
        }


        //Metodo para login
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, true, true);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Incorrect Username or Password");
            }

            return View(model);
        }


        //Carregar a pagina de registo
        public IActionResult Register()
        {
            return View();
        }
        

        //metodo para registar
        [HttpPost]
        public async Task<IActionResult> Register(Register model, Colaborador Colaborador)
        {
            // variavel criada para verificar se o email introduzido no registo pertence a um colaborador
            // caso exista, deixa fazer o registo, senão não consegue entrar no registo da empresa
            var list = _context.Colaborador.Any(x => x.Email == model.Username);          

            if (list)
            {
                if (ModelState.IsValid)
                {
                    var user = new IdentityUser
                    {
                        UserName = model.Username,
                        Email = model.Username

                    };
                    var result = await _userManager.CreateAsync(user, model.Password);                    
                    if (result.Succeeded)
                    {
                        //add role here
                        //await _userManager.AddToRoleAsync(user, "Admin");
                        return RedirectToAction("Login", "Account");
                    }
                }
                ModelState.AddModelError("", "Invalid Register.");
                
            }
            else
            {
                ModelState.AddModelError("", "Email dosen't exist in database.");
            }
            
            
            return View(model);
        }

        

        //Metodo para fazer logout
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

    }
}
