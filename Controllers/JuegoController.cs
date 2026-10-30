using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TpFinal_Lab2.DTO;
using TpFinal_Lab2.Models;
using TpFinal_Lab2.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace TpFinal_Lab2.Controllers;

public class JuegoController(JuegoRepository juegoRepo, DesarrolladoraRepository desarrolladoraRepo, ImagenJuegoRepository imagenRepo) : Controller
{

    [Authorize]
    [HttpGet]
    public IActionResult Listar(int pagina = 1)
    {
        int tamano = 10; 
        var lista = juegoRepo.ListAll(pagina, tamano); 
    
        return View(lista);
    }


    [Authorize]
    [HttpGet]
    public IActionResult Registrar()
    {
        return View();
    }

    [Authorize]
    [HttpPost]
    public IActionResult Registrar(JuegoDTO dto)
    {
        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        var desarrolladora = desarrolladoraRepo.FindById(dto.DesarrolladoraId ?? 0);
        
        if (desarrolladora == null)
        {
            ModelState.AddModelError("DesarrolladoraId", "La desarrolladora seleccionada no existe");
        }

        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        Juego juego = new Juego
        {
            Titulo = dto.Titulo,
            Genero = dto.Genero,
            Descripcion = dto.Descripcion,
            Precio = dto.Precio.Value,
            Stock = dto.Stock.Value,
            Estado = dto.Estado,
            Desarrolladora = desarrolladora 
        };

        try
        {
            juegoRepo.Create(juego);    
            return RedirectToAction(nameof(Listar));
        } catch(Exception e)
        {
            Console.Error.WriteLine(e.Message);
            ModelState.AddModelError("", "ERROR GUARDANDO EL JUEGO EN LA BD");
            return View(dto);
        }
    }

    [Authorize]
    [HttpGet]
    public IActionResult Editar(int id)
    {
        var juego = juegoRepo.FindById(id);
        if(juego == null)
        {
            return NotFound();
        }

        var dto = new JuegoDTO
        {
            Id = juego.Id,
            Titulo = juego.Titulo,
            Genero = juego.Genero,
            Descripcion = juego.Descripcion,
            Precio = juego.Precio,
            Stock = juego.Stock,
            Estado = juego.Estado,
            DesarrolladoraId = juego.Desarrolladora?.Id ?? 0
        };

        ViewBag.DesarrolladoraNombre = juego.Desarrolladora != null ? juego.Desarrolladora.Nombre : "Sin desarrolladora";
        return View(dto);
    }

    [Authorize]
    [HttpPost]
    public IActionResult Editar(JuegoDTO dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.DesarrolladoraNombre = "";
            return View(dto);
        }

        var desarrolladora = desarrolladoraRepo.FindById(dto.DesarrolladoraId ?? 0);
        if (desarrolladora == null)
        {
            ModelState.AddModelError("DesarrolladoraId", "Esa desarrolladora no existe");
        }

        if (!ModelState.IsValid)
        {
            ViewBag.DesarrolladoraNombre = "";
            return View(dto);
        }

        Juego juego = new Juego
        {
            Id = dto.Id,
            Titulo = dto.Titulo,
            Genero = dto.Genero,
            Descripcion = dto.Descripcion,
            Precio = dto.Precio.Value,
            Stock = dto.Stock.Value,
            Estado = dto.Estado,
            Desarrolladora = desarrolladora 
        };

        try
        {
            juegoRepo.Update(juego);    
            return RedirectToAction(nameof(Listar));
        } catch(Exception e)
        {
            Console.Error.WriteLine(e.Message);
            ModelState.AddModelError("", "ERROR ACTUALIZANDO EL JUEGO EN LA BD");
            ViewBag.DesarrolladoraNombre = "";
            return View(dto);
        }

    }


    [Authorize(Policy = "Administrador")]
    [HttpGet]
    public IActionResult Eliminar(int id)
    {
        var juego = juegoRepo.FindById(id);
        if (juego == null)
        {
            return NotFound();
        }
        return View(juego); 
    }

    [Authorize(Policy = "Administrador")]
    [HttpPost]
    public IActionResult Eliminar(Juego juego)
    {
        if (juego.Id <= 0)
        {
            return RedirectToAction(nameof(Listar));
        }

        try
        {
            juegoRepo.Delete(juego.Id);
            TempData["Mensaje"] = "Se borro el juego correctamente"; 
        } 
        catch(Exception e)
        {
            Console.Error.WriteLine(e.Message);
            TempData["Error"] = "No se pudo borrar el juego";
        }
        
        return RedirectToAction(nameof(Listar));
    }


    [Authorize]
    [HttpGet]
    public JsonResult BuscarFiltrados(string filtro)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(filtro))
            {
                return Json(new List<Juego>());
            }

            var juegos = juegoRepo.BuscarFiltrados(filtro);
            return Json(juegos);
        }
        catch (Exception)
        {
            return Json(new List<Juego>()); 
        }
    }

    [Authorize]
    public IActionResult Imagenes(int id)
    {
        var juego = juegoRepo.FindById(id);

        if(juego == null)
        {
            return NotFound();
        }

        return View(juego);
    }

    [Authorize]
    [HttpGet]
    public IActionResult Detalles(int id)
    {
        var juego = juegoRepo.FindById(id); 

        if (juego == null)
        {
            return NotFound();
        }

        imagenRepo.Load(juego);

        return View(juego);
    }


}
