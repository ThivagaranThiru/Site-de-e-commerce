using eCommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.Service.Interface
{
    public interface ICategoriesService
    {
        IEnumerable<Categories> GetCategories(string token);
        Categories AddCategories(Categories categorie, string token);
        Categories GetCategorieById(int id, string token);
        void Update(int id, Categories categorie, string token);
        void Delete(int id, string token);
    }
}
