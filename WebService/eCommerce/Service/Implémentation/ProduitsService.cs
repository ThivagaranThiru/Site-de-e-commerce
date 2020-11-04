using eCommerce.Models;
using eCommerce.Service.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace eCommerce.Service.Implémentation
{
    public class ProduitsService : IProduitsService
    {
        private readonly HttpClient _httpClient;
        private readonly string _controllerUrl = "/api/Produits";

        public ProduitsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Produits AddProduits(Produits produit, string token)
        {
            if(produit != null)
            {
                string produits = JsonConvert.SerializeObject(produit);
                StringContent stringContent = new StringContent(produits, System.Text.Encoding.UTF8, "application/json");

                _httpClient.DefaultRequestHeaders.Add("Cookie", token);

                HttpResponseMessage response = _httpClient.PostAsync(_controllerUrl + "/", stringContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    var register = response.Content.ReadAsStringAsync().Result;
                    var produitResult = JsonConvert.DeserializeObject<Produits>(register);

                    return produitResult;
                }
            }

            return produit;
        }

        public void Delete(int id, string token)
        {
            if(id > 0)
            {
                _httpClient.DefaultRequestHeaders.Add("Cookie", token);
                HttpResponseMessage response = _httpClient.DeleteAsync(_controllerUrl + "/" + id).Result;
            }
        }

        public Produits GetProduitById(int id, string token)
        {
            Produits produit = null;

            if(id > 0)
            {
                _httpClient.DefaultRequestHeaders.Add("Cookie", token);
                HttpResponseMessage response = _httpClient.GetAsync(_controllerUrl + "/" + id).Result;

                if (response.IsSuccessStatusCode)
                {
                    var produitString = response.Content.ReadAsStringAsync().Result;
                    produit = JsonConvert.DeserializeObject<Produits>(produitString);
                }
            }
        
            return produit;
        }

        public IEnumerable<Produits> GetProduitByUser(string idUser, string token)
        {
            IEnumerable<Produits> produits = null;

            if(idUser != null)
            {
                var message = new HttpRequestMessage(HttpMethod.Get, _controllerUrl + "/ByUser?idUser=" + idUser);

                _httpClient.DefaultRequestHeaders.Add("Cookie", token);

                var result = _httpClient.SendAsync(message).Result;

                if (result.IsSuccessStatusCode)
                {
                    var resultContent = result.Content.ReadAsStringAsync().Result;
                    produits = JsonConvert.DeserializeObject<IEnumerable<Produits>>(resultContent);
                }
            }

            return produits;
        }

        public IEnumerable<Produits> GetProduits(string token)
        {
            IEnumerable<Produits> produits = null;

            _httpClient.DefaultRequestHeaders.Add("Cookie", token);

            HttpResponseMessage response = _httpClient.GetAsync(_controllerUrl).Result;

            if (response.IsSuccessStatusCode)
            {
                var produitString = response.Content.ReadAsStringAsync().Result;
                produits = JsonConvert.DeserializeObject<IEnumerable<Produits>>(produitString);
            }

            return produits;
        }

        public IEnumerable<Produits> GetProduitsByIndex(int pagesIndex, string token)
        {
            throw new NotImplementedException();
        }

        public void Update(int id, Produits produit, string token)
        {
            if(id > 0 && produit != null)
            {
                string produits = JsonConvert.SerializeObject(produit);
                StringContent stringContent = new StringContent(produits, System.Text.Encoding.UTF8, "application/json");

                _httpClient.DefaultRequestHeaders.Add("Cookie", token);

                HttpResponseMessage response = _httpClient.PutAsync(_controllerUrl + "/" + id, stringContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    var register = response.Content.ReadAsStringAsync().Result;

                    var produitResult = JsonConvert.DeserializeObject<Produits>(register);
                }
            }
        }
    }
}