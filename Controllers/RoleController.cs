using Empresa.Data;
using Empresa.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net;

namespace Empresa.Controllers
{
    public class RoleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        private readonly EmpresaContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(UserManager<IdentityUser> usermanager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager, EmpresaContext context)
        {
            _userManager = usermanager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
        }


        //funciona
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        //funciona
        //Metodo para criar novas roles
        [HttpPost]
        public async Task<IActionResult> CreateRole(Role model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName
                };

                IdentityResult result = await _roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }       


        //Funciona
        //Metodo para listar todas as roles
        [HttpGet]
        public async Task<IActionResult> ListRoles()
        {
            List<IdentityRole> roles = await _roleManager.Roles.ToListAsync();

            return View(roles);
        }


        //Funciona
        //Metodo para devolver os detalhes da role
        [HttpGet]
        public async Task<IActionResult> EditRole(string roleId)
        {
            //First Get the role information from the database
            IdentityRole role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {                
                return View("Error");
            }

            //Populate the EditRoleViewModel from the data retrieved from the database
            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name               
            };

            //Initialize the Users Property to avoid Null Reference Exception while Add the username
            model.Users = new List<string>();

            // Retrieve all the Users
            foreach (var user in _userManager.Users.ToList())
            {
                // If the user is in this role, add the username to
                // Users property of EditRoleViewModel. 
                // This model object is then passed to the view for display
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }

            return View(model);
        }


        //Funciona
        //Metodo para editar uma role
        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByIdAsync(model.Id);
                if (role == null)
                {                    
                    ViewBag.ErrorMessage = $"Role with Id = {model.Id} cannot be found";
                    return View("NotFound");
                }
                else
                {
                    role.Name = model.RoleName;
                    // Update other properties if needed

                    var result = await _roleManager.UpdateAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ListRoles"); // Redirect to the roles list
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    return View(model);
                }
            }

            return View(model);
        }


        //Funciona
        //Metodo para apagar uma role
        [HttpPost]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {                
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("NotFound");
            }

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                // Role deletion successful
                return RedirectToAction("ListRoles"); // Redirect to the roles list page
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            // If we reach here, something went wrong, return to the view
            return View("ListRoles", await _roleManager.Roles.ToListAsync());
        }



        //Funciona
        //Metodo para adicionar users to role
        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            ViewBag.roleId = roleId;


            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("NotFound");
            }

            ViewBag.RollName = role.Name;
            var model = new List<UserRoleViewModel>();

            foreach (var user in _userManager.Users.ToList())
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }

                model.Add(userRoleViewModel);
            }

            return View(model);
        }



        //Funciona
        //Metodo para editar user in role
        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("NotFound");
            }

            for (int i = 0; i < model.Count; i++)
            {
                var user = await _userManager.FindByIdAsync(model[i].UserId);

                IdentityResult? result;


                if (model[i].IsSelected && !(await _userManager.IsInRoleAsync(user, role.Name)))
                {
                    //If IsSelected is true and User is not already in this role, then add the user
                    result = await _userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected && await _userManager.IsInRoleAsync(user, role.Name))
                {
                    //If IsSelected is false and User is already in this role, then remove the user
                    result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    //Don't do anything simply continue the loop
                    continue;
                }

                //If you add or remove any user, please check the Succeeded of the IdentityResult
                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    else
                        return RedirectToAction("EditRole", new { roleId = roleId });
                }
            }

            return RedirectToAction("EditRole", new { roleId = roleId });
        }



























        //----------------------------------------------------------------------------------------
        //Funciona
        //Get all register users
        //public IActionResult RoleIndex()
        //{            
        //    var users = _userManager.Users.Select(c => new AssignRoles()
        //    {
        //        Username = c.UserName,
        //        RoleName = string.Join(",", _userManager.GetRolesAsync(c).Result.ToArray())

        //    }).ToList();


        //    return View(users);
        //}






        //Funciona
        //Get all register users with role asssign
        //public IActionResult RoleIndex()
        //{
        //    var users = _context.Users
        //    .Join(_context.UserRoles, u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
        //    .Join(_roleManager.Roles, ur => ur.ur.RoleId, r => r.Id, (ur, r) => new { ur, r })
        //    .ToList()
        //    .GroupBy(uv => new { uv.ur.u.UserName }).Select(r => new AssignRoles()
        //    {
        //        Username = r.Key.UserName,
        //        RoleName = string.Join(",", r.Select(c => c.r.Name).ToArray())

        //    }).ToList();

        //    return View(users);
        //}



        //funciona
        //metodo para carregar na pagina de designar uma role a um user aparecer os nomes através duma droplist
        //[HttpGet]
        //public IActionResult AssignRoles()
        //{
        //    List<SelectListItem> List = new List<SelectListItem>();
        //    foreach (var role in _roleManager.Roles)
        //        List.Add(new SelectListItem() { Value = role.Name, Text = role.Name });
        //    ViewBag.Roles = List;

        //    List<SelectListItem> List1 = new List<SelectListItem>();
        //    foreach (var user in _userManager.Users)
        //        List1.Add(new SelectListItem() { Value = user.UserName, Text = user.UserName });
        //    ViewBag.Users = List1;

        //    return View();
        //}

        //funciona
        //Metodo para atribuir uma role a um user
        //[HttpPost]
        //public async Task<IActionResult> AssignRoles(AssignRoles roles)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        IdentityUser user = await _userManager.FindByEmailAsync(roles.Username);
        //        if (user != null)
        //        {
        //            await _userManager.AddToRoleAsync(user, roles.RoleName);
        //        }

        //    }

        //    return RedirectToAction("Index", "Home");
        //}
        //----------------------------------------------------------------------------------------





    }
}
