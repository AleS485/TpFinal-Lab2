using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TpFinal_Lab2.Models;
using TpFinal_Lab2.Repositories;

namespace TpFinal_Lab2.Controllers.Api;

[Route("api/[controller]")]
[ApiController]
public class TokenController(UsuarioRepository usuarioRepo, IConfiguration configuration) : ControllerBase
{

    private readonly PasswordHasher<Usuario> hasher = new();

    // POST: api/token/login
    [HttpPost("login")]
    [AllowAnonymous]
    public IActionResult Login([FromBody] LoginView modelo)
    {
        try
        {

            var usuario = usuarioRepo.FindByEmail(modelo.Usuario);
            if (usuario == null)
            {
                return BadRequest("Nombre de usuario o clave incorrecta");
            }
            var ok = hasher.VerifyHashedPassword(usuario, usuario.Password, modelo.Password);

            if (ok == PasswordVerificationResult.Failed)
            {
                return BadRequest("Nombre de usuario o clave incorrecta");
            }

            var secreto = configuration["TokenAuthentication:SecretKey"];
            if (string.IsNullOrEmpty(secreto))
                return StatusCode(500, "Falta configurar TokenAuthentication:SecretKey");
            var key = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(secreto));
            var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Email ?? ""),
                new Claim(ClaimTypes.Role, usuario.Role ?? "Usuario")
            };

            var token = new JwtSecurityToken(
                issuer: configuration["TokenAuthentication:Issuer"],
                audience: configuration["TokenAuthentication:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credenciales
            );

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}