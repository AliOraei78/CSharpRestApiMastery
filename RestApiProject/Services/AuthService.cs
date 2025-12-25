using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using RestApiProject.Auth;

namespace RestApiProject.Services;

public class AuthService
{
    private readonly string _key = "YourSuperSecretKey1234567890123456"; // At least 32 characters for HS256
    private readonly string _issuer = "BookStoreApi";
    private readonly string _audience = "BookStoreClient";

    // Demo users (use a database in production)
    private readonly Dictionary<string, string> _users = new()
    {
        { "admin", "password123" },
        { "user", "userpass" }
    };

    public AuthResponse? Login(LoginRequest request)
    {
        if (!_users.TryGetValue(request.Username, out var password) || password != request.Password)
            return null;

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_key);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, request.Username),
                new Claim(ClaimTypes.Role, request.Username == "admin" ? "Admin" : "User")
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            Issuer = _issuer,
            Audience = _audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return new AuthResponse
        {
            Token = tokenHandler.WriteToken(token),
            Expiration = token.ValidTo
        };
    }
}
