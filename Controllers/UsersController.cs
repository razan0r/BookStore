using BookStore.Data;
using BookStore.Models;
using BookStore.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net;

namespace BookStore.Controllers
{
    [Authorize(Roles ="Admin")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public UsersController(ApplicationDbContext context,RoleManager<IdentityRole> roleManager,UserManager<ApplicationUser>  userManager) {
            this.context = context;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        public async Task<IActionResult>  Index()
        {
            var users = await userManager.Users.ToListAsync();
            var userMiewModels = new List<ApplicationUserVM>();

            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);

                var userViewModel = new ApplicationUserVM
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Address = user.Address,
                    PhoneNumber = user.PhoneNumber,
                    Roles = roles.ToList(),
                };
                userMiewModels.Add(userViewModel);
            }


            return View(userMiewModels);
        }

        [HttpGet]
        public async Task<IActionResult>  Create()
        {
            var roles =await roleManager.Roles.ToListAsync();

            var viewModel = new ApplicationUserCreateVm
            {
                Roles = roles.Select(role => new SelectListItem
                {
                    Value = role.Name,
                    Text = role.Name
                }
                ).ToList(),
        };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ApplicationUserCreateVm uservm)
        {
            if(!ModelState.IsValid)
            {
                return View(uservm);
            }
            var user = new ApplicationUser
            {
                UserName = uservm.UserName,
                Email = uservm.Email,
                PhoneNumber = uservm.PhoneNumber,
                Address = uservm.Address,
            };

           var result= await userManager.CreateAsync(user, uservm.PasswordHash);

            if (!result.Succeeded)
            {
                return View(uservm);
            }
           await userManager.AddToRolesAsync(user, uservm.SelectedRoles);
                return RedirectToAction("index");



        }


    }
}
