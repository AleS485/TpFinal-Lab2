using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TpFinal_Lab2.Models;
using TpFinal_Lab2.Repositories;

namespace TpFinal_Lab2.Controllers;

public class NoticiaController(DataContext contexto, UsuarioRepository usuariosRepo) : Controller
{

    [HttpGet]
    public IActionResult Index()
    {
        var listaNoticias = contexto.Noticias
            .OrderBy(n => n.Fecha)
            .ToList();

        listaNoticias.Reverse();

        return View(listaNoticias);
    }

    [HttpGet]
    public IActionResult Detalles(int id)
    {

        var noticia = contexto.Noticias
            .FirstOrDefault(n => n.IdNoticia == id);

        if (noticia == null)
        {
            return NotFound();
        }

        var usuario = usuariosRepo.FindById(noticia.UsuarioId);

        if (usuario != null)
        {
            ViewBag.NombreUsuario = $"{usuario.Nombre} {usuario.Apellido}";
        }
        else
        {
            ViewBag.NombreUsuario = "Desconocido";
        }

        return View(noticia);
    }


}
