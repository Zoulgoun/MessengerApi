using MessengerApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace MessengerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly AppDbContext _context;

        
        public MessagesController(AppDbContext context)
        {
            _context = context;
        }

        
        [HttpGet("all")]
        public IEnumerable<Message> GetMessages()
        {
                        
            return _context.Messages.ToList();

        }

        
        [HttpGet("{id}")]
        public IActionResult GetMessageById(int id)
        {
            var message = _context.Messages.Find(id);
            if (message == null)
                return NotFound();  

            return Ok(message);     
        }

        
        [HttpPost]
        [Authorize]
        public ActionResult<Message> PostMessage(Message message)
        {
            _context.Messages.Add(message);
            _context.SaveChanges();

            return Ok("Сообщение отправлено!");     
                                    
        }


        
        [HttpPut("{id}")]
        public IActionResult UpdateMessage(int id, [FromBody] Message updatedMessage)
        {
            var existingMessage = _context.Messages.Find(id);
            if (existingMessage == null)
                return NotFound(); 

            
            existingMessage.Content = updatedMessage.Content;
            
            
            _context.Entry(existingMessage).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            return NoContent(); 
        }

        
        [HttpDelete("{id}")]
        public IActionResult DeleteMessage(int id)
        {
            var messageToRemove = _context.Messages.Find(id);
            if (messageToRemove == null)
                return NotFound(); 

            _context.Messages.Remove(messageToRemove);
            _context.SaveChanges();

            return NoContent(); 
        }
    }
}

        
    

