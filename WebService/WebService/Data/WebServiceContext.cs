using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebService.Models;

namespace WebService.Data
{
    public class WebServiceContext : IdentityDbContext<Users>
    {
        public WebServiceContext (DbContextOptions<WebServiceContext> options)
            : base(options)
        {
        }

        public DbSet<WebService.Models.Adresses> Adresses { get; set; }

        public DbSet<WebService.Models.Categories> Categories { get; set; }

        public DbSet<WebService.Models.Commandes> Commandes { get; set; }

        public DbSet<WebService.Models.Paniers> Paniers { get; set; }

        public DbSet<WebService.Models.Produits> Produits { get; set; }
    }
}