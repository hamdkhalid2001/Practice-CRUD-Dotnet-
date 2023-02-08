using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PracticeCRUD.Models;

namespace PracticeCRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly StoreDBContext _dBContext;
        public UsersController(StoreDBContext dBContext)
        {
            _dBContext = dBContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Users>>> GetUsers()
        {
            if (_dBContext.Users == null)
            {
                return NotFound();
            }
            return await _dBContext.Users.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Users>> GetUser(int id)
        {
            if (_dBContext.Users == null)
            {
                return NotFound();
            }

            var user = await _dBContext.Users.FindAsync(id);
            if(user == null) { 
                return NotFound();
            }
            return user;
        }

        [HttpPost]
        public async Task<ActionResult<Users>> PostUser(Users user)
        {
            if (_dBContext.Users == null)
            {
                return NotFound();
            }
            _dBContext.Users.Add(user);
            await _dBContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUser), new {id = user.Id}, user);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> EditUser(int id,Users user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }
            _dBContext.Entry(user).State = EntityState.Modified;
            try
            {
                await _dBContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IsUserAvailable(id))
                {
                    return NotFound();
                }                
            }
            return Ok();

        }
        private bool IsUserAvailable(int id)
        {
            return (_dBContext.Users?.Any(u => u.Id == id)).GetValueOrDefault();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            if(_dBContext.Users == null)
            {
                return NotFound();
            }

            var user = await _dBContext.Users.FindAsync(id);
            if(user == null)
            {
                return NotFound();
            }
            _dBContext.Users.Remove(user);
            await _dBContext.SaveChangesAsync();
            return Ok();

        }
    }
}
