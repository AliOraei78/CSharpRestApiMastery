using Microsoft.AspNetCore.Mvc;
using RestApiProject.Auth;
using RestApiProject.Services;

namespace RestApiProject.Controllers.V2;

[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Login and get JWT token (v2)
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