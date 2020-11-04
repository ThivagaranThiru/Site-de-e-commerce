using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebService.Data;
using WebService.Models;
using WebService.ModelsDTO;

namespace WebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<Users> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<Users> _signInManager;

        public int pageSize = 5;

        public UsersController(UserManager<Users> userManager, RoleManager<IdentityRole> roleManager, SignInManager <Users> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        // GET: api/Users
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UsersDTO>>> GetUsers()
        {
            return await _userManager.Users
                .Select(u => new UsersDTO
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Password = u.PasswordHash,
                    Email = u.Email,
                    Civilite = u.Civilite,
                    FirstName = u.FirstName,
                    Name = u.Name,
                    TelNumber = u.PhoneNumber,
                    DateOfBirth = u.DateOfBirth,
                })
                .ToListAsync();
        }

        [HttpGet("index")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UsersDTO>>> GetUsersByIndex(int pagesIndex = 0)
        {
            return await _userManager.Users
                .Select(u => new UsersDTO
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Password = u.PasswordHash,
                    Email = u.Email,
                    Civilite = u.Civilite,
                    FirstName = u.FirstName,
                    Name = u.Name,
                    TelNumber = u.PhoneNumber,
                    DateOfBirth = u.DateOfBirth,
                })
                .Skip(pageSize * pagesIndex)
                .Take(pageSize)
                .ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult<UsersDTO>> GetUsers(string id)
        {
            var users = await _userManager.FindByIdAsync(id);

            if (users == null)
            {
                return NotFound();
            }

            return UserToUserDTO(users);
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> PutUsers(string id, UsersDTO usersDTO)
        {
            if (id != usersDTO.Id)
            {
                return BadRequest();
            }

            var user = await _userManager.FindByIdAsync(id);

            if(user == null)
            {
                return NotFound();
            }

            user.UserName = usersDTO.UserName;
            user.Email = usersDTO.Email;
            user.Civilite = usersDTO.Civilite;
            user.FirstName = usersDTO.FirstName;
            user.Name = usersDTO.Name;
            user.PhoneNumber = usersDTO.TelNumber;
            user.DateOfBirth = usersDTO.DateOfBirth;

            var result = await _userManager.UpdateAsync(user);

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
            }

                return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UsersDTO>> PostUsers(UsersDTO usersDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var users = new Users()
            {
                UserName = usersDTO.UserName,
                Email = usersDTO.Email,
                Civilite = usersDTO.Civilite,
                FirstName = usersDTO.FirstName,
                Name = usersDTO.Name,
                PhoneNumber = usersDTO.TelNumber,
                DateOfBirth = usersDTO.DateOfBirth
            };

            var result = await _userManager.CreateAsync(users, usersDTO.Password);

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

            var user = UserToUserDTO(users);

            return CreatedAtAction("GetUsers", new { id = users.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult> DeleteUsers(string id)
        {
            var users = await _userManager.FindByIdAsync(id);
          
            if (users == null)
            {
                return NotFound();
            }

           var result = await _userManager.DeleteAsync(users);

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

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<UsersDTO>> Register(UsersDTO usersDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var users = new Users()
            {
                UserName = usersDTO.UserName,
                Email = usersDTO.Email,
                Civilite = usersDTO.Civilite,
                FirstName = usersDTO.FirstName,
                Name = usersDTO.Name,
                PhoneNumber = usersDTO.TelNumber,
                DateOfBirth = usersDTO.DateOfBirth
            };

            var result = await _userManager.CreateAsync(users, usersDTO.Password);
            IdentityRole role = null;
            
            if (result.Succeeded)
            {

                role = await _roleManager.FindByIdAsync(usersDTO.RoleId);

                if (role == null)
                {
                    return NotFound("Role Not Found");
                }

                var userResult = await _userManager.FindByEmailAsync(users.Email);

                if (userResult == null)
                {
                    return NotFound("User Not Found");
                }

                IdentityResult results = await _userManager.AddToRoleAsync(userResult, role.Name);
                
                if (!results.Succeeded)
                {
                    if (results.Errors != null)
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

                await _signInManager.SignInAsync(users, isPersistent: true);
            }

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

            var user = UserToUserDTO(users);

            user.RoleName = role.Name;

            return CreatedAtAction("GetUsers", new { id = users.Id }, user);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<UsersDTO>> Login(Login usersDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var users = await _userManager.FindByEmailAsync(usersDTO.Email);

            if(users == null)
            {
                return NotFound();
            }

            var password = await _userManager.CheckPasswordAsync(users,usersDTO.Password);
            
            if (!password)
            {
                ModelState.AddModelError(string.Empty, "Password Wrong");

                return BadRequest(ModelState);
            }

            await _signInManager.SignInAsync(users, isPersistent: true);
            
            var userDto = UserToUserDTO(users);

            userDto.RoleName = _userManager.GetRolesAsync(users).Result.First();
            
            return Ok(userDto);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return SignOut();
        }

        [HttpPost("changePassword")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult> ChangePassword(UsersDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var users = await _userManager.FindByEmailAsync(userDTO.Email);

            if (users == null)
            {
                return NotFound();
            }

            IdentityResult result = await _userManager.ChangePasswordAsync(users, userDTO.OldPassword, userDTO.Password);

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

        [Authorize]
        private UsersDTO UserToUserDTO(Users user)
        {
            return new UsersDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                Password = user.PasswordHash,
                Email = user.Email,
                Civilite = user.Civilite,
                FirstName = user.FirstName,
                Name = user.Name,
                TelNumber = user.PhoneNumber,
                DateOfBirth = user.DateOfBirth
            };
        }
    }
}