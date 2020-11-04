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
    public class CategoriesController : ControllerBase
    {
        private readonly WebServiceContext _context;
        public int pageSize = 5;

        public CategoriesController(WebServiceContext context)
        {
            _context = context;
        }

        // GET: api/Categories
        [HttpGet]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<IEnumerable<CategoriesDTO>>> GetCategories()
        {
            return await _context.Categories
                .Select(c => new CategoriesDTO 
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description
                })
                .ToListAsync();
        }

        [HttpGet("index")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<IEnumerable<CategoriesDTO>>> GetCategoriesByIndex(int pagesIndex = 0)
        {
            return await _context.Categories
                .Select(c => new CategoriesDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description
                })
                .Skip(pageSize * pagesIndex)
                .Take(pageSize)
                .ToListAsync();
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CategoriesDTO>> GetCategories(int id)
        {
            var categories = await _context.Categories.FindAsync(id);

            if (categories == null)
            {
                return NotFound();
            }

            return CategorieToCategorieDTO(categories);
        }

        // PUT: api/Categories/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutCategories(int id, CategoriesDTO categoriesDTO)
        {
            if (id != categoriesDTO.Id)
            {
                return BadRequest();
            }

            var categories = await _context.Categories.FindAsync(id);

            if (categories == null)
            {
                return NotFound();
            }

            categories.Name = categoriesDTO.Name;
            categories.Description = categoriesDTO.Description;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoriesExists(id))
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

        // POST: api/Categories
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CategoriesDTO>> PostCategories(Categories categories)
        {
            _context.Categories.Add(categories);
            await _context.SaveChangesAsync();

            var categorieDTO = CategorieToCategorieDTO(categories);

            return CreatedAtAction("GetCategories", new { id = categories.Id }, categorieDTO);
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteCategories(int id)
        {
            var categories = await _context.Categories.FindAsync(id);
            if (categories == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(categories);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [Authorize]
        private bool CategoriesExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }

        [Authorize]
        private CategoriesDTO CategorieToCategorieDTO(Categories categorie)
        {
            return new CategoriesDTO
            {
                Id = categorie.Id,
                Name = categorie.Name,
                Description = categorie.Description
            };
        }
    }
}