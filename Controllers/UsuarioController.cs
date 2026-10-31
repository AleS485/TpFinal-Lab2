using Microsoft.AspNetCore.Mvc;
using TpFinal_Lab2.Models;
using TpFinal_Lab2.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace TpFinal_Lab2.Controllers;

public class UsuarioController(UsuarioRepository usuarioRepo) : Controller
{

    private readonly PasswordHasher<Usuario> hasher = new();

    [AllowAnonymous]
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }


    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            ViewBag.Error = "Completa todos los campos";
            return View();
        }

        try
        {
            var usuario = usuarioRepo.FindByEmail(email);

            if (usuario == null)
            {
                ViewBag.Error = "Email o contrasena incorrectos.";
                return View();
            }

            if (!usuario.Estado)
            {
                ViewBag.Error = "Este usuario esta desactivado del sistema.";
                return View();
            }

            var ok = hasher.VerifyHashedPassword(usuario, usuario.Password, password);  
            if (ok != PasswordVerificationResult.Success)
            {
                ViewBag.Error = "Email o contrasena incorrectos";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Email),
                new Claim("FullName", $"{usuario.Nombre} {usuario.Apellido}"),
                new Claim(ClaimTypes.Role, usuario.Role)
            };

            var claimsIdentity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme,
                ClaimTypes.NameIdentifier, ClaimTypes.Role);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity)
            );

            return RedirectToAction("Index", "Home");    
            
        } catch (Exception)
        {
            ViewBag.Error = "Hubo un error cuando intentabas iniciar sesion, proba mas tarde";
            return View();
        }
        
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "Usuario");
    }

    [Authorize]
    [HttpGet]
    public IActionResult Perfil()
    {
        var id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var usuario = usuarioRepo.FindById(id);
        
        if (usuario == null)
        {
            return NotFound();
        }

        return View("Editar", usuario);
    }

    [Authorize(Policy = "Administrador")]
    [HttpGet]
    public IActionResult Registrar()
    {
        return View();
    }


    [Authorize(Policy = "Administrador")]
    [HttpPost]
    public IActionResult Registrar(Usuario usuario, IFormFile? avatarFile)
    {
        if (string.IsNullOrWhiteSpace(usuario.Password))
        {
            ModelState.AddModelError("Password", "La contrasena es obligatoria");
        }

        if (!ModelState.IsValid)
        {
            return View(usuario);
        }

        var usuarioExistente = usuarioRepo.FindByEmail(usuario.Email!);
        if (usuarioExistente != null)
        {
            ModelState.AddModelError("Email", "Ya existe un usuario con ese email");
            return View(usuario);
        }

        usuario.Password = hasher.HashPassword(usuario, usuario.Password!);
        usuarioRepo.Create(usuario);

        if (avatarFile != null && avatarFile.Length > 0)
        {
            usuarioRepo.UploadAvatar(avatarFile, usuario); 
        }

        TempData["Mensaje"] = "Usuario creado con exito";
        return RedirectToAction(nameof(Listar));
    }


    [Authorize(Policy = "Administrador")]
    [HttpGet]
    public IActionResult Editar(int id)
    {
        var usuario = usuarioRepo.FindById(id);
        if (usuario == null) return NotFound();
        return View(usuario);
    }


    [Authorize]
    [HttpPost]
    public IActionResult Editar(Usuario usuario, IFormFile? avatarFile, string? nuevaPassword)
    {
        var actual = usuarioRepo.FindById(usuario.Id);
        if (actual == null) return NotFound();


        if (string.IsNullOrWhiteSpace(usuario.Nombre) ||
        string.IsNullOrWhiteSpace(usuario.Apellido) ||
        string.IsNullOrWhiteSpace(usuario.Email))
        {
            ViewBag.Error = "No podes dejar el nombre, apellido y mail vacios";
            usuario.AvatarUrl = actual.AvatarUrl;
            return View(usuario);
        }

        usuario.AvatarUrl = actual.AvatarUrl;

        int idLogueado = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

        if (!User.IsInRole("Administrador") || usuario.Id == idLogueado)
        {
            usuario.Estado = actual.Estado;

            if (!User.IsInRole("Administrador"))
            {
                usuario.Role = actual.Role;
            }
        }

        if (!string.IsNullOrWhiteSpace(nuevaPassword))
        {
            usuario.Password = hasher.HashPassword(usuario, nuevaPassword);
        }
        else
        {
            usuario.Password = actual.Password;
        }

        if (avatarFile != null)
        {
            usuarioRepo.UploadAvatar(avatarFile, usuario);
        }

        usuarioRepo.Update(usuario);

        if (User.IsInRole("Administrador"))
        {
            return RedirectToAction(nameof(Listar)); 
        }

        return RedirectToAction("Index", "Home");
    }


    [Authorize(Policy = "Administrador")]
    [HttpGet]
    public IActionResult Listar(int pagina = 1)
    {
        int tamano = 10;
        var usuarios = usuarioRepo.ListAll(pagina, tamano);

        ViewBag.Pagina = pagina;
        ViewBag.HaySiguiente = usuarios.Count == tamano;

        return View(usuarios);
    }


    [Authorize(Policy = "Administrador")]
    [HttpGet]
    public IActionResult Eliminar(int id)
    {
        var usuario = usuarioRepo.FindById(id);
        if (usuario == null) return NotFound();
        return View(usuario);
    }


    [Authorize(Policy = "Administrador")]
    [HttpPost]
    public IActionResult Eliminar(Usuario usuario)
    {

        int idLogueado = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

        if (usuario.Id == idLogueado)
        {
            TempData["Error"] = "No te podes borrar a vos mismo.";
            return RedirectToAction(nameof(Listar));
        }

        usuarioRepo.Delete(usuario.Id);
        TempData["Mensaje"] = "El usuario fue dado de baja del sistema.";
        return RedirectToAction(nameof(Listar));
    }

}
