using MessengerApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace MessengerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }


        

       
        [HttpGet]
        public IEnumerable<User> GetUsers()
        {
            return _context.Users.ToList();
        }

       
        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.id == id);
            if (user == null)
                return NotFound(); 

            return Ok(user);       
        }

        
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var userToDelete = _context.Users.FirstOrDefault(u => u.id == id);
            if (userToDelete == null)
                return NotFound(); 

            _context.Users.Remove(userToDelete);
            _context.SaveChanges();

            return NoContent(); 
        }
    }
}