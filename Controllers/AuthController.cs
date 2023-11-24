using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthController(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    // POST: api/Auth/GetToken
    [HttpPost("GetToken")]
    public IActionResult GetToken([FromBody] UsuarioLogin usuarioLogin)
    {
        var usuario = _context.Usuarios
            .SingleOrDefault(u => u.NombreUsuario == usuarioLogin.NombreUsuario && u.Contraseña == usuarioLogin.Contraseña);

        if (usuario == null)
        {
            return Unauthorized();
        }

        var token = GenerateToken(usuario);

        return Ok(new { Token = token });
    }

    private string GenerateToken(Usuario usuario)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, usuario.NombreUsuario),
            new Claim(ClaimTypes.Email, "correo@ejemplo.com"), // Puedes cambiar esto con el correo real del usuario
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Issuer"],
            claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
