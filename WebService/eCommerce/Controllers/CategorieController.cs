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
    public class CategorieController : Controller
    {
        private readonly ICategoriesService _categorieService;
        private readonly ILogger<CategorieController> _logger;

        public CategorieController(ILogger<CategorieController> logger, ICategoriesService categorieService)
        {
            _logger = logger;
            _categorieService = categorieService;
        }

        [HttpGet]
        public IActionResult ListCategories(int? pageNumber) 
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

            var token = HttpContext.Session.GetString("Token");

            var categorie = _categorieService.GetCategories(token);

            PaginatedList<Categories> pageCategorie = PaginatedList<Categories>.CreateAsync(categorie, pageNumber ?? 1, 5);
            
            return View(pageCategorie);
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
        public IActionResult Create(Categories model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                var token = HttpContext.Session.GetString("Token");

                _categorieService.AddCategories(model, token);

                return RedirectToAction("ListCategories", "Categorie");
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

            if (!userInfo.Role.Contains("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }

            var token = HttpContext.Session.GetString("Token");

            var categorie = _categorieService.GetCategorieById(int.Parse(id), token);

            if (categorie == null)
            {
                return RedirectToAction("ListCategories", "Categorie");
            }

            return View(categorie);
        }

        [HttpPost]
        public IActionResult Edit(Categories model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                var token = HttpContext.Session.GetString("Token");

                _categorieService.Update(model.Id, model, token);

                return RedirectToAction("ListCategories", "Categorie");
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

            if (!userInfo.Role.Contains("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }

            var token = HttpContext.Session.GetString("Token");

            _categorieService.Delete(int.Parse(id), token);

            return RedirectToAction("ListCategories", "Categorie");
        }
    }
}