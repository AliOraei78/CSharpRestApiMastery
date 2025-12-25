using Microsoft.AspNetCore.Mvc;
using RestApiProject.Auth;
using RestApiProject.Services;

namespace RestApiProject.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Login and get JWT token
    /// </summary>
    [HttpPost("login")]
    public ActionResult<AuthResponse> Login(LoginRequest request)
    {
        var response = _authService.Login(request);
        if (response is null)
            return Unauthorized("Invalid username or password");

        return Ok(response);
    }
}