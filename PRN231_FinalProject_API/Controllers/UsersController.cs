using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PRN231_FinalProject_API.Models;

namespace PRN231_FinalProject_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly PRN221_ProjectContext _context;
        private readonly IConfiguration _configuration;

        public UsersController(PRN221_ProjectContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/Users
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }
        private string GenerateToken(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var ExpiryInDays = Convert.ToInt32(_configuration["Jwt:ExpiryInDays"]);
            var Issuer = _configuration["Jwt:Issuer"];
            var Audience = _configuration["Jwt:Audience"];
            var secretKey = _configuration["Jwt:Key"];
            var secretKeyByte = Encoding.UTF8.GetBytes(secretKey);
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Username", user.Username),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("Id", user.UserId.ToString()),
                    new Claim("TokenId", Guid.NewGuid().ToString()),
                }),
                Expires = DateTime.UtcNow.AddDays(ExpiryInDays),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyByte), SecurityAlgorithms.HmacSha512Signature),
                Issuer = Issuer,
                Audience = Audience,
            };
            var token = jwtTokenHandler.CreateToken(tokenDescription);
            return jwtTokenHandler.WriteToken(token);
        }
        [HttpGet("{UserName}/{Password}")]
        public async Task<IActionResult> GetUserByUserNameAndPassWord(string UserName, string Password)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.Where(u => u.Username.Equals(UserName) && u.Password.Equals(Password)).FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            return Ok(new
            {
                User = user,
                Message = "Thành Công Đăng Nhập!",
                IsSuccess = true,
                Data = GenerateToken(user)
            });
        }
        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'PRN221_ProjectContext.Users'  is null.");
            }
            var userexist = await _context.Users.Where(u => u.Username.Equals(user.Username)).FirstOrDefaultAsync();
            if(userexist!=null)
            {
                return Problem("User Exist.");
            }
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.UserId == id)).GetValueOrDefault();
        }

        [HttpGet("total/{id}")]
        public async Task<ActionResult<decimal>> GetTotalBalance(int id)
        {
            var currentUser = await _context.Users.FindAsync(id);

            if (currentUser == null)
            {
                return Unauthorized();
            }

            return Ok(currentUser.Balance);
        }
    }
}
