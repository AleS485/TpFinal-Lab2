using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TpFinal_Lab2.DTO;
using TpFinal_Lab2.Models;
using TpFinal_Lab2.Repositories;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace TpFinal_Lab2.Controllers;

public class VentaController(VentaRepository ventaRepo, JuegoRepository juegoRepo, ClienteRepository clienteRepo, UsuarioRepository usuarioRepo) : Controller
{

    [Authorize]
    [HttpGet]
    public IActionResult Listar(int pagina = 1)
    {
        int tamano = 10;
        var lista = ventaRepo.ListAll(pagina, tamano);

        ViewBag.Pagina = pagina;
        ViewBag.HaySiguiente = lista.Count == tamano;

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
    public IActionResult Registrar(VentaDTO dto)
    {
        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        var juego = juegoRepo.FindById(dto.JuegoId.Value);
        
        if (juego == null || juego.Stock < dto.Cantidad)
        {
            TempData["Error"] = "No hay stock suficiente o el juego no existe";
            return View(dto);
        }

        var cliente = clienteRepo.FindById(dto.ClienteId.Value);
        if (cliente == null)
        {
            TempData["Error"] = "El cliente elegido no existe";
            return View(dto);
        }

        var usuarioIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(usuarioIdString))
        {
            TempData["Error"] = "No se encontro ese usuario";
            return View(dto);
        }

        int usuarioId = Convert.ToInt32(usuarioIdString);

        var usuario = usuarioRepo.FindById(usuarioId); 
        if (usuario == null)
        {
            TempData["Error"] = "El usuario tratando de registrar la venta no existe";
            return View(dto);
        }


        Venta venta = new Venta
        {
            Juego = juego,
            Cliente = cliente,
            Usuario = usuario, 
            Cantidad = dto.Cantidad,
            PrecioTotal = dto.Cantidad * juego.Precio,
            FechaHora = DateTime.Now
        };

        ventaRepo.Create(venta);
        juegoRepo.ActualizarStock(dto.JuegoId.Value, juego.Stock - dto.Cantidad);

        TempData["Mensaje"] = "Se realizo la venta correctamente";
        return RedirectToAction("Listar");
    }

    [Authorize(Policy = "Administrador")]
    [HttpGet]
    public IActionResult Eliminar(int id)
    {
        var venta = ventaRepo.GetById(id);
        if (venta == null)
        {
            return NotFound();
        }
        return View(venta); 
    }

    [Authorize(Policy = "Administrador")]
    [HttpPost]
    public IActionResult Eliminar(Venta venta)
    {
        try
        {
            var ventaBD = ventaRepo.GetById(venta.Id);
            
            if (ventaBD == null)
            {
                TempData["Error"] = "Esa venta no existe";
                return RedirectToAction(nameof(Listar));
            }

            var juego = juegoRepo.FindById(ventaBD.Juego.Id);
            if (juego != null)
            {
                juegoRepo.ActualizarStock(juego.Id, juego.Stock + ventaBD.Cantidad);
            }

            ventaRepo.Delete(venta.Id);
            TempData["Mensaje"] = "La venta fue borrada correctamente y se reestablecio el stock"; 
        } 
        catch(Exception e)
        {
            Console.Error.WriteLine(e.Message);
            TempData["Error"] = "No se pudo borrar esa venta";
        }
        
        return RedirectToAction(nameof(Listar));
    }





}
