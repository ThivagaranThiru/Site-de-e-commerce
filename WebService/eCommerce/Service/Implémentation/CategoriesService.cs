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
    public class CategoriesService : ICategoriesService
    {
        private readonly HttpClient _httpClient;
        private readonly string _controllerUrl = "/api/Categories";

        public CategoriesService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Categories AddCategories(Categories categorie, string token)
        {
            if(categorie != null) 
            {
                string categories = JsonConvert.SerializeObject(categorie);
                StringContent stringContent = new StringContent(categories, System.Text.Encoding.UTF8, "application/json");
                _httpClient.DefaultRequestHeaders.Add("Cookie", token);

                HttpResponseMessage response = _httpClient.PostAsync(_controllerUrl + "/", stringContent).Result;

                if(response.IsSuccessStatusCode)
                {
                    var register = response.Content.ReadAsStringAsync().Result;

                    var categorieResult = JsonConvert.DeserializeObject<Categories>(register);

                    return categorieResult;
                }
            }

            return categorie;
        }

        public void Delete(int id, string token)
        {
            if(id > 0)
            {
                _httpClient.DefaultRequestHeaders.Add("Cookie", token);
                HttpResponseMessage response = _httpClient.DeleteAsync(_controllerUrl + "/" + id).Result;
            }
        }

        public Categories GetCategorieById(int id, string token)
        {
            Categories categorie = null;

            if(id > 0)
            {
                _httpClient.DefaultRequestHeaders.Add("Cookie", token);
                HttpResponseMessage response = _httpClient.GetAsync(_controllerUrl + "/" + id).Result;

                if (response.IsSuccessStatusCode)
                {
                    var roleString = response.Content.ReadAsStringAsync().Result;

                    categorie = JsonConvert.DeserializeObject<Categories>(roleString);
                }
            }

            return categorie;
        }

        public IEnumerable<Categories> GetCategories(string token)
        {
            IEnumerable<Categories> categories = null;
            _httpClient.DefaultRequestHeaders.Add("Cookie", token);

            HttpResponseMessage response = _httpClient.GetAsync(_controllerUrl).Result;

            if (response.IsSuccessStatusCode)
            {
                var categorieString = response.Content.ReadAsStringAsync().Result;
                categories = JsonConvert.DeserializeObject<IEnumerable<Categories>>(categorieString);
            }
            
            return categories;
        }

        public void Update(int id, Categories categorie, string token)
        {
            if(id > 0 && categorie != null)
            {
                string categories = JsonConvert.SerializeObject(categorie);
                StringContent stringContent = new StringContent(categories, System.Text.Encoding.UTF8, "application/json");
                _httpClient.DefaultRequestHeaders.Add("Cookie", token);

                HttpResponseMessage response = _httpClient.PutAsync(_controllerUrl + "/" + id, stringContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    var register = response.Content.ReadAsStringAsync().Result;
                    var adresseResult = JsonConvert.DeserializeObject<Categories>(register);
                }
            }
        }
    }
}
