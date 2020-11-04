using eCommerce.Models;
using eCommerce.Service.Interface;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace eCommerce.Service.Implémentation
{
    public class UsersService : IUsersService
    {
        private readonly HttpClient _httpClient;
        private readonly string _controllerUrl = "/api/Users";

        public UsersService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Users AddUsers(Users user, string token)
        {
            throw new NotImplementedException();
        }

        public void ChangePassword(Users user, string token)
        {
            throw new NotImplementedException();
        }

        public void Delete(string id, string token)
        {
            if(id != null)
            {
                _httpClient.DefaultRequestHeaders.Add("Cookie", token);
                HttpResponseMessage response = _httpClient.DeleteAsync(_controllerUrl + "/" + id).Result;
            }
        }

        public Users GetUserById(string id, string token)
        {
            Users user = null;

            if(id != null)
            {
                _httpClient.DefaultRequestHeaders.Add("Cookie", token);
                HttpResponseMessage response = _httpClient.GetAsync(_controllerUrl + "/" + id).Result;

                if (response.IsSuccessStatusCode)
                {
                    var adresseString = response.Content.ReadAsStringAsync().Result;
                    user = JsonConvert.DeserializeObject<Users>(adresseString);
                }
            }

            return user;
        }

        public IEnumerable<Users> GetUsers(string token)
        {
            throw new NotImplementedException();
        }

        public Users Login(Login user)
        {
            Users userResult = null;

            if (user != null)
            {
                string users = JsonConvert.SerializeObject(user);

                StringContent stringContent = new StringContent(users, System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = _httpClient.PostAsync(_controllerUrl + "/login", stringContent).Result;

                if(response.IsSuccessStatusCode)
                {
                    var register = response.Content.ReadAsStringAsync().Result;

                    userResult = JsonConvert.DeserializeObject<Users>(register);

                    IEnumerable<string> values;

                    if (response.Headers.TryGetValues("Set-Cookie", out values))
                    {
                        userResult.token = values.First();
                    }
                }
            }
          
            return userResult;
        }

        public void Logout(string token)
        {
            _httpClient.DefaultRequestHeaders.Add("Cookie", token);

            HttpResponseMessage response = _httpClient.PostAsync(_controllerUrl + "/logout", null).Result;

            if (response.IsSuccessStatusCode)
            {
                var register = response.Content.ReadAsStringAsync().Result;
            }
        }

        public Users Register(Users user)
        {
            Users userResult = null;

            if(user != null)
            {
                string users = JsonConvert.SerializeObject(user);

                StringContent stringContent = new StringContent(users, System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = _httpClient.PostAsync(_controllerUrl + "/register", stringContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    IEnumerable<string> values;

                    var register = response.Content.ReadAsStringAsync().Result;

                    userResult = JsonConvert.DeserializeObject<Users>(register);

                    if (response.Headers.TryGetValues("Set-Cookie", out values))
                    {
                        userResult.token = values.First();
                    }
                }
            }

            return userResult;
        }

        public void Update(string id, Users user, string token)
        {
            if(id != null && user != null)
            {
                string users = JsonConvert.SerializeObject(user);
                StringContent stringContent = new StringContent(users, System.Text.Encoding.UTF8, "application/json");

                _httpClient.DefaultRequestHeaders.Add("Cookie", token);

                HttpResponseMessage response = _httpClient.PutAsync(_controllerUrl + "/" + id, stringContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    var register = response.Content.ReadAsStringAsync().Result;
                    var adresseResult = JsonConvert.DeserializeObject<Users>(register);
                }
            }
        }
    }
}
