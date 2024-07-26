using BE_ASP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BE_ASP.Data;
using BE_ASP.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BE_ASP.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class UserController : ControllerBase
  {
    private readonly AppDBContext _context;
    public UserController(AppDBContext context)
    {
      _context = context;
    }
    
    // GET with optional query parameters
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers(
      [FromQuery] string? name, 
      [FromQuery] int? id, 
      [FromQuery] string? email
    )
    {
      if (_context.Users == null)
      {
        return NotFound();
      }

      var query = _context.Users.AsQueryable();

      if (!string.IsNullOrEmpty(name))
      {
        query = query.Where(u => u.Name.Contains(name));
      }

      if (id.HasValue)
      {
        query = query.Where(u => u.Id == id.Value);
      }

      if (!string.IsNullOrEmpty(email))
      {
        query = query.Where(u => u.Email.Contains(email));
      }

      var users = await query.Select(u => new UserDto
      {
        Id = u.Id,
        Name = u.Name,
        Email = u.Email,
        Phone = u.Phone,
      }).ToListAsync();

      return users;
    }

    // GET: api/User/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUser(int id)
    {
      var user = await _context.Users.FindAsync(id);
      if (user == null)
      {
        return NotFound();
      }
      var users = new UserDto
      {
        Id = user.Id,
        Name = user.Name,
        Email = user.Email,
        Phone = user.Phone,
        Password = user.Password
      };

      return users;
    }

    // POST: api/User
    [HttpPost]
    public async Task<ActionResult<UserDto>> PostUser([FromBody] UserDto newUser)
    {
      try
      {
        var user = new User
        {
          Name = newUser.Name,
          Email = newUser.Email,
          Phone = newUser.Phone,
          Password = newUser.Password
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var userDto = new UserDto
        {
          Id = user.Id,
          Name = user.Name,
          Email = user.Email,
          Phone = user.Phone
        };

        return CreatedAtAction("GetUser", new { id = user.Id }, userDto);
      } 
      catch
      {
        return BadRequest();
      }
    }

    // PUT: api/User/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> PutUser(int id, [FromBody] UserDto user)
    {
        var foundedUser = await _context.Users.FindAsync(id);
        if (foundedUser == null)
        {
            return NotFound();
        }

        if (!string.IsNullOrEmpty(user.Name)) foundedUser.Name = user.Name;
        if (!string.IsNullOrEmpty(user.Email)) foundedUser.Email = user.Email;
        if (!string.IsNullOrEmpty(user.Phone)) foundedUser.Phone = user.Phone;
        if (!string.IsNullOrEmpty(user.Password)) foundedUser.Password = user.Password;

        _context.Entry(foundedUser).State = EntityState.Modified;

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

    private bool UserExists(int id)
    {
      return _context.Users.Any(e => e.Id == id);
    }

    // DELETE: api/User/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
      var user = await _context.Users.FindAsync(id);
      if (user == null)
      {
        return NotFound();
      }

      _context.Users.Remove(user);
      await _context.SaveChangesAsync();
      return NoContent();
    }
  }
}
