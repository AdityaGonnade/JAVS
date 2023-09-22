using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using JWT_Token_Example.Context;
using JWT_Token_Example.Helpers;
using JWT_Token_Example.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace JWT_Token_Example.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly AppDbContext _authContext;

    public UserController(AppDbContext appDbContext)
    {
        _authContext = appDbContext;
    }
    
    [HttpPost("authenticate")]
    public async Task<IActionResult> Authenticate([FromBody] User userObj)
    {
        //check if user object is null or not
        if (userObj == null)
        {
            return BadRequest(new
            {
                Message = "userObj is null"
            });
        }

        // check is user is present in database
        var user = await _authContext.Users.FirstOrDefaultAsync(x => x.UserName == userObj.UserName);

        // if user not present in database return message
        if (user == null)
        {
            return NotFound(new { Message = "User Not Found!" });
        }

        if (!PasswordHasher.VerifyPassword(userObj.Password, user.Password))
        {
            return BadRequest(new { Message = "Password is Incorrect" });
        }
        //
        // user.Token = CreateJwt(user);
        //
        return Ok(new
        {
            // Token = user.Token,
            Message = "Login Success!"
        });
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] User userObj)
    {
        Console.WriteLine(userObj);
        if (userObj == null)
        {
            
            return BadRequest(new
            {
                Message = "userobj is null"
            });
        }

        
        //check Username
        
        if (await CheckUserNameExistAsync(userObj.UserName))
        {
            return BadRequest(new
            {
                Message= "Username Already Exist!"
            });
        }

        //check email
        
        if (await CheckEmailExistAsync(userObj.Email))
        {
            return BadRequest(new
            {
                Message= "Email Already Exist!"
            });
        }
        
        //check password strength
        var pass = CheckPasswordStrength(userObj.Password);
        if (!string.IsNullOrEmpty(pass))
        {
            return BadRequest(new
            {
                Message = pass.ToString()
            });
        }

        userObj.Password = PasswordHasher.HashPassword(userObj.Password);
        userObj.Role = "User";
        userObj.Token = "";
        
        await _authContext.Users.AddAsync(userObj);
        await _authContext.SaveChangesAsync();
        return Ok(new
        {
            Message = "User Registered!"
        });
    }

    private async Task<bool> CheckUserNameExistAsync(string username)
    {
        return await _authContext.Users.AnyAsync(x => x.UserName == username);
    }

    private async Task<bool> CheckEmailExistAsync(string email)
    {
        return await _authContext.Users.AnyAsync(x => x.Email == email);
    }

    private string CheckPasswordStrength(string password)
    {
        StringBuilder sb = new StringBuilder();
        if (password.Length < 8)
        {
            sb.Append("Minimum password length should be 8" + Environment.NewLine);
        }

        if (!(Regex.IsMatch(password, "[a-z]") && Regex.IsMatch(password, "[A-Z]")
                                              && Regex.IsMatch(password, "[0-9]")))
        {
            sb.Append("Password Should be Alphanumeric" + Environment.NewLine);
        }

        if (!(Regex.IsMatch(password, "[<,>@!#$%^&*()_+\\[\\]{}?:;|'./~`=]")))
        {
            sb.Append("Password should contain special char" + Environment.NewLine);
        }

        return sb.ToString();
    }

    private string CreateJwt(User user)
    {
        //header.payload.signature
        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("veryverysecret...");
        var identity = new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}")
        });

        var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = identity,
            Expires = DateTime.Now.AddDays(1),
            SigningCredentials = credentials
        };

        var token = jwtTokenHandler.CreateToken(tokenDescriptor);
        return jwtTokenHandler.WriteToken(token);

    }
    
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<User>> GetAllUsers()
    {
        return Ok(await _authContext.Users.ToListAsync());
    }

}