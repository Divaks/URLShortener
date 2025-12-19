using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Core.Entities;
using UrlShortener.Web.Models;

namespace UrlShortener.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            return BadRequest("Користувач з таким email вже існує.");
        }

        var user = new User
        {
            UserName = request.Email,
            Email = request.Email
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        await _signInManager.SignInAsync(user, isPersistent: true);

        return Ok(new { message = "Реєстрація успішна!" });
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult> Login([FromBody] LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, 
                model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {
                return Ok(new { msg = "Success" });
            }
            
            return BadRequest("Користувача не знайдено.");
        }

        return BadRequest(ModelState);
    }

    [HttpPost("logout")]
    public async Task<ActionResult> LogOut()
    {
        await _signInManager.SignOutAsync();
        return Ok(new { msg = "Success" });
    }
    
    [HttpGet("test")]
    [AllowAnonymous]
    public IActionResult Test() => Ok("API works");
    
    [HttpPost("make-me-admin")]
    [Authorize]
    public async Task<IActionResult> MakeMeAdmin()
    {
        if (!await _roleManager.RoleExistsAsync("Admin"))
        {
            await _roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        await _userManager.AddToRoleAsync(user, "Admin");

        await _signInManager.SignOutAsync();
        await _signInManager.SignInAsync(user, isPersistent: true);

        return Ok(new { message = "Вітаю! Ви тепер Адміністратор. (Куки оновлено)" });
    }
}