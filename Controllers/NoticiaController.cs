using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TpFinal_Lab2.Models;
using TpFinal_Lab2.Repositories;

namespace TpFinal_Lab2.Controllers;

public class NoticiaController(DataContext contexto, UsuarioRepository usuariosRepo) : Controller
{

    [HttpGet]
    public IActionResult Index(int pagina = 1)
    {
        int cantPorPagina = 10;

        int totalNoticias = contexto.Noticias.Count();
        
        var listaNoticias = contexto.Noticias
            .OrderByDescending(n => n.Fecha)
            .Skip((pagina - 1) * cantPorPagina)
            .Take(cantPorPagina)
            .ToList();

        bool haySiguiente = (pagina * cantPorPagina) < totalNoticias;

        ViewBag.Pagina = pagina;
        ViewBag.HaySiguiente = haySiguiente;

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
