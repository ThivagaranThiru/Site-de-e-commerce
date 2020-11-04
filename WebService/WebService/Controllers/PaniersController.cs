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
    [Authorize(Roles = "Admin,User")]
    public class PaniersController : ControllerBase
    {
        private readonly WebServiceContext _context;
        public int pageSize = 5;

        public PaniersController(WebServiceContext context)
        {
            _context = context;
        }

        // GET: api/Paniers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaniersDTO>>> GetPaniers()
        {
            return await _context.Paniers
                .Select(p => new PaniersDTO
                {
                    Id = p.Id,
                    Quantite = p.Quantite,
                    Status = p.Status,
                    UserEmail = p.User.Email,
                    ProduitId = p.Produit.Id
                })
                .ToListAsync();
        }

        [HttpGet("ByUser")]
        public async Task<ActionResult<IEnumerable<Paniers>>> GetPaniersByUser(string idUser, string status)
        {
            return await _context.Paniers
                .Where(p => p.UserId == idUser && p.Status == status)
                .ToListAsync();
        }

        [HttpGet("ByCommand")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<Paniers>>> GetCommandByUser(string idUser)
        {
            return await _context.Paniers
                .Where(p => p.Produit.UserId == idUser && p.Status == "En Livraison")
                .ToListAsync();
        }

        [HttpGet("index")]
        public async Task<ActionResult<IEnumerable<PaniersDTO>>> GetPaniersByIndex(int pagesIndex = 0)
        {
            return await _context.Paniers
                .Select(p => new PaniersDTO
                {
                    Id = p.Id,
                    Quantite = p.Quantite,
                    Status = p.Status,
                    UserEmail = p.User.Email,
                    ProduitId = p.Produit.Id
                })
                .Skip(pageSize * pagesIndex)
                .Take(pageSize)
                .ToListAsync();
        }

        // GET: api/Paniers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PaniersDTO>> GetPaniers(int id)
        {
            var paniers = await _context.Paniers.FindAsync(id);

            if (paniers == null)
            {
                return NotFound();
            }

            return PanierToPanierDTO(paniers);
        }

        // PUT: api/Paniers/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPaniers(int id, PaniersDTO paniersDTO)
        {
            if (id != paniersDTO.Id)
            {
                return BadRequest();
            }

            var paniers = await _context.Paniers.FindAsync(id);

            if (paniers == null)
            {
                return NotFound();
            }

            paniers.Quantite = paniersDTO.Quantite;
            paniers.Status = paniersDTO.Status;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaniersExists(id))
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

        // POST: api/Paniers
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<PaniersDTO>> PostPaniers(Paniers paniers)
        {
            _context.Paniers.Add(paniers);
            await _context.SaveChangesAsync();

            var panierDTO = PanierToPanierDTO(paniers);

            return CreatedAtAction("GetPaniers", new { id = paniers.Id }, panierDTO);
        }

        // DELETE: api/Paniers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePaniers(int id)
        {
            var paniers = await _context.Paniers.FindAsync(id);
            if (paniers == null)
            {
                return NotFound();
            }

            _context.Paniers.Remove(paniers);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PaniersExists(int id)
        {
            return _context.Paniers.Any(e => e.Id == id);
        }

        private PaniersDTO PanierToPanierDTO(Paniers panier)
        {
            _context.Entry(panier)
               .Reference(p => p.User)
               .Load();

            _context.Entry(panier)
               .Reference(p => p.Produit)
               .Load();

            return new PaniersDTO
            {
                Id = panier.Id,
                Quantite = panier.Quantite,
                Status = panier.Status,
                UserEmail = panier.User.Email,
                ProduitId = panier.Produit.Id
            };
        }
    }
}
