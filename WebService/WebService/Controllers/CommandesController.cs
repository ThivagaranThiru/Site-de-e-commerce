using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class CommandesController : ControllerBase
    {
        private readonly WebServiceContext _context;
        public int pageSize = 5;

        public CommandesController(WebServiceContext context)
        {
            _context = context;
        }

        // GET: api/Commandes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommandesDTO>>> GetCommandes()
        {
            return await _context.Commandes
                .Select(c => new CommandesDTO 
                {
                    Id = c.Id,
                    Livraison = c.Livraison,
                    Payment = c.Payment,
                    TotalQt = c.TotalQt,
                    TotalPrix = c.TotalPrix,
                    Panier = c.Panier
                })
                .ToListAsync();
        }

        [HttpGet("index")]
        public async Task<ActionResult<IEnumerable<CommandesDTO>>> GetCommandesByIndex(int pagesIndex = 0)
        {
            return await _context.Commandes
                .Select(c => new CommandesDTO
                {
                    Id = c.Id,
                    Livraison = c.Livraison,
                    Payment = c.Payment,
                    TotalQt = c.TotalQt,
                    TotalPrix = c.TotalPrix,
                    Panier = c.Panier
                })
                .Skip(pageSize * pagesIndex)
                .Take(pageSize)
                .ToListAsync();
        }
        // GET: api/Commandes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CommandesDTO>> GetCommandes(int id)
        {
            var commandes = await _context.Commandes.FindAsync(id);

            if (commandes == null)
            {
                return NotFound();
            }

            return CommandeToCommandeDTO(commandes);
        }

        // PUT: api/Commandes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCommandes(int id, CommandesDTO commandesDTO)
        {
            if (id != commandesDTO.Id)
            {
                return BadRequest();
            }

            var commandes = await _context.Commandes.FindAsync(id);

            if (commandes == null)
            {
                return NotFound();
            }

            commandes.Livraison = commandesDTO.Livraison;
            commandes.Payment = commandesDTO.Payment;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommandesExists(id))
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

        // POST: api/Commandes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<CommandesDTO>> PostCommandes(Commandes commandes)
        {
            //_context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[commandes] ON");
            _context.Commandes.Add(commandes);
            await _context.SaveChangesAsync();
            //_context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[commandes] OFF");

            var commandesDTO = CommandeToCommandeDTO(commandes);

            return CreatedAtAction("GetCommandes", new { id = commandes.Id }, commandesDTO);
        }

        // DELETE: api/Commandes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCommandes(int id)
        {
            var commandes = await _context.Commandes.FindAsync(id);
            if (commandes == null)
            {
                return NotFound();
            }

            _context.Commandes.Remove(commandes);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CommandesExists(int id)
        {
            return _context.Commandes.Any(e => e.Id == id);
        }

        private CommandesDTO CommandeToCommandeDTO(Commandes commandes)
        {
            _context.Entry(commandes)
                .Reference(c => c.Panier)
                .Load();

            return new CommandesDTO
            {
                Id = commandes.Id,
                Livraison = commandes.Livraison,
                Payment = commandes.Payment,
                TotalQt = commandes.TotalQt,
                TotalPrix = commandes.TotalPrix,
                Panier = commandes.Panier
            };
        }
    }
}