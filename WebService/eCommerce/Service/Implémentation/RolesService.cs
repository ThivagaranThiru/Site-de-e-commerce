using eCommerce.Models;
using eCommerce.Service.Interface;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace eCommerce.Service.Implémentation
{
    public class RolesService : IRolesService
    {
        private readonly HttpClient _httpClient = new HttpClient();

        private readonly string _controllerUrl = "/api/Roles";

        public RolesService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Roles AddRoles(Roles role, string token)
        {
            if(role != null)
            {
                string roles = JsonConvert.SerializeObject(role);
                StringContent stringContent = new StringContent(roles, System.Text.Encoding.UTF8, "application/json");
                _httpClient.DefaultRequestHeaders.Add("Cookie", token);

                HttpResponseMessage response = _httpClient.PostAsync(_controllerUrl + "/", stringContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    var register = response.Content.ReadAsStringAsync().Result;

                    var roleResult = JsonConvert.DeserializeObject<Roles>(register);

                    return roleResult;
                }
            }

            return role;
        }

        public void AddRoleToUser(string idRole, string email, string token)
        {
            if(idRole != null && email != null)
            {
                var message = new HttpRequestMessage(HttpMethod.Post, _controllerUrl + "/RoleToUser?nameRole=" + idRole + "&emailUser=" + email);
                _httpClient.DefaultRequestHeaders.Add("Cookie", token);

                var result = _httpClient.SendAsync(message).Result;

                if (result.IsSuccessStatusCode)
                {
                    var resultContent = result.Content.ReadAsStringAsync().Result;
                }
            }
        }

        public void Delete(string id, string token)
        {
            if(id != null)
            {
                _httpClient.DefaultRequestHeaders.Add("Cookie", token);
                HttpResponseMessage response = _httpClient.DeleteAsync(_controllerUrl + "/" + id).Result;
            }
        }

        public IEnumerable<Roles> GetRoles()
        {
            IEnumerable<Roles> roles = null;

            HttpResponseMessage response = _httpClient.GetAsync(_controllerUrl).Result;
            
            if (response.IsSuccessStatusCode)
            {
                var roleString = response.Content.ReadAsStringAsync().Result;
                roles = JsonConvert.DeserializeObject<IEnumerable<Roles>>(roleString);
            }

            return roles;
        }

        public Roles GetRolesById(string id, string token)
        {
            Roles role = null;

            if(id != null)
            {
                _httpClient.DefaultRequestHeaders.Add("Cookie", token);

                HttpResponseMessage response = _httpClient.GetAsync(_controllerUrl + "/" + id).Result;

                if (response.IsSuccessStatusCode)
                {
                    var roleString = response.Content.ReadAsStringAsync().Result;
                    role = JsonConvert.DeserializeObject<Roles>(roleString);
                }
            }

            return role;
        }

        public void Update(string id, Roles role, string token)
        {
            if(id != null && role != null)
            {
                string roles = JsonConvert.SerializeObject(role);
                StringContent stringContent = new StringContent(roles, System.Text.Encoding.UTF8, "application/json");
                _httpClient.DefaultRequestHeaders.Add("Cookie", token);

                HttpResponseMessage response = _httpClient.PutAsync(_controllerUrl + "/" + id, stringContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    var register = response.Content.ReadAsStringAsync().Result;
                    var roleResult = JsonConvert.DeserializeObject<Roles>(register);
                }
            }
        }

        public Roles SelectRole(string token)
        {
            Roles roles = null;

            if(token != null)
            {
                _httpClient.DefaultRequestHeaders.Add("Cookie", token);

                HttpResponseMessage response = _httpClient.GetAsync(_controllerUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    roles = new Roles();

                    var roleString = response.Content.ReadAsStringAsync().Result;
                    roles.roles = JsonConvert.DeserializeObject<IEnumerable<Roles>>(roleString).ToList().ConvertAll(a =>
                    {
                        return new SelectListItem()
                        {
                            Text = a.Name,
                            Value = a.Name,
                            Selected = false
                        };
                    });
                }
            }

            return roles;
        }
    }
}
