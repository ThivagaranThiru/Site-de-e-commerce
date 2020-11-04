using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebService.Models;
using WebService.ModelsDTO;

namespace WebService.Controllers
{
    [Route("api/Roles")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<Users> _userManager;
        private readonly SignInManager<Users> _signInManager;

        public int pageSize = 5;

        public RoleController(UserManager<Users> userManager, RoleManager<IdentityRole> roleManager, SignInManager<Users> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: api/Role
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<RoleDTO>>> GetRoles()
        {
            return Ok(await _roleManager.Roles
                .Select(r => new RoleDTO
                {
                    Id = r.Id,
                    Name = r.Name
                })
                .ToListAsync());
        }

        [HttpGet("index")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<RoleDTO>>> GetRolesByIndex(int pagesIndex = 0)
        {
            return await _roleManager.Roles
                .Select(r => new RoleDTO
                {
                    Id = r.Id,
                    Name = r.Name
                })
                .Skip(pageSize * pagesIndex)
                .Take(pageSize)
                .ToListAsync();
        }

        // GET: api/Role/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<RoleDTO>> GetRoles(string id)
        {
            var roles = await _roleManager.FindByIdAsync(id);

            if (roles == null)
            {
                return NotFound();
            }

            return RoleToRoleDTO(roles);
        }

        // POST: api/Role
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<RoleDTO>> PostRole(RoleDTO roleDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var role = new IdentityRole()
            {
                Name = roleDTO.Name
            };

            IdentityResult result = await _roleManager.CreateAsync(role);

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }

                if (ModelState.IsValid)
                {
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            var roles = RoleToRoleDTO(role);

            return CreatedAtAction("GetRoles", new { id = role.Id }, roles);
        }

        // PUT: api/Role/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutRole(string id, RoleDTO roleDTO)
        {
            if(id != roleDTO.Id)
            {
                return BadRequest();
            }

            var role = await _roleManager.FindByIdAsync(id);

            if(role == null)
            {
                return NotFound();
            }

            role.Name = roleDTO.Name;

            var result = await _roleManager.UpdateAsync(role);

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }

                if (ModelState.IsValid)
                {
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return Ok();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            var result = await _roleManager.DeleteAsync(role);

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }

                if (ModelState.IsValid)
                {
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return NoContent();
        }

        [HttpPost("RoleToUser")]
        [Authorize]
        public async Task<ActionResult> AddRoleToUser(string nameRole, string emailUser)
        {

            var role = await _roleManager.FindByNameAsync(nameRole);

            if (role == null)
            {
                return NotFound("Role Not Found");
            }
           
            var users = await _userManager.FindByEmailAsync(emailUser);

            if (users == null)
            {
                return NotFound("User Not Found");
            }

            IdentityResult result = await _userManager.AddToRoleAsync(users, role.Name);

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }

                if (ModelState.IsValid)
                {
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            await _signInManager.RefreshSignInAsync(users);

            return Ok();
        }

        [Authorize]
        private RoleDTO RoleToRoleDTO (IdentityRole role)
        {
            return new RoleDTO
            {
                Id = role.Id,
                Name = role.Name
            };
        }
    }
}
