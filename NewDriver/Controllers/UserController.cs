
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewDriver.Data;
using NewDriver.DTOs.Request;
using NewDriver.Repositories.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace NewDriver.Controllers
{
    [Route("api")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userrepository;

        public UserController(IUserRepository userrepository)
        {
            _userrepository = userrepository;
        }


        [HttpGet("users")]
        public IActionResult GetUsers()
        {
            var users = _userrepository.GetUsers();
            return Ok(users);
        }

        [HttpGet("userByEmail/{email}")]
        public IActionResult GetUserByEmail(string email)
        {
            var user = _userrepository.GetUserByEmail(email);
            if (user == null)
            {
                return NotFound($"User with email {email} not found.");
            }
            return Ok(user);
        }

        [HttpPost("createUser")]
        public IActionResult AddUser([FromBody] User user)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var existingUser = _userrepository.GetUserByEmail(user.Email);
                if (existingUser != null)
                {
                    return BadRequest("User with this email already exists.");
                }

                // PostgreSQL Fix: Ensure UTC
                user.DOB = DateTime.SpecifyKind(user.DOB, DateTimeKind.Utc);

                var result = _userrepository.AddUser(user);
                return Ok(new { message = "Successfully added", result });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error saving user: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto dto)
        {
            var user = _userrepository.GetUserByEmail(dto.Email);
            if (user == null || user.Password != dto.Password)
            {
                return Unauthorized("Invalid email or password");
            }
            return Ok(user);
        }

        [HttpDelete("deleteUser/{id:int}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _userrepository.GetUserById(id);
            if (user == null)
            {
                return NotFound($"User with ID {id} does not exist.");
            }

            _userrepository.DeleteUser(id);
            return Ok(new { message = "Successfully deleted", userId = id });
        }

        [HttpPut("userUpdate/{id:int}")]
        public IActionResult UpdateUser(int id, [FromBody] User user)
        {
            var existingUser = _userrepository.GetUserById(id);
            if (existingUser == null)
            {
                return NotFound($"Cannot update. User with ID {id} does not exist.");
            }

            
            user.Id = id;
            user.DOB = DateTime.SpecifyKind(user.DOB, DateTimeKind.Utc);

            _userrepository.UpdateUser(user);
            return Ok("Successfully updated");
        }

        [HttpPut("acceptUser/{id:int}")]
        [AllowAnonymous] 
        public IActionResult AcceptUser(int id)
        {
            var user = _userrepository.GetUserById(id);
            if (user == null)
            {
                return NotFound($"Cannot accept. User with ID {id} does not exist.");
            }

            _userrepository.AcceptUser(id);
            return Ok("User status updated to: Accepted");
        }

        [HttpPut("denyUser/{id:int}")]
        public IActionResult DenyUser(int id)
        {
            var user = _userrepository.GetUserById(id);
            if (user == null)
            {
                return NotFound($"Cannot deny. User with ID {id} does not exist.");
            }

            _userrepository.DenyUser(id);
            return Ok("User status updated to: Denied");
        }

        [HttpPut("submitUser")]
        public IActionResult SubmitUser([FromBody] SubmitDto dto)
        {
            try
            {
                var user = _userrepository.GetUserByEmail(dto.Email);
                if (user == null) return NotFound("User email not found.");

                _userrepository.SubmitUser(dto.Email, dto.Provimi, dto.Submitted);
                return Ok("Submitted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }


    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class SubmitDto
    {
        public string Email { get; set; }
        public string Provimi { get; set; }
        public string Submitted { get; set; }
    }
}