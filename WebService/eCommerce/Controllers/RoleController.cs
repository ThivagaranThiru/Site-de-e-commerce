using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerce.Models;
using eCommerce.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace eCommerce.Controllers
{
    public class RoleController : Controller
    {
        private readonly IRolesService _roleService;
        private readonly ILogger<RoleController> _logger;

        public RoleController(ILogger<RoleController> logger, IRolesService rolesService)
        {
            _logger = logger;
            _roleService = rolesService;
        }

        [HttpGet]
        public IActionResult ListRoles(int? pageNumber)
        {
            if (HttpContext.Session.GetString("SessionUser") == null || HttpContext.Session.GetString("Token") == null)
            {
                return RedirectToAction("Logout", "Account");
            }

            var userInfo = JsonConvert.DeserializeObject<UserSession>(HttpContext.Session.GetString("SessionUser"));

            if (!userInfo.Role.Contains("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }

            var roles = _roleService.GetRoles();

            PaginatedList<Roles> pageRole = PaginatedList<Roles>.CreateAsync(roles, pageNumber ?? 1, 5);

            return View(pageRole);
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("SessionUser") == null || HttpContext.Session.GetString("Token") == null)
            {
                return RedirectToAction("Logout", "Account");
            }

            var userInfo = JsonConvert.DeserializeObject<UserSession>(HttpContext.Session.GetString("SessionUser"));

            if (!userInfo.Role.Contains("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Create(Roles model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                var token = HttpContext.Session.GetString("Token");
                _roleService.AddRoles(model,token);

                return RedirectToAction("ListRoles", "Role");
            }
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            if (HttpContext.Session.GetString("SessionUser") == null || HttpContext.Session.GetString("Token") == null)
            {
                return RedirectToAction("Logout", "Account");
            }

            var userInfo = JsonConvert.DeserializeObject<UserSession>(HttpContext.Session.GetString("SessionUser"));
            var token = HttpContext.Session.GetString("Token");

            if (!userInfo.Role.Contains("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }

            var role = _roleService.GetRolesById(id, token);
            
            if(role == null)
            {
                return RedirectToAction("ListRoles", "Role");
            }

            return View(role);
        }

        [HttpPost]
        public IActionResult Edit(Roles model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                var token = HttpContext.Session.GetString("Token");
                _roleService.Update(model.Id, model, token);

                return RedirectToAction("ListRoles", "Role");
            };
        }

        [HttpGet]
        public IActionResult Delete(string id)
        {
            if (HttpContext.Session.GetString("SessionUser") == null || HttpContext.Session.GetString("Token") == null)
            {
                return RedirectToAction("Logout", "Account");
            }

            var userInfo = JsonConvert.DeserializeObject<UserSession>(HttpContext.Session.GetString("SessionUser"));
            var token = HttpContext.Session.GetString("Token");

            if (!userInfo.Role.Contains("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }

            _roleService.Delete(id, token);

            return RedirectToAction("ListRoles", "Role");
        }
    }
}