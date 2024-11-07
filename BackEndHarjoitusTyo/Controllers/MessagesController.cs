using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackEndHarjoitusTyo.Models;
using BackEndHarjoitusTyo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using BackEndHarjoitusTyo.Middleware;
using System.Security.Claims;

namespace BackEndHarjoitusTyo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly iMessageService _messageService;
        private readonly iUserAuthenticationService _userAuthenticationService;

        public MessagesController(iMessageService service, iUserAuthenticationService authenticationService)
        {
            _messageService = service;
            _userAuthenticationService = authenticationService;
        }

        // GET: api/Messages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDTO>>> GetMessages()
        {
            return Ok(await _messageService.GetMessagesAsync());
        } 

        // GET: api/Messages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Message>> GetMessage(long id)
        {
            var message = await _messageService.GetMessageAsync(id);

            if (message == null)
            {
                return NotFound();
            }

            return Ok(message);
        }

        // PUT: api/Messages/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMessage(long id, MessageDTO message)
        {
            //tarkistus käsitteleekö kone oikeaa vistiä
            if (id != message.Id)
            {
                return BadRequest();
            }

            bool result = await _messageService.UpdateMessageAsync(message);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        // POST: api/Messages
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MessageDTO>> PostMessage(MessageDTO message)
        {
            MessageDTO? newMessage = await _messageService.NewMessageAsync(message);

            if (newMessage == null)
            {
                return Problem();
            }

            return CreatedAtAction("GetMessage", new { id = message.Id }, message);
        }


        // DELETE: api/Messages/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            string username = this.User.FindFirst(ClaimTypes.Name).Value;
            if (!await _userAuthenticationService.isMyMessage(username, id))
            {
                return BadRequest();
            }
            bool result = await (_messageService.DeleteMessageAsync(id));
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

    }
}
