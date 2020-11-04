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
    public class PaniersService : IPaniersService
    {
        private readonly HttpClient _httpClient;
        private readonly string _controllerUrl = "/api/Paniers";

        public PaniersService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Paniers AddPaniers(Paniers panier, string token)
        {
            if(panier != null)
            {
                string paniers = JsonConvert.SerializeObject(panier);
                StringContent stringContent = new StringContent(paniers, System.Text.Encoding.UTF8, "application/json");

                _httpClient.DefaultRequestHeaders.Add("Cookie", token);

                HttpResponseMessage response = _httpClient.PostAsync(_controllerUrl + "/", stringContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    var register = response.Content.ReadAsStringAsync().Result;
                    var panierResult = JsonConvert.DeserializeObject<Paniers>(register);

                    return panierResult;
                }
            }

            return panier;
        }

        public void Delete(int id, string token)
        {
            if(id > 0)
            {
                _httpClient.DefaultRequestHeaders.Add("Cookie", token);
                HttpResponseMessage response = _httpClient.DeleteAsync(_controllerUrl + "/" + id).Result;
            }
        }

        public Paniers GetPanierById(int id, string token)
        {
            Paniers panier = null;

            if(id > 0)
            {
                _httpClient.DefaultRequestHeaders.Add("Cookie", token);

                HttpResponseMessage response = _httpClient.GetAsync(_controllerUrl + "/" + id).Result;

                if (response.IsSuccessStatusCode)
                {
                    var panierString = response.Content.ReadAsStringAsync().Result;
                    panier = JsonConvert.DeserializeObject<Paniers>(panierString);
                }
            }

            return panier;
        }

        public IEnumerable<Paniers> GetPaniers(string token)
        {
            throw new NotImplementedException();
        }

        public void Update(int id, Paniers panier, string token)
        {
            if(id > 0 && panier != null)
            {
                string paniers = JsonConvert.SerializeObject(panier);
                StringContent stringContent = new StringContent(paniers, System.Text.Encoding.UTF8, "application/json");

                _httpClient.DefaultRequestHeaders.Add("Cookie", token);

                HttpResponseMessage response = _httpClient.PutAsync(_controllerUrl + "/" + id, stringContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    var register = response.Content.ReadAsStringAsync().Result;
                    var adresseResult = JsonConvert.DeserializeObject<Paniers>(register);
                }
            }
        }

        public IEnumerable<Paniers> GetPaniersByUser(string idUser, string status, string token)
        {
            IEnumerable<Paniers> paniers = null;

            if(idUser != null && status != null)
            {
                var message = new HttpRequestMessage(HttpMethod.Get, _controllerUrl + "/ByUser?idUser=" + idUser + "&status=" + status);

                _httpClient.DefaultRequestHeaders.Add("Cookie", token);

                var result = _httpClient.SendAsync(message).Result;

                if (result.IsSuccessStatusCode)
                {
                    var resultContent = result.Content.ReadAsStringAsync().Result;
                    paniers = JsonConvert.DeserializeObject<IEnumerable<Paniers>>(resultContent);
                }
            }

            return paniers;
        }

        IEnumerable<Paniers> IPaniersService.GetCommandsByUser(string idUser, string token)
        {
            IEnumerable<Paniers> paniers = null;

            if(idUser != null)
            {
                var message = new HttpRequestMessage(HttpMethod.Get, _controllerUrl + "/ByCommand?idUser=" + idUser);

                _httpClient.DefaultRequestHeaders.Add("Cookie", token);

                var result = _httpClient.SendAsync(message).Result;

                if (result.IsSuccessStatusCode)
                {
                    var resultContent = result.Content.ReadAsStringAsync().Result;
                    paniers = JsonConvert.DeserializeObject<IEnumerable<Paniers>>(resultContent);
                }
            }

            return paniers;
        }
    }
}
