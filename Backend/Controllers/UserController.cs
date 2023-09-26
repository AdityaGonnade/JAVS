using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using JWT_Token_Example.Context;
using JWT_Token_Example.Helpers;
using JWT_Token_Example.Models;
using JWT_Token_Example.Models.DTO;
using JWT_Token_Example.UtilityService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace JWT_Token_Example.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly AppDbContext _authContext;
    private readonly IConfiguration _configuration;
    private readonly IEmailService _emailService;

    public UserController(AppDbContext appDbContext, IConfiguration configuration, IEmailService emailService)
    {
        _authContext = appDbContext;
        _configuration = configuration;
        _emailService = emailService;
    }
    [AllowAnonymous]
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

            if (!PasswordHasher.VerifyPassword(userObj.Password, user.Password ))
            {
                return BadRequest(new { Message = "Password is Incorrect" });
            }
            
            user.Token = JwtController.CreateJwt(user);

            return Ok(new
            {
                Token = user.Token,
                Role = user.Role,
                Message = "Login Success!"
            });
    }

    [AllowAnonymous]
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

    
    
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<User>> GetAllUsers()
    {
        return Ok(await _authContext.Users.ToListAsync());
    }
    
    [HttpPost("send-reset-email/{email}")] 
    public async Task<IActionResult> SendEmail(string email)
    {
        var user = await _authContext.Users.FirstOrDefaultAsync(a => a.Email == email);
        if (user is null)
        {
            return NotFound(new
            {
                StatusCode = 404,
                Message = "Email Doesn't Exist"
            });
        }

        var tokenBytes = RandomNumberGenerator.GetBytes(64);
        var emailToken = Convert.ToBase64String(tokenBytes);
        user.ResetPasswordToken = emailToken;
        user.ResetPasswordExpiry = DateTime.Now.AddMinutes(15);
        string from = _configuration["EmailSettings:From"];
        var emailModel = new EmailModel(email, "Reset Password!", EmailBody.EmailStringBody(email, emailToken));
        _emailService.SendEmail(emailModel);
        _authContext.Entry(user).State = EntityState.Modified;
        await _authContext.SaveChangesAsync();
        return Ok(new
        {
            StatusCode = 200,
            Message = "Email Sent!"
        });
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
    {
        var newToken = resetPasswordDto.EmailToken.Replace(" ", "+");
        var user = await _authContext.Users.AsNoTracking().FirstOrDefaultAsync(a => a.Email == resetPasswordDto.Email);
        if (user is null)
        {
            return NotFound(new
            {
                StatusCode = 404,
                Message = "User Doesn't Exist"
            });
        }

        var tokenCode = user.ResetPasswordToken;
        DateTime emailTokenExpiry = user.ResetPasswordExpiry;
        if (tokenCode != resetPasswordDto.EmailToken || emailTokenExpiry < DateTime.Now)
        {
            return BadRequest(new
            {
                StatusCode = 400,
                Message = " Invalid Reset Link"
            });
        }

        user.Password = PasswordHasher.HashPassword(resetPasswordDto.NewPassword);
        _authContext.Entry(user).State = EntityState.Modified;
        await _authContext.SaveChangesAsync();
        return Ok(new
        {
            StatusCode = 200,
            Message = "Password Reset Successfully"
        });
    }

}