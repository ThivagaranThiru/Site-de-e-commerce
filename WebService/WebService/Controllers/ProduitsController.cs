using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebService.Data;
using WebService.Models;
using WebService.ModelsDTO;

namespace WebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProduitsController : ControllerBase
    {
        private readonly WebServiceContext _context;
        public int pageSize = 5;

        public ProduitsController(WebServiceContext context)
        {
            _context = context;
        }

        // GET: api/Produits
        [HttpGet]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<IEnumerable<ProduitsDTO>>> GetProduits()
        {
            return await _context.Produits
                .Select(p => new ProduitsDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Prix = p.Prix,
                    Stock = p.Stock,
                    Description = p.Description,
                    ImageUrl = p.ImageUrl,
                    UserName = p.User.UserName,
                    CategorieName = p.Categorie.Name
                })
                .ToListAsync();
        }

        
        [HttpGet("index")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<IEnumerable<ProduitsDTO>>> GetProduitsByIndex(int pagesIndex = 0)
        {
            return await _context.Produits
                .Select(p => new ProduitsDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Prix = p.Prix,
                    Stock = p.Stock,
                    Description = p.Description,
                    ImageUrl = p.ImageUrl,
                    UserName = p.User.UserName,
                    CategorieName = p.Categorie.Name
                })
                .Skip(pageSize * pagesIndex)
                .Take(pageSize)
                .ToListAsync();
        }

        // GET: api/Produits/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<ProduitsDTO>> GetProduits(int id)
        {
            var produits = await _context.Produits.FindAsync(id);

            if (produits == null)
            {
                return NotFound();
            }

            return ProduitToProduitDTO(produits);
        }

        [HttpGet("ByUser")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<Produits>>> GetProduitsByUser(string idUser)
        {
            return await _context.Produits
                .Where(p => p.UserId == idUser)
                .ToListAsync();
        }

        // PUT: api/Produits/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> PutProduits(int id, Produits produitsDTO)
        {
            if (id != produitsDTO.Id)
            {
                return BadRequest();
            }

            var produits = await _context.Produits.FindAsync(id);

            if (produits == null)
            {
                return NotFound();
            }

            produits.Name = produitsDTO.Name;
            produits.Prix = produitsDTO.Prix;
            produits.Stock = produitsDTO.Stock;
            produits.Description = produitsDTO.Description;
            produits.ImageUrl = produitsDTO.ImageUrl;
            produits.CategorieId = produitsDTO.CategorieId;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProduitsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Produits
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ProduitsDTO>> PostProduits(Produits produits)
        {
            _context.Produits.Add(produits);
            await _context.SaveChangesAsync();

            var produitDTO = ProduitToProduitDTO(produits);

            return CreatedAtAction("GetProduits", new { id = produits.Id }, produitDTO);
        }

        // DELETE: api/Produits/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteProduits(int id)
        {
            var produits = await _context.Produits.FindAsync(id);
            if (produits == null)
            {
                return NotFound();
            }

            _context.Produits.Remove(produits);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [Authorize]
        private bool ProduitsExists(int id)
        {
            return _context.Produits.Any(e => e.Id == id);
        }

        [Authorize]
        private ProduitsDTO ProduitToProduitDTO(Produits produit)
        {
            _context.Entry(produit)
                .Reference(p => p.User)
                .Load();

            _context.Entry(produit)
                .Reference(p => p.Categorie)
                .Load();

            return new ProduitsDTO
            {
                Id = produit.Id,
                Name = produit.Name,
                Prix = produit.Prix,
                Stock = produit.Stock,
                Description = produit.Description,
                ImageUrl = produit.ImageUrl,
                CategorieId = produit.CategorieId,
                UserName = produit.User.UserName,
                CategorieName = produit.Categorie.Name
            };
        }
    }
}