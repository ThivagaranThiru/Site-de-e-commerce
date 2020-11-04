using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerce.Models;
using eCommerce.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace eCommerce.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IUsersService _usersService;
        private readonly IRolesService _roleService;
        private readonly IAdressesService _adressesService;
       
        public AccountController(ILogger<AccountController> logger, IUsersService usersService, IRolesService rolesService, IAdressesService adressesService)
        {
            _logger = logger;
            _usersService = usersService;
            _roleService = rolesService;
            _adressesService = adressesService;
        }

        [HttpGet]
        public IActionResult UserRegister()
        {
            if (HttpContext.Session.GetString("SessionUser") != null || HttpContext.Session.GetString("Token") != null)
            {
                return RedirectToAction("Index", "Home");
            }

            Users user = new Users();

            user.role = _roleService.GetRoles().ToList()
                .ConvertAll(a =>
                {
                    return new SelectListItem()
                    {
                        Text = a.Name,
                        Value = a.Id,
                        Selected = false
                    };
                });

            return View(user);
        }

        [HttpPost]
        public IActionResult UserRegister(Users model)
        {
            if (ModelState.IsValid)
            {
                Console.WriteLine("Role ID :" + model.RoleId);

                Users userResult = _usersService.Register(model);

                if(userResult != null)
                {
                    UserSession userSession = new UserSession()
                    {
                        Id = userResult.Id,
                        UserName = userResult.UserName,
                        Email = userResult.Email,
                        Role = userResult.RoleName
                    };

                    Console.WriteLine("Role Attribute to : " + userSession.Role);

                    HttpContext.Session.SetString("SessionUser", JsonConvert.SerializeObject(userSession));

                    HttpContext.Session.SetString("Token", userResult.token);

                    Console.WriteLine("TOKEN REGISTER " + userResult.token);

                    return RedirectToAction("AdresseRegister", "Account");
                }
            }

            model.role = _roleService.GetRoles().ToList()
              .ConvertAll(a =>
              {
                  return new SelectListItem()
                  {
                      Text = a.Name,
                      Value = a.Id,
                      Selected = false
                  };
              });

            return View(model);
        }

        [HttpGet]
        public IActionResult ChooseRole()
        {
            if (HttpContext.Session.GetString("SessionUser") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var userInfo = JsonConvert.DeserializeObject<UserSession>(HttpContext.Session.GetString("SessionUser"));

            if(userInfo.Role != null)
            {
                return RedirectToAction("Index", "Home");
            }

            var token = HttpContext.Session.GetString("Token");

            Roles model = _roleService.SelectRole(token);

            return View(model);
        }

        [HttpPost]
        public IActionResult ChooseRole(Roles model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserSession>(HttpContext.Session.GetString("SessionUser"));

                var token = HttpContext.Session.GetString("Token");

                _roleService.AddRoleToUser(model.Name, userInfo.Email, token);

                userInfo.Role = model.Name;

                HttpContext.Session.SetString("SessionUser", JsonConvert.SerializeObject(userInfo));

                return RedirectToAction("AdresseRegister", "Account");
            }
        }

        [HttpGet]
        public IActionResult AdresseRegister()
        {
            if (HttpContext.Session.GetString("SessionUser") == null || HttpContext.Session.GetString("Token") == null)
            {
                return RedirectToAction("Logout", "Account");
            }

            var userInfo = JsonConvert.DeserializeObject<UserSession>(HttpContext.Session.GetString("SessionUser"));

            if (userInfo.AdresseId > 0)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public IActionResult AdresseRegister(Adresses model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                if (HttpContext.Session.GetString("SessionUser") == null || HttpContext.Session.GetString("Token") == null)
                {
                    return RedirectToAction("Logout", "Account");
                }

                var userInfo = JsonConvert.DeserializeObject<UserSession>(HttpContext.Session.GetString("SessionUser"));

                var token = HttpContext.Session.GetString("Token");

                model.UserId = userInfo.Id;

                var adresse = _adressesService.AddAdresses(model, token);

                if(adresse == null)
                {
                    return View();
                }

                userInfo.AdresseId = adresse.Id;

                HttpContext.Session.SetString("SessionUser", JsonConvert.SerializeObject(userInfo));

                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("SessionUser") != null || HttpContext.Session.GetString("Token") != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Login(Login model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                Users userResult = _usersService.Login(model);

                if(userResult == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                Adresses adresse = _adressesService.GetAdressesByUser(userResult.Id, userResult.token);

                UserSession userSession = new UserSession()
                {
                    Id = userResult.Id,
                    UserName = userResult.UserName,
                    Email = userResult.Email,
                    Role = userResult.RoleName,
                    AdresseId = (adresse != null && adresse.Id > 0 ) ? adresse.Id : 0
                };

                Console.WriteLine("id " + userSession.Id);
                Console.WriteLine("userSession.UserName " + userSession.UserName);
                Console.WriteLine("Email " + userSession.Email);
                Console.WriteLine("Role " + userSession.Role);
                Console.WriteLine("Adresse " + userSession.AdresseId);

                HttpContext.Session.SetString("SessionUser", JsonConvert.SerializeObject(userSession));
                
                HttpContext.Session.SetString("Token", userResult.token);

                Console.WriteLine("TOKEN REGISTER " + userResult.token);

                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult Logout()
        {
            var token = HttpContext.Session.GetString("Token");

            _usersService.Logout(token);

            HttpContext.Session.Clear();

            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult MyAdresse()
        {
            if (HttpContext.Session.GetString("SessionUser") == null || HttpContext.Session.GetString("Token") == null)
            {
                return RedirectToAction("Logout", "Account");
            }

            var userInfo = JsonConvert.DeserializeObject<UserSession>(HttpContext.Session.GetString("SessionUser"));

            if (userInfo.AdresseId <= 0)
            {
                return RedirectToAction("AdresseRegister", "Account");
            }

            var token = HttpContext.Session.GetString("Token");

            var adresses = _adressesService.GetAdressesById(userInfo.AdresseId, token);

            return View(adresses);
        }

        [HttpPost]
        public IActionResult MyAdresse(Adresses model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                if (HttpContext.Session.GetString("SessionUser") == null || HttpContext.Session.GetString("Token") == null)
                {
                    return RedirectToAction("Logout", "Account");
                }

                var userInfo = JsonConvert.DeserializeObject<UserSession>(HttpContext.Session.GetString("SessionUser"));
                var token = HttpContext.Session.GetString("Token");

                var adresses = _adressesService.GetAdressesById(userInfo.AdresseId, token);

                model.Id = adresses.Id;

                _adressesService.Update(adresses.Id, model, token);

                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public IActionResult MyProfil()
        {
            if (HttpContext.Session.GetString("SessionUser") == null || HttpContext.Session.GetString("Token") == null)
            {
                return RedirectToAction("Logout", "Account");
            }

            var userInfo = JsonConvert.DeserializeObject<UserSession>(HttpContext.Session.GetString("SessionUser"));
            
            if (userInfo.Id == null)
            {
                return RedirectToAction("Logout", "Account");
            }

            var token = HttpContext.Session.GetString("Token");

            var user = _usersService.GetUserById(userInfo.Id, token);

            UserProfil userProfil = new UserProfil
            {
                Id = user.Id,
                Civilite = user.Civilite,
                Email = user.Email,
                UserName = user.UserName,
                FirstName = user.FirstName,
                Name = user.Name,
                DateOfBirth = user.DateOfBirth
            };

            return View(userProfil);
        }

        [HttpPost]
        public IActionResult MyProfil(UserProfil model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                if (HttpContext.Session.GetString("SessionUser") == null || HttpContext.Session.GetString("Token") == null)
                {
                    return RedirectToAction("Logout", "Account");
                }

                var userInfo = JsonConvert.DeserializeObject<UserSession>(HttpContext.Session.GetString("SessionUser"));

                var token = HttpContext.Session.GetString("Token");

                var user = _usersService.GetUserById(userInfo.Id, token);

                model.Id = user.Id;

                Users userNew = new Users
                {
                    Id = model.Id,
                    Civilite = model.Civilite,
                    Email = model.Email,
                    UserName = model.UserName,
                    FirstName = model.FirstName,
                    Name = model.Name,
                    DateOfBirth = model.DateOfBirth
                };

                _usersService.Update(userInfo.Id, userNew, token);

                return RedirectToAction("Index", "Home");
            }
        }
        public IActionResult DeleteMe(string id)
        {
            if (HttpContext.Session.GetString("SessionUser") == null || HttpContext.Session.GetString("Token") == null)
            {
                return RedirectToAction("Logout", "Account");
            }

            var token = HttpContext.Session.GetString("Token");

            _usersService.Delete(id, token);

            return RedirectToAction("Logout", "Account");
        }
    }
}