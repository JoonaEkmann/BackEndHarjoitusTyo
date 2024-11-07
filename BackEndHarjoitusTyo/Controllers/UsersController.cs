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

namespace BackEndHarjoitusTyo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
public class UsersController : Controller
{
    private readonly iUserService _userService;
        public UsersController(iUserService service)
        {
            _userService = service;
        }

        // GET: api/Users
        /// <summary>
        /// Gets the information of all users in database
        /// </summary>
        /// <returns>List of users</returns>
        [HttpGet]

        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            return Ok(await _userService.GetUsersAsync());
        }

        // GET: api/Users/username
        /// <summary>
        /// Gets a user specified by username
        /// </summary>
        /// <param name="username">Name of the user</param>
        /// <returns>User information for one user or empty</returns>
        [HttpGet("{username}")]
        [Authorize]
        public async Task<ActionResult<UserDTO>> GetUser(string username)
        {
            UserDTO? user = await _userService.GetUserAsync(username);

            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // PUT: api/Users/5
        // To protect from overposting attacks
        /// <summary>
        /// Update user information
        /// </summary>
        /// <param name="username"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPut("{username}")]

    public async Task<ActionResult<UserDTO>> PutUser(string username, User user)
        {
            if (username != user.UserName)
            {
                return BadRequest();
            }

            if (await _userService.UpdateUserAsync(user))
            {
                return NoContent();
            }
            return NotFound();
        }
        // POST: api/Users
        // To protect from overposting attacks
        /// <summary>
        /// Create new user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<UserDTO>> PostUser(User user)
        {
            UserDTO? newUser = await _userService.NewUserAsync(user);
            if (newUser == null)
            {
                return Problem("Username not available");
            }

            return CreatedAtAction("GetUser", new { username = user.UserName }, user);
        }

        // DELETE: api/Users/5
        /// <summary>
        /// Delete user specified by username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpDelete("{username}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(string username)
        {
            if (await _userService.DeleteUserAsync(username))
            { 
            return NoContent(); 
            }
            
            return NotFound();
        }
    }
}

