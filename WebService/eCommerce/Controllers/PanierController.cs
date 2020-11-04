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
    public class PanierController : Controller
    {
        private readonly ILogger<PanierController> _logger;
        private readonly IPaniersService _panierService;
        private readonly IProduitsService _produitService;
        private readonly IAdressesService _adresseService;
        private readonly IUsersService _userService;

        public PanierController(ILogger<PanierController> logger, IPaniersService panierService, IProduitsService produitService,
            IAdressesService adresseService, IUsersService userService)
        {
            _logger = logger;
            _panierService = panierService;
            _produitService = produitService;
            _adresseService = adresseService;
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Index(string quantite, string id)
        {
            if (HttpContext.Session.GetString("SessionUser") == null || HttpContext.Session.GetString("Token") == null)
            {
                return RedirectToAction("Logout", "Account");
            }

            var userInfo = JsonConvert.DeserializeObject<UserSession>(HttpContext.Session.GetString("SessionUser"));

            var token = HttpContext.Session.GetString("Token");

            PaniersTotal panierTotal = new PaniersTotal();

            if (quantite != null && id != null)
            {
                Paniers panier = new Paniers
                {
                    Quantite = quantite,
                    Status = "En Cours",
                    ProduitId = int.Parse(id),
                    UserId = userInfo.Id
                };

                _panierService.AddPaniers(panier, token);
            }

            IEnumerable<Paniers> paniers = _panierService.GetPaniersByUser(userInfo.Id, "En Cours", token);

            foreach (var pani in paniers)
            {
                pani.Produit = _produitService.GetProduitById(pani.ProduitId, token);
                pani.totalPrix = int.Parse(pani.Quantite) * int.Parse(pani.Produit.Prix);
                panierTotal.totalPaniers = pani.totalPrix + panierTotal.totalPaniers;
            };

            panierTotal.paniers = paniers;

            return View(panierTotal);
        }

        [HttpPost]
        public IActionResult UpdateCart(string idPanier, string quantite)
        {
            var token = HttpContext.Session.GetString("Token");

            var panier = _panierService.GetPanierById(int.Parse(idPanier), token);

            if (quantite != "0")
            {
                panier.Quantite = quantite;

                _panierService.Update(panier.Id, panier, token);
            }
            else
            {
                _panierService.Delete(panier.Id, token);
            }

            return RedirectToAction("Index", "Panier");
        }

        public IActionResult Checkout()
        {
            if (HttpContext.Session.GetString("SessionUser") == null || HttpContext.Session.GetString("Token") == null)
            {
                return RedirectToAction("Logout", "Account");
            }

            var userInfo = JsonConvert.DeserializeObject<UserSession>(HttpContext.Session.GetString("SessionUser"));

            var token = HttpContext.Session.GetString("Token");

            PaniersTotal panierTotal = new PaniersTotal();

            IEnumerable<Paniers> paniers = _panierService.GetPaniersByUser(userInfo.Id, "En Cours", token);

            foreach (var pani in paniers)
            {
                pani.Produit = _produitService.GetProduitById(pani.ProduitId, token);
                pani.totalPrix = int.Parse(pani.Quantite) * int.Parse(pani.Produit.Prix);
                panierTotal.totalPaniers = pani.totalPrix + panierTotal.totalPaniers;
                panierTotal.totalQuantite = panierTotal.totalQuantite + int.Parse(pani.Quantite);
            };

            panierTotal.paniers = paniers;
            panierTotal.user = _userService.GetUserById(userInfo.Id, token);
            if(userInfo.AdresseId <= 0)
            {
                return RedirectToAction("AdresseRegister", "Account");
            }
            panierTotal.adresse = _adresseService.GetAdressesById(userInfo.AdresseId, token);

            return View(panierTotal);
        }
        public IActionResult Confirmation()
        {
            if (HttpContext.Session.GetString("SessionUser") == null || HttpContext.Session.GetString("Token") == null)
            {
                return RedirectToAction("Logout", "Account");
            }

            var userInfo = JsonConvert.DeserializeObject<UserSession>(HttpContext.Session.GetString("SessionUser"));

            var token = HttpContext.Session.GetString("Token");

            PaniersTotal panierTotal = new PaniersTotal();

            IEnumerable<Paniers> paniers = _panierService.GetPaniersByUser(userInfo.Id, "En Cours", token);

            foreach (var pani in paniers)
            {
                pani.Produit = _produitService.GetProduitById(pani.ProduitId, token);
                pani.totalPrix = int.Parse(pani.Quantite) * int.Parse(pani.Produit.Prix);
                panierTotal.totalPaniers = pani.totalPrix + panierTotal.totalPaniers;
                panierTotal.totalQuantite = panierTotal.totalQuantite + int.Parse(pani.Quantite);

                pani.Produit.Stock = (int.Parse(pani.Produit.Stock) - int.Parse(pani.Quantite)).ToString();
                pani.Status = "En Livraison";

                _produitService.Update(pani.Produit.Id, pani.Produit, token);
                _panierService.Update(pani.Id, pani, token);
            };

            panierTotal.paniers = paniers;
            panierTotal.user = _userService.GetUserById(userInfo.Id, token);
            
            if(userInfo.AdresseId <= 0)
            {
                return RedirectToAction("AdresseRegister", "Account");
            }

            panierTotal.adresse = _adresseService.GetAdressesById(userInfo.AdresseId, token);

            return View(panierTotal);
        }

        public IActionResult MyCommand()
        {
            if (HttpContext.Session.GetString("SessionUser") == null || HttpContext.Session.GetString("Token") == null)
            {
                return RedirectToAction("Logout", "Account");
            }

            var userInfo = JsonConvert.DeserializeObject<UserSession>(HttpContext.Session.GetString("SessionUser"));
            var token = HttpContext.Session.GetString("Token");

            IEnumerable<Paniers> paniers = _panierService.GetPaniersByUser(userInfo.Id, "En Livraison", token);
            
            PaniersTotal panierTotal = new PaniersTotal();
            
            foreach (var pani in paniers)
            {
                pani.Produit = _produitService.GetProduitById(pani.ProduitId, token);
                pani.totalPrix = int.Parse(pani.Quantite) * int.Parse(pani.Produit.Prix);
                panierTotal.totalPaniers = pani.totalPrix + panierTotal.totalPaniers;
            };

            panierTotal.paniers = paniers;

            return View(panierTotal);
        }

        public IActionResult MyCommandByUser()
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

            IEnumerable<Paniers> paniers = _panierService.GetCommandsByUser(userInfo.Id, token);

            PaniersTotal panierTotal = new PaniersTotal();

            foreach (var pani in paniers)
            {
                pani.Produit = _produitService.GetProduitById(pani.ProduitId, token);
                pani.User = _userService.GetUserById(pani.UserId, token);
                pani.totalPrix = int.Parse(pani.Quantite) * int.Parse(pani.Produit.Prix);
                panierTotal.totalPaniers = pani.totalPrix + panierTotal.totalPaniers;
            };

            panierTotal.paniers = paniers;

            return View(panierTotal);
        }
    }
}