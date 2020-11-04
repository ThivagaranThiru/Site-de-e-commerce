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
    public class ProduitController : Controller
    {
        private readonly ILogger<ProduitController> _logger;
        private readonly IProduitsService _produitsService;
        private readonly ICategoriesService _categoriesService;

        public ProduitController(ILogger<ProduitController> logger, IProduitsService produitsService, ICategoriesService categoriesService) 
        {
            _logger = logger;
            _produitsService = produitsService;
            _categoriesService = categoriesService;
        }

        [HttpGet]
        public IActionResult Index(string currentFilter, int ? pageNumber) 
        {
            if (HttpContext.Session.GetString("SessionUser") == null || HttpContext.Session.GetString("Token") == null)
            {
                return RedirectToAction("Logout", "Account");
            }

            ProduitCategoriecs produitCategories = new ProduitCategoriecs();

            var token = HttpContext.Session.GetString("Token");

            IEnumerable<Produits> produits = _produitsService.GetProduits(token);
            
            produitCategories.categories = _categoriesService.GetCategories(token);

            ViewData["CurrentFilter"] = currentFilter;

            if (currentFilter == null || currentFilter.Contains("All"))
            {
                IEnumerable<Produits> produit0 = produits.Where(x => float.Parse(x.Stock) > 0);
                produitCategories.produits = PaginatedList<Produits>.CreateAsync(produit0, pageNumber ?? 1, 5);
            }
            else
            {
                IEnumerable<Produits> produitCate = produits.Where(x => x.CategorieName == currentFilter && int.Parse(x.Stock) > int.Parse("0"));

                produitCategories.produits = PaginatedList<Produits>.CreateAsync(produitCate, pageNumber ?? 1, 5);
            }

            return View(produitCategories);
        }

        [HttpGet]
        public IActionResult Detail(string id)
        {
            if (HttpContext.Session.GetString("SessionUser") == null || HttpContext.Session.GetString("Token") == null)
            {
                return RedirectToAction("Logout", "Account");
            }

            var token = HttpContext.Session.GetString("Token");

            var produit = _produitsService.GetProduitById(int.Parse(id), token);

            return View(produit);
        }

        [HttpGet]
        public IActionResult ListProduits(int ? pageNumber)
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


            var produits = _produitsService.GetProduitByUser(userInfo.Id, token);

            foreach(var produit in produits)
            {
                produit.CategorieName = _categoriesService.GetCategorieById(produit.CategorieId, token).Name;
            }

            PaginatedList<Produits> pageProduit = PaginatedList<Produits>.CreateAsync(produits, pageNumber ?? 1, 5);

            return View(pageProduit);
        }


        [HttpGet]
        public IActionResult Create()
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

            Produits produit = new Produits();

            produit.categories = _categoriesService.GetCategories(token).ToList()
                .ConvertAll(a =>
                {
                    return new SelectListItem()
                    {
                        Text = a.Name,
                        Value = a.Id.ToString(),
                        Selected = false
                    };
                });

            return View(produit);
        }

        [HttpPost]
        public IActionResult Create(Produits model)
        {
            if (!ModelState.IsValid)
            {
                if (HttpContext.Session.GetString("SessionUser") == null || HttpContext.Session.GetString("Token") == null)
                {
                    return RedirectToAction("Logout", "Account");
                }

                var token = HttpContext.Session.GetString("Token");

                model.categories = _categoriesService.GetCategories(token).ToList()
                .ConvertAll(a =>
                {
                    return new SelectListItem()
                    {
                        Text = a.Name,
                        Value = a.Id.ToString(),
                        Selected = false
                    };
                });

                return View(model);
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserSession>(HttpContext.Session.GetString("SessionUser"));
                var token = HttpContext.Session.GetString("Token");

                model.UserId = userInfo.Id;

                _produitsService.AddProduits(model, token);
              
                return RedirectToAction("ListProduits", "Produit");
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

            var produit = _produitsService.GetProduitById(int.Parse(id), token);

            if (produit == null)
            {
                return RedirectToAction("ListProduits", "Produit");
            }

            produit.categories = _categoriesService.GetCategories(token).ToList()
                .ConvertAll(a =>
                {
                    return new SelectListItem()
                    {
                        Text = a.Name,
                        Value = a.Id.ToString(),
                        Selected = (a.Id == produit.CategorieId) ? true : false
                    };
                });

            return View(produit);
        }

        [HttpPost]
        public IActionResult Edit(Produits model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserSession>(HttpContext.Session.GetString("SessionUser"));
                var token = HttpContext.Session.GetString("Token");

                model.UserId = userInfo.Id;

                _produitsService.Update(model.Id, model, token);

                return RedirectToAction("ListProduits", "Produit");
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

            _produitsService.Delete(int.Parse(id), token);

            return RedirectToAction("ListProduits", "Produit");
        }
    }
}