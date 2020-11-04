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
    public class AdressesService : IAdressesService
    {
        private readonly HttpClient _httpClient;
        private readonly string _controllerUrl = "/api/Adresses";
        
        public AdressesService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Adresses AddAdresses(Adresses adresse, string token)
        {
            if(adresse != null)
            {
                string adresses = JsonConvert.SerializeObject(adresse);
                StringContent stringContent = new StringContent(adresses, System.Text.Encoding.UTF8, "application/json");
                _httpClient.DefaultRequestHeaders.Add("Cookie", token);

                HttpResponseMessage response = _httpClient.PostAsync(_controllerUrl + "/", stringContent).Result;

                if(response.IsSuccessStatusCode)
                {
                    var register = response.Content.ReadAsStringAsync().Result;

                    var adresseResult = JsonConvert.DeserializeObject<Adresses>(register);

                    return adresseResult;
                }
            }

            return adresse;
        }

        public void Delete(int id, string token)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Adresses> GetAdresses(string token)
        {
            throw new NotImplementedException();
        }

        public Adresses GetAdressesById(int id, string token)
        {
            Adresses adresse = null;

            if (id > 0)
            {
                _httpClient.DefaultRequestHeaders.Add("Cookie", token);

                HttpResponseMessage response = _httpClient.GetAsync(_controllerUrl + "/" + id).Result;

                if (response.IsSuccessStatusCode)
                {
                    var adresseString = response.Content.ReadAsStringAsync().Result;
                    adresse = JsonConvert.DeserializeObject<Adresses>(adresseString);
                }
            }

            return adresse;
        }

        public void Update(int id, Adresses adresse, string token)
        {
            if(id > 0 && adresse != null)
            {
                string adresses = JsonConvert.SerializeObject(adresse);
                StringContent stringContent = new StringContent(adresses, System.Text.Encoding.UTF8, "application/json");
                _httpClient.DefaultRequestHeaders.Add("Cookie", token);

                HttpResponseMessage response = _httpClient.PutAsync(_controllerUrl + "/" + id, stringContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    var register = response.Content.ReadAsStringAsync().Result;

                    var adresseResult = JsonConvert.DeserializeObject<Adresses>(register);
                }
            }
        }

       public Adresses GetAdressesByUser(string id, string token)
        {
            Adresses adresse = null;
            _httpClient.DefaultRequestHeaders.Add("Cookie", token);

            HttpResponseMessage response = _httpClient.GetAsync(_controllerUrl + "/adresseByUser/" + id).Result;

            if (response.IsSuccessStatusCode)
            {
                var adresseString = response.Content.ReadAsStringAsync().Result;
                adresse = JsonConvert.DeserializeObject<Adresses>(adresseString);
            }

            return adresse;
        }
    }
}
