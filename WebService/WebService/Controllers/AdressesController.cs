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
    public class AdressesController : ControllerBase
    {
        private readonly WebServiceContext _context;
        public int pageSize = 5;

        public AdressesController(WebServiceContext context)
        {
            _context = context;
        }

        // GET: api/Adresses
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<AdressesDTO>>> GetAdresses()
        {
            return await _context.Adresses
                .Select(a => new AdressesDTO
                {
                    Id = a.Id,
                    Street = a.Street,
                    PostalCode = a.PostalCode,
                    City = a.City,
                    Country = a.Country,
                    UserName = a.User.UserName
                })
                .ToListAsync();
        }

        [HttpGet("index")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<AdressesDTO>>> GetAdressesByIndex(int pagesIndex = 0)
        {
            return await _context.Adresses
                .Select(a => new AdressesDTO
                {
                    Id = a.Id,
                    Street = a.Street,
                    PostalCode = a.PostalCode,
                    City = a.City,
                    Country = a.Country,
                    UserName = a.User.UserName
                })
                .Skip(pageSize * pagesIndex)
                .Take(pageSize)
                .ToListAsync();
        }

        // GET: api/Adresses/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult<AdressesDTO>> GetAdresses(int id)
        {
            var adresses = await _context.Adresses.FindAsync(id);

            if (adresses == null)
            {
                return NotFound();
            }

            return AdressesToAdressesDTO(adresses);
        }

        [HttpGet("adresseByUser/{id}")]
        [Authorize(Roles = "Admin, User")]
        public  ActionResult<AdressesDTO> GetAdressesByUser(string id)
        {
            var adresses =  _context.Adresses.Where(a => a.UserId == id).FirstOrDefault();

            if (adresses == null)
            {
                return NotFound();
            }

            return AdressesToAdressesDTO(adresses);
        }

        // PUT: api/Adresses/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> PutAdresses(int id, AdressesDTO adressesDTO)
        {
            if (id != adressesDTO.Id)
            {
                return BadRequest();
            }

            var adresse = await _context.Adresses.FindAsync(id);

            if (adresse == null)
            {
                return NotFound();
            }

            adresse.Street = adressesDTO.Street;
            adresse.PostalCode = adressesDTO.PostalCode;
            adresse.City = adressesDTO.City;
            adresse.Country = adressesDTO.Country;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdressesExists(id))
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

        // POST: api/Adresses
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<AdressesDTO>> PostAdresses(Adresses adresses)
        {
            _context.Adresses.Add(adresses);
            await _context.SaveChangesAsync();

            var adressesDTO = AdressesToAdressesDTO(adresses);

            return CreatedAtAction("GetAdresses", new { id = adresses.Id }, adressesDTO);
        }

        // DELETE: api/Adresses/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult> DeleteAdresses(int id)
        {
            var adresses = await _context.Adresses.FindAsync(id);
            if (adresses == null)
            {
                return NotFound();
            }

            _context.Adresses.Remove(adresses);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [Authorize]
        private bool AdressesExists(int id)
        {
            return _context.Adresses.Any(e => e.Id == id);
        }

        [Authorize]
        private AdressesDTO AdressesToAdressesDTO(Adresses adresse)
        {
            _context.Entry(adresse)
                .Reference(a => a.User)
                .Load();

            return new AdressesDTO
            {
                Id = adresse.Id,
                Street = adresse.Street,
                PostalCode = adresse.PostalCode,
                City = adresse.City,
                Country = adresse.Country,
                UserName = adresse.User.UserName
            };
        }
    }
}