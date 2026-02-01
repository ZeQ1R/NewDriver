using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewDriver.Data;
using NewDriver.DTOs.Request;
using NewDriver.DTOs.Response;

namespace NewDriver.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly AppDbContext _context;
    private readonly TokenService _tokenService;

    public AuthController(
        UserManager<IdentityUser> userManager,
        AppDbContext context,
        TokenService tokenService)
    {
        _userManager = userManager;
        _context = context;
        _tokenService = tokenService;
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegistrationRequestDTO request)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            return BadRequest("User already exists");
        }

        var newUser = new IdentityUser
        {
            UserName = request.Email,
            Email = request.Email
        };

        var result = await _userManager.CreateAsync(newUser, request.Password);

        if (!result.Succeeded)
        {
           
            return BadRequest(result.Errors);
        }

        var userInDb = new User
        {
            Email = request.Email,
            Name = request.Email,      
            LastName = "Not Provided",  
            PhoneNumber = "+38900000000", 
            EMBG = "0000000000000",    
            DOB = DateTime.UtcNow.AddYears(-18), 
            Role = "Applicant",
            Submitted = "Pending"
        };

        _context.Users.Add(userInDb);
        await _context.SaveChangesAsync();

        return Ok("User registered successfully");
    }

    //[HttpPost("role")]
    //public async Task<IActionResult> CreateRoles(string roleName)
    //{
    //    if(!await _roleManager.RoleExistsAsync(roleName))
    //    {
    //        await _roleManager.CreateAsync(new IdentityRole(roleName));
    //    }
    //    return Ok("Role created successfully");
    //}

    //[HttpPost("assign")]
    //public async Task<ActionResult> AssignRoleToUser(string email, string roleName)
    //{
    //    var user = await _userManager.FindByEmailAsync(email)
    //            ?? throw new ApplicationException("No user found");

    //    if(!await _roleManager.RoleExistsAsync(roleName))
    //    {
    //        throw new ApplicationException("No role found");
    //    }

    //    if(!await _userManager.IsInRoleAsync(user, roleName))
    //    {
    //        await _userManager.AddToRoleAsync(user, roleName);
    //    }
    //    return Ok();
    //}

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDTO request)
    {
        
        var managedUser = await _userManager.FindByEmailAsync(request.Email);
        if (managedUser == null)
        {
            return BadRequest("No user found with that email");
        }

        
        var isPasswordValid = await _userManager.CheckPasswordAsync(managedUser, request.Password);
        if (!isPasswordValid)
        {
            return BadRequest("Invalid password");
        }

        
        var userInDb = _context.Users.FirstOrDefault(u => u.Email == request.Email);

        if (userInDb == null)
        {
            return Unauthorized("User exists in Identity but not in custom database.");
        }

        
        var accessToken = await _tokenService.CreateToken(userInDb);
        await _context.SaveChangesAsync();

        return Ok(new LoginResponseDTO
        {
            Token = accessToken,
            Name = userInDb.Name,
            Email = userInDb.Email
        });
    }
}